using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    #region VARIABLES
    [Header("몬스터 초기값 데이터")]
    public List<MonsterUnitData> MonsterInitData = new List<MonsterUnitData>();

    [Header("몬스터가 이동 할 경로")]
    private Queue<Vector3> RouteQueue = new Queue<Vector3>();

    [Header("포인트 타일맵")]
    public Tilemap PointTilemap;

    [Header("포인트 타일 변수")]
    public Sprite StartPointTileSprite;
    public Sprite EndPointTileSprite;
    public Sprite CheckPointTileSprite;
    #endregion





    #region TEMP->JSON
    [Header("스폰 횟수")]
    private int _spawnCount = 0;
    #endregion




    #region MONOBEHAVIOUR
    private void Awake()
    {
        RouteProcessing();    
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
        StartCoroutine(SpawnMonsterWithDelay(e));
    }

    IEnumerator SpawnMonsterWithDelay(BeginSpawnMonsterEvent e)
    {
        while(_spawnCount < JGameManager.Instance.NumOfMonster)
        {
            ++_spawnCount;

            MonsterUnit monsterUnit = Instantiate(MonsterInitData[0].UnitPrefab, RouteQueue.Peek(), Quaternion.identity);
            monsterUnit.SetInitialData(MonsterInitData[0], RouteQueue);

            yield return new WaitForSeconds(JGameManager.Instance.MonsterSpawnDelay);
        }

        _spawnCount = 0;
    }

    private void RouteProcessing()
    {
        Vector3       _startPoint  = new Vector3();
        Vector3       _endPoint    = new Vector3();
        List<Vector3> _checkPoints = new List<Vector3>();

        // 타일맵을 순회하면서(좌하단부터 우측 방향으로 순회함)
        foreach (Vector3Int pointTilePos in PointTilemap.cellBounds.allPositionsWithin)
        {
            // 가져온 타일의
            Tile pointTile = PointTilemap.GetTile(pointTilePos) as Tile;

            // 스프라이트가 시작점 스프라이트와 같다면
            if(pointTile != null && pointTile.sprite == StartPointTileSprite)
            {
                _startPoint = PointTilemap.GetCellCenterWorld(pointTilePos);
            }
            // 스프라이트가 도착점 스프라이트와 같다면
            if (pointTile != null && pointTile.sprite == EndPointTileSprite)
            {
                _endPoint = PointTilemap.GetCellCenterWorld(pointTilePos);
            }
            // 스프라이트가 체크포인트 스프라이트와 같다면
            if (pointTile != null && pointTile.sprite == CheckPointTileSprite)
            {
                _checkPoints.Add(PointTilemap.GetCellCenterWorld(pointTilePos));
            }
        }

        // 몬스터가 이동해야 할 경로를 Queue에 담아줌
        // 이건 에디터 보고 직접 찍어야 될듯
        {
            RouteQueue.Enqueue(_startPoint);
            RouteQueue.Enqueue(_checkPoints[7]);
            RouteQueue.Enqueue(_checkPoints[8]);
            RouteQueue.Enqueue(_checkPoints[5]);
            RouteQueue.Enqueue(_checkPoints[6]);
            RouteQueue.Enqueue(_checkPoints[1]);
            RouteQueue.Enqueue(_checkPoints[0]);
            RouteQueue.Enqueue(_checkPoints[3]);
            RouteQueue.Enqueue(_checkPoints[2]);
            RouteQueue.Enqueue(_checkPoints[4]);
            RouteQueue.Enqueue(_endPoint);
        }
    }
    #endregion
}
