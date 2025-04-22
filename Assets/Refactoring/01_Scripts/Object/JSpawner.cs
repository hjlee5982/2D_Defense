using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JSpawner : MonoBehaviour
{
    #region VARIABLES
    [Header("스폰 할 몬스터 프리팹")]
    public JMonster BearPrefab;

    [Header("스폰 횟수")]
    private int _spawnCount = 0;

    [Header("몬스터가 이동 할 경로")]
    private Queue<Vector3> Route = new Queue<Vector3>();

    [Header("포인트 타일맵")]
    public Tilemap PointTilemap;

    [Header("포인트 타일 변수")]
    public Sprite StartPointTileSprite;
    public Sprite EndPointTileSprite;
    public Sprite CheckPointTileSprite;

    [Header("경로 변수")]
    private Vector3       _startPoint;
    private Vector3       _endPoint;
    private List<Vector3> _checkPoints = new List<Vector3>();
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
    #endregion





    #region FUNCTIONS
    public void SpawnMonster()
    {
        Debug.Log("몬스터 생성을 시작해요");

        StartCoroutine(SpawnMonsterWithDelay());
    }

    IEnumerator SpawnMonsterWithDelay()
    {
        while(_spawnCount <= 5)
        {
            Debug.Log("몬스터가 생성됐어요, " + _spawnCount.ToString());
            ++_spawnCount;

            yield return new WaitForSeconds(1f);
        }

        _spawnCount = 0;
    }

    private void RouteProcessing()
    {
        // 타일맵을 순회하면서(좌하단부터 우측 방향으로 순회함)
        foreach(Vector3Int pointTilePos in PointTilemap.cellBounds.allPositionsWithin)
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
        {
            Route.Enqueue(_startPoint);
            Route.Enqueue(_checkPoints[2]);
            Route.Enqueue(_checkPoints[1]);
            Route.Enqueue(_checkPoints[0]);
            Route.Enqueue(_endPoint);
        }
    }
    #endregion
}
