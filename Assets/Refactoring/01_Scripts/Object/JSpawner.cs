using System.Collections;
using System.Collections.Generic;
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

    [Header("��� ����")]
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
        Debug.Log("���� ������ �����ؿ�");

        StartCoroutine(SpawnMonsterWithDelay());
    }

    IEnumerator SpawnMonsterWithDelay()
    {
        while(_spawnCount <= 5)
        {
            Debug.Log("���Ͱ� �����ƾ��, " + _spawnCount.ToString());
            ++_spawnCount;

            yield return new WaitForSeconds(1f);
        }

        _spawnCount = 0;
    }

    private void RouteProcessing()
    {
        // Ÿ�ϸ��� ��ȸ�ϸ鼭(���ϴܺ��� ���� �������� ��ȸ��)
        foreach(Vector3Int pointTilePos in PointTilemap.cellBounds.allPositionsWithin)
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
            Route.Enqueue(_checkPoints[2]);
            Route.Enqueue(_checkPoints[1]);
            Route.Enqueue(_checkPoints[0]);
            Route.Enqueue(_endPoint);
        }
    }
    #endregion
}
