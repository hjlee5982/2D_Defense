using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AllySpawner : MonoBehaviour
{
    #region VARIABLES
    [Header("��ȯ ���� ����")]
    public Tilemap SpawnEnablePoints;

    [Header("��ȯ �̸�����")]
    private GameObject _spawnPreview;

    [Header("��ȯ ���� ���ֵ�")]
    public List<AllyUnit> AllyUnits = new List<AllyUnit>();

    [Header("���� ���� �÷���")]
    private bool _doingAllySpawn = false;

    [Header("��ȯ ��ư �ε���")]
    private int _btnIdx = -1;

    [Header("��ȯ ��ġ")]
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
        Debug.Log("��ȯ ����");

        Instantiate(AllyUnits[_btnIdx], _spawnPos, Quaternion.identity);

        _doingAllySpawn = false;
        SpawnEnablePoints.gameObject.SetActive(false);
    }

    public void CancelSpawnAlly()
    {
        Debug.Log("��ȯ ���");
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
