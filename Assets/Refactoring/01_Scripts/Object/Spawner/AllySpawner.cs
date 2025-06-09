using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AllySpawner : MonoBehaviour
{
    #region VARIABLES
    [Header("��ȯ ���� ����")]
    public Tilemap SpawnEnablePoints;

    [Header("��ȯ �̸�����")]
    private List<GameObject> _spawnPreviews = new List<GameObject>();

    [Header("��ȯ �� ���� ������")]
    private AllyUnitData _allyUnitData;

    [Header("���� ���� �÷���")]
    private bool _doingAllySpawn = false;

    [Header("��ȯ ��ġ")]
    private Vector3    _spawnPos;
    private Vector3Int _tilePos;

    [Header("��ȯ �÷��� ��������Ʈ")]
    public Sprite AvailablePoint;
    public Sprite InavailablePoint;

    [Header("���� ��ġ ��Ȳ")]
    private Dictionary<Vector3Int, AllyUnit> _arrangementState = new Dictionary<Vector3Int, AllyUnit>();
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        for(int i = 0; i < 3; ++i)
        {
            GameObject preview = SpawnEnablePoints.transform.GetChild(i).gameObject;

            _spawnPreviews.Add(preview);

            preview.SetActive(false);
        }
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

    private void OnEnable()
    {
        JEventBus.Subscribe<BeginSpawnAllyEvent>(BeginSpawnAlly);
        JEventBus.Subscribe<UnitRecallPhase2Event>(UnitRecall);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<BeginSpawnAllyEvent>(BeginSpawnAlly);
        JEventBus.Unsubscribe<UnitRecallPhase2Event>(UnitRecall);
    }
    #endregion





    #region FUNCTIONS
    public void BeginSpawnAlly(BeginSpawnAllyEvent e)
    {
        _allyUnitData = e.AllyUnitData;

        _doingAllySpawn = true;
        SpawnEnablePoints.gameObject.SetActive(true);
    }

    public void ExecuteSpawnAlly()
    {
        AllyUnit allyUnit = Instantiate(_allyUnitData.UnitPrefab, _spawnPos, Quaternion.identity).GetComponent<AllyUnit>();
        allyUnit.SetInitialData(_allyUnitData);

        // TODO
        // ���⼭ ���� ��ߵǳ�
        JEventBus.SendEvent(new SummonCompleteEvent(_allyUnitData.Cost));
        JAudioManager.Instance.PlaySFX("AllySummon");
        
        CreateTile(InavailablePoint, _tilePos);

        if(_arrangementState.ContainsKey(_tilePos) == true)
        {
            _arrangementState[_tilePos] = allyUnit;
        }
        else
        {
            _arrangementState.Add(_tilePos, allyUnit);
        }


        foreach (GameObject preview in _spawnPreviews)
        {
            preview.SetActive(false);
        }
        _doingAllySpawn = false;
        SpawnEnablePoints.gameObject.SetActive(false);
    }

    public void CancelSpawnAlly()
    {
        foreach (GameObject preview in _spawnPreviews)
        {
            preview.SetActive(false);
        }
        _doingAllySpawn = false;
        SpawnEnablePoints.gameObject.SetActive(false);
    }

    private bool MouseToTileSpace()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _tilePos = SpawnEnablePoints.WorldToCell(mouseWorldPos);

        TileBase tileBase = SpawnEnablePoints.GetTile(_tilePos);
        
        Tile tile = tileBase as Tile;

        if (tileBase != null && tile.sprite == AvailablePoint)
        {
            SpawnPreviewOn(_tilePos);
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
        _spawnPreviews[_allyUnitData.Index].SetActive(true);

        Vector3 previewWorldPos = SpawnEnablePoints.CellToWorld(tilePos);

        Vector3 tileCenterOffset = SpawnEnablePoints.cellSize / 2;

        Vector3 yOffset = new Vector3(tileCenterOffset.x, tileCenterOffset.y * 1.4f, tileCenterOffset.z);

        _spawnPreviews[_allyUnitData.Index].transform.position = _spawnPos = previewWorldPos + yOffset;
    }

    public void SpawnPreviewOff()
    {
        _spawnPreviews[_allyUnitData.Index].SetActive(false);
    }

    private void UnitRecall(UnitRecallPhase2Event e)
    {
        // ������ �������� �� ȣ���
        // �� �� �������� null�� �κ��� ����������
        // �׷��� ���� ������Ʈ�� ����� ���� �����ӿ��� Ÿ���� �����Ϸ� ��
        // Destroy�� �������� ���������� �۵���
        // �׷��� ���� �����ӿ� Ÿ���� �����ؾ� ��

        StartCoroutine(CreateTileNextFrame());
    }

    private IEnumerator CreateTileNextFrame()
    {
        yield return null;

        foreach (var kvp in _arrangementState)
        {
            if (kvp.Value == null)
            {
                CreateTile(AvailablePoint, kvp.Key);
            }
        }
    }

    private void CreateTile(Sprite sprite, Vector3Int tilePos)
    {
        TileBase originalTileBase = SpawnEnablePoints.GetTile(_tilePos);
        Tile originalTile = originalTileBase as Tile;

        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        {
            newTile.sprite = sprite;
            newTile.color = originalTile.color;
            newTile.colliderType = originalTile.colliderType;
        }

        SpawnEnablePoints.SetTile(tilePos, newTile);
    }
    #endregion
}
