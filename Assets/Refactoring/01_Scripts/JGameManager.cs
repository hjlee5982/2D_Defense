using System.Collections.Generic;
using UnityEngine;

public class JGameManager : MonoBehaviour
{
    #region SINGLETON
    private static JGameManager instance;
    public static  JGameManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            // 왜 접근하려 함? 돌아버린거냐
        }
    }

    void SingletonInitialize()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    #endregion





    #region VARIABLES
    [Header("몬스터 스포너")]
    public MonsterSpawner MonsterSpawner;

    [Header("유닛 스포너")]
    public AllySpawner AllySpawner;

    [Header("데이터 로더")]
    public JDataLoader DataLoader;

    [Header("선택된 유닛")]
    private AllyUnit _selectedUnit;

    [Header("랜덤 모듈")]
    public RandomAssistant RandomAssistant;

    [Header("현재 스테이지")]
    private int _currentStage = 0;

    [Header("현재 체력")]
    private int _life = 0;
    public int Life
    {
        get => _life;
        set
        { 
            if(_life != value)
            {
                _life = value;
                JEventBus.SendEvent(new GameStatusChangeEvent(_life, _numOfMonster, _gold));
            }
        }
    }

    [Header("현재 몬스터 수")]
    private int _numOfMonster = 0;
    public int NumOfMonster
    {
        get => _numOfMonster;
        set
        {
            if (_numOfMonster != value)
            {
                _numOfMonster = value;
                JEventBus.SendEvent(new GameStatusChangeEvent(_life, _numOfMonster, _gold));
            }
        }
    }

    [Header("현재 골드")]
    private int _gold = 0;
    public int Gold
    {
        get => _numOfMonster;
        set
        {
            if (_life != value)
            {
                _numOfMonster = value;
                JEventBus.SendEvent(new GameStatusChangeEvent(_life, _numOfMonster, _gold));
            }
        }
    }

    // 아래는 나중에 데이터로 뺄것들

    // 강화 데이터
    [Header("랜덤 옵션 확률 가중치")]
    private List<(int value, int weight)> weightsTable = new List<(int value, int weight)>();
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();

        DataProcess();

        RandomAssistant = new RandomAssistant();



        // 강화 데이터(임시)
        // json으로 읽어올꺼임
        {
            weightsTable.Add((1, 50));
            weightsTable.Add((2, 25));
            weightsTable.Add((3, 15));
            weightsTable.Add((4,  7));
            weightsTable.Add((5,  3));
        }
    }

    void Start()
    {
        Life = DataLoader.GameRule[0].LifeLimit;
    }

    void Update()
    {
        UnitSelect();
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<StartRoundEvent>(StartRound);
        JEventBus.Subscribe<StartSpawnAllyEvent>(BeginSpawnAlly);
        JEventBus.Subscribe<StartEnhancementEvent>(EnhancementProcess);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<StartRoundEvent>(StartRound);
        JEventBus.Unsubscribe<StartSpawnAllyEvent>(BeginSpawnAlly);
        JEventBus.Unsubscribe<StartEnhancementEvent>(EnhancementProcess);
    }
    #endregion





    #region FUNCTION
    private void StartRound(StartRoundEvent e)
    {
        if(DataLoader.StageData.Count <= _currentStage)
        {
            Debug.Log("스테이지 데이터를 다 순회했어요");
            return;
        }

        // _currentStage의 데이터 (얼마나 소환할지, 얼마 간격으로 소환할지 등등)
        StageData   stageData    = DataLoader.StageData[_currentStage];

        // _currentStage에 소환 할 몬스터의 정보
        MonsterUnitData spawnMonsterData = DataLoader.MonsterUnitData[stageData.SpawnMonster];

        // JGameManager -> MonsterSpawner
        JEventBus.SendEvent(new BeginSpawnMonsterEvent(stageData, spawnMonsterData));

        NumOfMonster = DataLoader.StageData[_currentStage].NumOfMonster;

        ++_currentStage;
    }

    private void BeginSpawnAlly(StartSpawnAllyEvent e)
    {
        // JGameManager -> AllySpawner
        JEventBus.SendEvent(new BeginSpawnAllyEvent(DataLoader.AllyUnitData[e.BtnIdx]));
    }

    private void UnitSelect()
    {
        if(Input.GetMouseButtonDown(0) == true)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos2D    = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            int clickLayerMask = LayerMask.GetMask("ClickDetection");

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, clickLayerMask);

            if (hit.collider != null)
            {
                _selectedUnit = hit.collider.GetComponentInParent<AllyUnit>();

                JEventBus.SendEvent(new UnitSelectEvent(_selectedUnit));
            }
        }
        if(Input.GetMouseButtonDown(1) == true)
        {
            JEventBus.SendEvent(new UnitDeselectEvent());
        }
    }

    private void EnhancementProcess(StartEnhancementEvent e)
    {
        if(_selectedUnit.IsEnhancable() == false)
        {
            return;
        }

        switch (e.BtnIdx)
        {
            // 100%
            case 0:
                {
                    if(RandomAssistant.TryChance(100))
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, 1);

                        _selectedUnit.ApplyStatChange(StatType.Grade, 1);
                    }
                    break;
                }
            // 60%
            case 1:
                {
                    if(RandomAssistant.TryChance(60))
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, 2);
                        _selectedUnit.ApplyStatChange(StatType.AtkRange, 1);

                        _selectedUnit.ApplyStatChange(StatType.Grade, 1);
                    }
                    break;
                }
            // 10%
            case 2:
                {
                    if (RandomAssistant.TryChance(10))
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, 5);
                        _selectedUnit.ApplyStatChange(StatType.AtkRange, 3);
                        _selectedUnit.ApplyStatChange(StatType.AtkSpeed, 1);

                        _selectedUnit.ApplyStatChange(StatType.Grade, 1);
                    }
                    break;
                }
            // Chaos 60%
            case 3:
                {
                    if (RandomAssistant.TryChance(35))
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, RandomAssistant.GetRandomSign() * RandomAssistant.WeightedRandomSelector(weightsTable));
                        _selectedUnit.ApplyStatChange(StatType.AtkRange, RandomAssistant.GetRandomSign() * RandomAssistant.WeightedRandomSelector(weightsTable));
                        _selectedUnit.ApplyStatChange(StatType.AtkSpeed, RandomAssistant.GetRandomSign() * RandomAssistant.WeightedRandomSelector(weightsTable));
                       
                        _selectedUnit.ApplyStatChange(StatType.Grade, 1);
                    }
                    break;
                }
        }

        _selectedUnit.ApplyStatChange(StatType.UpgradeCount, -1);

        JEventBus.SendEvent(new EnhanceCompleteEvent(_selectedUnit));
    }

    private void DataProcess()
    {
        foreach(var kvp in DataLoader.AllyUnitData)
        {
            if(DataLoader.PrefabData.ContainsKey(kvp.Value.UnitPrefabName) == true)
            {
                kvp.Value.UnitPrefab = DataLoader.PrefabData[kvp.Value.UnitPrefabName].GetComponent<AllyUnit>();
            }
        }
        foreach(var kvp in DataLoader.MonsterUnitData)
        {
            if(DataLoader.PrefabData.ContainsKey(kvp.Value.UnitPrefabName) == true)
            {
                kvp.Value.UnitPrefab = DataLoader.PrefabData[kvp.Value.UnitPrefabName].GetComponent<MonsterUnit>();
            }
        }
    }
    #endregion
}
