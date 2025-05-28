using System.Collections.Generic;
using UnityEngine;

using static GameStatusChangeEvent;
using static MonsterStateChangeEvent;

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
            // �� �����Ϸ� ��? ���ƹ����ų�
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
    [Header("���� ������")]
    public MonsterSpawner MonsterSpawner;

    [Header("���� ������")]
    public AllySpawner AllySpawner;

    [Header("������ �δ�")]
    public JDataLoader DataLoader;

    [Header("���õ� ����")]
    private AllyUnit _selectedUnit;

    [Header("���� ���")]
    public RandomAssistant RandomAssistant;

    [Header("���� ��������")]
    private int _currentStage = -1;
    public int CurrentStage
    {
        get => _currentStage;
        set
        {
            if(_currentStage != value)
            {
                _currentStage = value;
                JEventBus.SendEvent(new GameStatusChangeEvent(GameStatusType.Round, _currentStage, DataLoader.StageData.Count));
            }
        }
    }
    [Header("���� ü��")]
    private int _life = -1;
    public int Life
    {
        get => _life;
        set
        { 
            if(_life != value)
            {
                _life = value;
                JEventBus.SendEvent(new GameStatusChangeEvent(GameStatusType.Life, _life));
            }
        }
    }

    [Header("���� ���� ��")]
    private int _numOfMonster = -1;
    public int NumOfMonster
    {
        get => _numOfMonster;
        set
        {
            if (_numOfMonster != value)
            {
                _numOfMonster = value;
                JEventBus.SendEvent(new GameStatusChangeEvent(GameStatusType.NumOfMonster, _numOfMonster));
            }
        }
    }

    [Header("���� ���")]
    private int _gold = -1;
    public int Gold
    {
        get => _gold;
        set
        {
            if (_gold != value)
            {
                _gold = value;
                JEventBus.SendEvent(new GoldRestrictionEvent(_gold));
                JEventBus.SendEvent(new GameStatusChangeEvent(GameStatusType.Gold, _gold));
            }
        }
    }

    // �Ʒ��� ���߿� �����ͷ� ���͵�

    // ��ȭ ������
    [Header("���� �ɼ� Ȯ�� ����ġ")]
    private List<(int value, int weight)> weightsTable = new List<(int value, int weight)>();
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();

        DataProcess();

        RandomAssistant = new RandomAssistant();
    }

    void Start()
    {
        Life         = DataLoader.GameRuleData[0].LifeLimit;
        NumOfMonster = 0;
        Gold         = DataLoader.GameRuleData[0].InitialGold;
        CurrentStage = 0;

        
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
        JEventBus.Subscribe<MonsterStateChangeEvent>(MonsterStateChange);
        JEventBus.Subscribe<SummonCompleteEvent>(SummonComplete);
        
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<StartRoundEvent>(StartRound);
        JEventBus.Unsubscribe<StartSpawnAllyEvent>(BeginSpawnAlly);
        JEventBus.Unsubscribe<StartEnhancementEvent>(EnhancementProcess);
        JEventBus.Unsubscribe<MonsterStateChangeEvent>(MonsterStateChange);
        JEventBus.Unsubscribe<SummonCompleteEvent>(SummonComplete);
    }
    #endregion





    #region FUNCTION
    private void StartRound(StartRoundEvent e)
    {
        // _currentStage�� ������ (�󸶳� ��ȯ����, �� �������� ��ȯ���� ���)
        StageData   stageData    = DataLoader.StageData[CurrentStage];

        // _currentStage�� ��ȯ �� ������ ����
        MonsterUnitData spawnMonsterData = DataLoader.MonsterUnitData[stageData.SpawnMonster];

        // JGameManager -> MonsterSpawner
        JEventBus.SendEvent(new BeginSpawnMonsterEvent(stageData, spawnMonsterData));

        NumOfMonster = DataLoader.StageData[CurrentStage].NumOfMonster;
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
                // ������ ���õǾ��� ������ �ε������ʹ� �������
                _selectedUnit?.ToggleIndicator(false);

                _selectedUnit = hit.collider.GetComponentInParent<AllyUnit>();

                JEventBus.SendEvent(new UnitSelectEvent(_selectedUnit));

                // if�� �ٷ� ���� _selectedUnit�� �� �������� ���õ� ���� ���� ���õ� ������
                _selectedUnit.ToggleIndicator(true);
            }
        }
        if(Input.GetMouseButtonDown(1) == true)
        {
            JEventBus.SendEvent(new UnitDeselectEvent());

            _selectedUnit?.ToggleIndicator(false);
        }
    }

    // ���Ͱ� �װų� �����ϸ� ȣ���
    private void MonsterStateChange(MonsterStateChangeEvent e)
    {
        switch (e.Type)
        {
            case MonsterStateType.Die:

                // TODO
                // ��� ���� (���� ���������� ������ ��� ��常ŭ)
                // ���� ���� �� 1 ����
                Gold         += DataLoader.StageData[CurrentStage].DropGold;
                NumOfMonster -= 1;

                break;

            case MonsterStateType.Finish:

                // TODO
                // ����� 1 ����
                // ���� ���� �� 1 ����
                Life         -= 1;
                NumOfMonster -= 1;

                break;
        }

        if (Life <= 0)
        {
            GameOverProcess();
        }
        if (NumOfMonster == 0 && DataLoader.StageData.Count -1 <= CurrentStage)
        {
            GameClearProcess();
        }
        else if(NumOfMonster == 0)
        {
            JEventBus.SendEvent(new EndRoundEvent());
            ++CurrentStage;
        }
    }

    private void SummonComplete(SummonCompleteEvent e)
    {
        Gold -= e.Gold;
    }

    private void EnhancementProcess(StartEnhancementEvent e)
    {
        if(_selectedUnit.IsEnhancable() == false)
        {
            return;
        }

        switch (e.BtnIdx)
        {
            case 0:
                {
                    if(RandomAssistant.TryChance(DataLoader.EnhancementData[0].Probability))
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, DataLoader.EnhancementData[0].dAtkPower);

                        _selectedUnit.ApplyStatChange(StatType.Grade, 1);
                    }

                    Gold -= DataLoader.EnhancementData[0].Cost;

                    break;
                }
            case 1:
                {
                    if(RandomAssistant.TryChance(DataLoader.EnhancementData[1].Probability))
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, DataLoader.EnhancementData[1].dAtkPower);
                        _selectedUnit.ApplyStatChange(StatType.AtkRange, DataLoader.EnhancementData[1].dAtkRange);

                        _selectedUnit.ApplyStatChange(StatType.Grade, 1);
                    }

                    Gold -= DataLoader.EnhancementData[1].Cost;

                    break;
                }
            case 2:
                {
                    if (RandomAssistant.TryChance(DataLoader.EnhancementData[2].Probability))
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, DataLoader.EnhancementData[2].dAtkPower);
                        _selectedUnit.ApplyStatChange(StatType.AtkRange, DataLoader.EnhancementData[2].dAtkRange);
                        _selectedUnit.ApplyStatChange(StatType.AtkSpeed, DataLoader.EnhancementData[2].dAtkSpeed);

                        _selectedUnit.ApplyStatChange(StatType.Grade, 1);
                    }

                    Gold -= DataLoader.EnhancementData[2].Cost;

                    break;
                }
            case 3:
                {
                    if (RandomAssistant.TryChance(DataLoader.EnhancementData[3].Probability))
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, RandomAssistant.GetRandomSign() * RandomAssistant.WeightedRandomSelector(weightsTable));
                        _selectedUnit.ApplyStatChange(StatType.AtkRange, RandomAssistant.GetRandomSign() * RandomAssistant.WeightedRandomSelector(weightsTable));
                        _selectedUnit.ApplyStatChange(StatType.AtkSpeed, RandomAssistant.GetRandomSign() * RandomAssistant.WeightedRandomSelector(weightsTable));
                       
                        _selectedUnit.ApplyStatChange(StatType.Grade, 1);
                    }

                    Gold -= DataLoader.EnhancementData[3].Cost;

                    break;
                }
        }

        _selectedUnit.ApplyStatChange(StatType.UpgradeCount, -1);

        JEventBus.SendEvent(new EnhanceCompleteEvent(_selectedUnit));
    }

    private void GameOverProcess()
    {
        Debug.Log("���ӳ�_�й�");

        Time.timeScale = 0;
    }

    private void GameClearProcess()
    {
        Debug.Log("���ӳ�_�¸�");

        Time.timeScale = 0;
    }

    private void DataProcess()
    {
        // ���� �������� ������ ������ ��������
        // Addressable�� �ε�� ��������� ã�Ƽ� ���� �����Ϳ� ��Ī������
        foreach(var kvp in DataLoader.AllyUnitData)
        {
            if(DataLoader.PrefabData.ContainsKey(kvp.Value.UnitPrefabName) == true)
            {
                kvp.Value.UnitPrefab = DataLoader.PrefabData[kvp.Value.UnitPrefabName].GetComponent<AllyUnit>();
            }
        }
        // ���� �������� ������ ������ ��������
        // Addressable�� �ε�� ��������� ã�Ƽ� ���� �����Ϳ� ��Ī������
        foreach (var kvp in DataLoader.MonsterUnitData)
        {
            if(DataLoader.PrefabData.ContainsKey(kvp.Value.UnitPrefabName) == true)
            {
                kvp.Value.UnitPrefab = DataLoader.PrefabData[kvp.Value.UnitPrefabName].GetComponent<MonsterUnit>();
            }
        }

        // ��� �����͸� �̾ƿͼ� ���� ���� �� ���Ϳ��� ��� ������ ���Խ�Ŵ
        MonsterSpawner.RouteDataProcessing(DataLoader.RouteData[0].Route);


        // ȥ���� �ֹ��� ������ �Ľ�
        foreach (var enhancementData in DataLoader.EnhancementData)
        {
            if (enhancementData.Value.isRandom == true)
            {
                string[] pairs = enhancementData.Value.RandomWeight.Split(':');

                foreach (string pair in pairs)
                {
                    string[] parsedPair = pair.Split(',');

                    weightsTable.Add((int.Parse(parsedPair[0]), int.Parse(parsedPair[1])));
                }
            }
        }
    }
    #endregion
}
