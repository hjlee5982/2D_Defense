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
    private Queue<Vector3> RouteQueue     = new Queue<Vector3>();
    private Queue<Vector3> RouteDataQueue = new Queue<Vector3>();

    [Header("포인트 타일맵")]
    public Tilemap PointTilemap;

    [Header("포인트 타일 변수")]
    public Sprite StartPointTileSprite;
    public Sprite EndPointTileSprite;
    public Sprite CheckPointTileSprite;

    [Header("스폰 횟수")]
    private int _spawnCount = 0;

    [Header("스테이지 데이터")]
    private StageData _stageData;

    [Header("현제 스테이지에서 소환할 몬스터 정보")]
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

        StartCoroutine(SpawnMonsterWithDelay(e));
    }

    IEnumerator SpawnMonsterWithDelay(BeginSpawnMonsterEvent e)
    {
        while(_spawnCount < _stageData.NumOfMonster)
        {
            ++_spawnCount;

            MonsterUnit monsterUnit = Instantiate(_monsterUnitData.UnitPrefab, RouteDataQueue.Peek(), Quaternion.identity);
            monsterUnit.SetInitialData(_monsterUnitData, RouteDataQueue);

            yield return new WaitForSeconds(_stageData.MonsterSpawnInterval);
        }

        _spawnCount = 0;
    }

    //private void RouteProcessing()
    //{
    //    Vector3       _startPoint  = new Vector3();
    //    Vector3       _endPoint    = new Vector3();
    //    List<Vector3> _checkPoints = new List<Vector3>();

    //    // 타일맵을 순회하면서(좌하단부터 우측 방향으로 순회함)
    //    foreach (Vector3Int pointTilePos in PointTilemap.cellBounds.allPositionsWithin)
    //    {
    //        // 가져온 타일의
    //        Tile pointTile = PointTilemap.GetTile(pointTilePos) as Tile;

    //        // 스프라이트가 시작점 스프라이트와 같다면
    //        if(pointTile != null && pointTile.sprite == StartPointTileSprite)
    //        {
    //            _startPoint = PointTilemap.GetCellCenterWorld(pointTilePos);
    //        }
    //        // 스프라이트가 도착점 스프라이트와 같다면
    //        if (pointTile != null && pointTile.sprite == EndPointTileSprite)
    //        {
    //            _endPoint = PointTilemap.GetCellCenterWorld(pointTilePos);
    //        }
    //        // 스프라이트가 체크포인트 스프라이트와 같다면
    //        if (pointTile != null && pointTile.sprite == CheckPointTileSprite)
    //        {
    //            _checkPoints.Add(PointTilemap.GetCellCenterWorld(pointTilePos));
    //        }
    //    }

    //    // 몬스터가 이동해야 할 경로를 Queue에 담아줌
    //    // 이건 에디터 보고 직접 찍어야 될듯
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
        List<Vector3> _points = new List<Vector3>();

        // 타일맵을 순회하면서(좌하단부터 우측 방향으로 순회함)
        foreach (Vector3Int pointTilePos in PointTilemap.cellBounds.allPositionsWithin)
        {
            // 가져온 타일의
            Tile pointTile = PointTilemap.GetTile(pointTilePos) as Tile;

            // 스프라이트 null이 아니라면
            if(pointTile != null)
            {
                // 시작, 도착, 체크포인트 스프라이트와 같다면

                if (pointTile.sprite == StartPointTileSprite ||
                    pointTile.sprite == EndPointTileSprite   ||
                    pointTile.sprite == CheckPointTileSprite)
                {
                    _points.Add(PointTilemap.GetCellCenterWorld(pointTilePos));
                }
            }
        }

        for(int i = 0; i < routeData.Length; ++i)
        {
            int routeIndex = (int)routeData[i] - '0';

            RouteDataQueue.Enqueue(_points[routeIndex]);
        }
    }
    #endregion
}
