using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JSpawner : MonoBehaviour
{
    #region VARIABLES
    [Header("���� �� ���� ������")]
    public JMonster BearPrefab;

    [Header("���� Ƚ��")]
    private int _spawnCount = 0;

    [Header("���Ͱ� �̵� �� ���")]
    private Queue<Vector3> Route = new Queue<Vector3>();

    [Header("����Ʈ Ÿ�ϸ�")]
    public Tilemap PointTilemap;

    [Header("����Ʈ Ÿ�� ����")]
    public Sprite StartPointTileSprite;
    public Sprite EndPointTileSprite;
    public Sprite CheckPointTileSprite;
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
        Debug.Log("���� ������ �����ؿ�");

        StartCoroutine(SpawnMonsterWithDelay());
    }

    IEnumerator SpawnMonsterWithDelay()
    {
        while(_spawnCount <= 5)
        {
            Debug.Log("���Ͱ� �����ƾ��, " + _spawnCount.ToString());
            ++_spawnCount;

            JMonster go = Instantiate(BearPrefab, Route.Peek(), Quaternion.identity);
            go.SetRouteData(Route);

            yield return new WaitForSeconds(JGameManager.Instance.MonsterSpawnDelay);
        }

        _spawnCount = 0;
    }

    private void RouteProcessing()
    {
        Vector3       _startPoint  = new Vector3();
        Vector3       _endPoint    = new Vector3();
        List<Vector3> _checkPoints = new List<Vector3>();

        // Ÿ�ϸ��� ��ȸ�ϸ鼭(���ϴܺ��� ���� �������� ��ȸ��)
        foreach (Vector3Int pointTilePos in PointTilemap.cellBounds.allPositionsWithin)
        {
            // ������ Ÿ����
            Tile pointTile = PointTilemap.GetTile(pointTilePos) as Tile;

            // ��������Ʈ�� ������ ��������Ʈ�� ���ٸ�
            if(pointTile != null && pointTile.sprite == StartPointTileSprite)
            {
                _startPoint = PointTilemap.GetCellCenterWorld(pointTilePos);
            }
            // ��������Ʈ�� ������ ��������Ʈ�� ���ٸ�
            if (pointTile != null && pointTile.sprite == EndPointTileSprite)
            {
                _endPoint = PointTilemap.GetCellCenterWorld(pointTilePos);
            }
            // ��������Ʈ�� üũ����Ʈ ��������Ʈ�� ���ٸ�
            if (pointTile != null && pointTile.sprite == CheckPointTileSprite)
            {
                _checkPoints.Add(PointTilemap.GetCellCenterWorld(pointTilePos));
            }
        }

        // ���Ͱ� �̵��ؾ� �� ��θ� Queue�� �����
        {
            Route.Enqueue(_startPoint);
            Route.Enqueue(_checkPoints[7]);
            Route.Enqueue(_checkPoints[8]);
            Route.Enqueue(_checkPoints[5]);
            Route.Enqueue(_checkPoints[6]);
            Route.Enqueue(_checkPoints[1]);
            Route.Enqueue(_checkPoints[0]);
            Route.Enqueue(_checkPoints[3]);
            Route.Enqueue(_checkPoints[2]);
            Route.Enqueue(_checkPoints[4]);
            Route.Enqueue(_endPoint);
        }
    }
    #endregion
}
