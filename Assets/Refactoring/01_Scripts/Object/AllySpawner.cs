using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AllySpawner : MonoBehaviour
{
    #region VARIABLES
    [Header("소환 가능 지역")]
    public Tilemap SpawnEnablePoints;

    [Header("소환 미리보기")]
    private GameObject _spawnPreview;

    [Header("소환 가능 유닛들")]
    public List<AllyUnit> AllyUnits = new List<AllyUnit>();

    [Header("유닛 스폰 플래그")]
    private bool _doingAllySpawn = false;

    [Header("소환 버튼 인덱스")]
    private int _btnIdx = -1;

    [Header("소환 위치")]
    private Vector3 _spawnPos;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        _spawnPreview = SpawnEnablePoints.transform.GetChild(0).gameObject;
    }

    void Start()
    {
    }

    void Update()
    {
        if (_doingAllySpawn == true)
        {
            if (MouseToTileSpace() == true)
            {
                if (Input.GetMouseButtonDown(0) == true)
                {
                    ExecuteSpawnAlly();
                }
            }
            if (Input.GetMouseButtonDown(1) == true)
            {
                CancelSpawnAlly();
            }
        }
    }
    #endregion





    #region FUNCTIONS
    public void BeginSpawnAlly(int btnIdx)
    {
        _doingAllySpawn = true;
        SpawnEnablePoints.gameObject.SetActive(true);

        _btnIdx = btnIdx;
    }

    public void ExecuteSpawnAlly()
    {
        Debug.Log("소환 진행");

        Instantiate(AllyUnits[_btnIdx], _spawnPos, Quaternion.identity);

        _doingAllySpawn = false;
        SpawnEnablePoints.gameObject.SetActive(false);
    }

    public void CancelSpawnAlly()
    {
        Debug.Log("소환 취소");
        _doingAllySpawn = false;
        SpawnEnablePoints.gameObject.SetActive(false);
    }

    private bool MouseToTileSpace()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int tilePos = SpawnEnablePoints.WorldToCell(mouseWorldPos);

        TileBase tile = SpawnEnablePoints.GetTile(tilePos);

        if (tile != null)
        {
            SpawnPreviewOn(tilePos);
            return true;
        }
        else
        {
            SpawnPreviewOff();
            return false;
        }
    }

    public void SpawnPreviewOn(Vector3Int tilePos)
    {
        _spawnPreview.SetActive(true);

        Vector3 previewWorldPos = SpawnEnablePoints.CellToWorld(tilePos);

        Vector3 tileCenterOffset = SpawnEnablePoints.cellSize / 2;

        Vector3 yOffset = new Vector3(tileCenterOffset.x, tileCenterOffset.y * 1.4f, tileCenterOffset.z);

        _spawnPreview.transform.position = _spawnPos = previewWorldPos + yOffset;
    }

    public void SpawnPreviewOff()
    {
        _spawnPreview.SetActive(false);
    }

    
    #endregion
}
