using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    #region VARIABLES
    [Header("���� �ʱⰪ ������")]
    public List<MonsterUnitData> MonsterInitData = new List<MonsterUnitData>();

    [Header("���Ͱ� �̵� �� ���")]
    private List<Queue<Vector3>> RouteDataQueueList = new List<Queue<Vector3>>();

    [Header("����Ʈ Ÿ�ϸ�")]
    public Tilemap PointTilemap;

    [Header("����Ʈ Ÿ�� ����")]
    public Sprite StartPointTileSprite;
    public Sprite EndPointTileSprite;
    public Sprite CheckPointTileSprite;

    [Header("���� Ƚ��")]
    private int _spawnCount = 0;

    [Header("�������� ������")]
    private StageData _stageData;

    [Header("���� ������������ ��ȯ�� ���� ����")]
    private MonsterUnitData _monsterUnitData;
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        // RouteProcessing();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<BeginSpawnMonsterEvent>(SpawnMonster);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<BeginSpawnMonsterEvent>(SpawnMonster);
    }
    #endregion





    #region FUNCTIONS
    public void SpawnMonster(BeginSpawnMonsterEvent e)
    {
        _stageData       = e.StageData;
        _monsterUnitData = e.MonsterUnitData;

        // ���尡 ���۵Ǹ� ������ HP�� ������
        _monsterUnitData.MaxHealth = _stageData.dHealth;
        _monsterUnitData.Health    = _stageData.dHealth;
        _monsterUnitData.MoveSpeed = _stageData.MoveSpeed;

        StartCoroutine(SpawnMonsterWithDelay(e));
    }

    IEnumerator SpawnMonsterWithDelay(BeginSpawnMonsterEvent e)
    {
        while(_spawnCount < _stageData.NumOfMonster)
        {
            ++_spawnCount;

            MonsterUnit monsterUnit = Instantiate(_monsterUnitData.UnitPrefab, RouteDataQueueList[Random.Range(0,100) < 70? 0 : 1].Peek(), Quaternion.identity);
            monsterUnit.SetInitialData(_monsterUnitData, RouteDataQueueList[Random.Range(0, 100) < 70 ? 0 : 1]);

            yield return new WaitForSeconds(_stageData.MonsterSpawnInterval);
        }

        _spawnCount = 0;
    }

    //private void RouteProcessing()
    //{
    //    Vector3       _startPoint  = new Vector3();
    //    Vector3       _endPoint    = new Vector3();
    //    List<Vector3> _checkPoints = new List<Vector3>();

    //    // Ÿ�ϸ��� ��ȸ�ϸ鼭(���ϴܺ��� ���� �������� ��ȸ��)
    //    foreach (Vector3Int pointTilePos in PointTilemap.cellBounds.allPositionsWithin)
    //    {
    //        // ������ Ÿ����
    //        Tile pointTile = PointTilemap.GetTile(pointTilePos) as Tile;

    //        // ��������Ʈ�� ������ ��������Ʈ�� ���ٸ�
    //        if(pointTile != null && pointTile.sprite == StartPointTileSprite)
    //        {
    //            _startPoint = PointTilemap.GetCellCenterWorld(pointTilePos);
    //        }
    //        // ��������Ʈ�� ������ ��������Ʈ�� ���ٸ�
    //        if (pointTile != null && pointTile.sprite == EndPointTileSprite)
    //        {
    //            _endPoint = PointTilemap.GetCellCenterWorld(pointTilePos);
    //        }
    //        // ��������Ʈ�� üũ����Ʈ ��������Ʈ�� ���ٸ�
    //        if (pointTile != null && pointTile.sprite == CheckPointTileSprite)
    //        {
    //            _checkPoints.Add(PointTilemap.GetCellCenterWorld(pointTilePos));
    //        }
    //    }

    //    // ���Ͱ� �̵��ؾ� �� ��θ� Queue�� �����
    //    // �̰� ������ ���� ���� ���� �ɵ�
    //    {
    //        RouteQueue.Enqueue(_startPoint);
    //        RouteQueue.Enqueue(_checkPoints[7]);
    //        RouteQueue.Enqueue(_checkPoints[8]);
    //        RouteQueue.Enqueue(_checkPoints[5]);
    //        RouteQueue.Enqueue(_checkPoints[6]);
    //        RouteQueue.Enqueue(_checkPoints[1]);
    //        RouteQueue.Enqueue(_checkPoints[0]);
    //        RouteQueue.Enqueue(_checkPoints[3]);
    //        RouteQueue.Enqueue(_checkPoints[2]);
    //        RouteQueue.Enqueue(_checkPoints[4]);
    //        RouteQueue.Enqueue(_endPoint);
    //    }
    //}

    public void RouteDataProcessing(string routeData)
    {
        List<Vector3> points = new List<Vector3>();

        // Ÿ�ϸ��� ��ȸ�ϸ鼭(���ϴܺ��� ���� �������� ��ȸ��)
        foreach (Vector3Int pointTilePos in PointTilemap.cellBounds.allPositionsWithin)
        {
            // ������ Ÿ����
            Tile pointTile = PointTilemap.GetTile(pointTilePos) as Tile;

            // ��������Ʈ null�� �ƴ϶��
            if(pointTile != null)
            {
                // ����, ����, üũ����Ʈ ��������Ʈ�� ���ٸ�

                if (pointTile.sprite == StartPointTileSprite ||
                    pointTile.sprite == EndPointTileSprite   ||
                    pointTile.sprite == CheckPointTileSprite)
                {
                    points.Add(PointTilemap.GetCellCenterWorld(pointTilePos));
                }
            }
        }

        // 2������ ��Ʈ
        string[] routeList = routeData.Split("&");

        for(int i = 0; i < routeList.Length; ++i)
        {
            string[] routeIndices = routeList[i].Split(",");

            Queue<Vector3> routeQueue = new Queue<Vector3>();

            foreach (string index in routeIndices)
            {
                int routeIndex = int.Parse(index);

                routeQueue.Enqueue(points[routeIndex]);

            }

            RouteDataQueueList.Add(routeQueue);
        }
    }
    #endregion
}
