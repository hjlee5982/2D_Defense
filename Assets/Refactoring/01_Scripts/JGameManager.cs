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
    [Header("현재 체력")]
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

    [Header("현재 몬스터 수")]
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

    [Header("현재 골드")]
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
        // _currentStage의 데이터 (얼마나 소환할지, 얼마 간격으로 소환할지 등등)
        StageData   stageData    = DataLoader.StageData[CurrentStage];

        // _currentStage에 소환 할 몬스터의 정보
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
                // 이전에 선택되었던 유닛의 인디케이터는 꺼줘야함
                _selectedUnit?.ToggleIndicator(false);

                _selectedUnit = hit.collider.GetComponentInParent<AllyUnit>();

                JEventBus.SendEvent(new UnitSelectEvent(_selectedUnit));

                // if문 바로 뒤의 _selectedUnit은 이 시점에서 선택된 유닛 전에 선택된 유닛임
                _selectedUnit.ToggleIndicator(true);
            }
        }
        if(Input.GetMouseButtonDown(1) == true)
        {
            JEventBus.SendEvent(new UnitDeselectEvent());

            _selectedUnit?.ToggleIndicator(false);
        }
    }

    // 몬스터가 죽거나 완주하면 호출됨
    private void MonsterStateChange(MonsterStateChangeEvent e)
    {
        switch (e.Type)
        {
            case MonsterStateType.Die:

                // TODO
                // 골드 증가 (현재 스테이지에 설정된 드랍 골드만큼)
                // 현재 몬스터 수 1 감소
                Gold         += DataLoader.StageData[CurrentStage].DropGold;
                NumOfMonster -= 1;

                break;

            case MonsterStateType.Finish:

                // TODO
                // 생명력 1 감소
                // 현재 몬스터 수 1 감소
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
        Debug.Log("게임끝_패배");

        Time.timeScale = 0;
    }

    private void GameClearProcess()
    {
        Debug.Log("게임끝_승리");

        Time.timeScale = 0;
    }

    private void DataProcess()
    {
        // 유닛 데이터의 프리펩 네임을 바탕으로
        // Addressable로 로드된 프리펩들을 찾아서 유닛 데이터에 매칭시켜줌
        foreach(var kvp in DataLoader.AllyUnitData)
        {
            if(DataLoader.PrefabData.ContainsKey(kvp.Value.UnitPrefabName) == true)
            {
                kvp.Value.UnitPrefab = DataLoader.PrefabData[kvp.Value.UnitPrefabName].GetComponent<AllyUnit>();
            }
        }
        // 몬스터 데이터의 프리펩 네임을 바탕으로
        // Addressable로 로드된 프리펩들을 찾아서 몬스터 데이터에 매칭시켜줌
        foreach (var kvp in DataLoader.MonsterUnitData)
        {
            if(DataLoader.PrefabData.ContainsKey(kvp.Value.UnitPrefabName) == true)
            {
                kvp.Value.UnitPrefab = DataLoader.PrefabData[kvp.Value.UnitPrefabName].GetComponent<MonsterUnit>();
            }
        }

        // 경로 데이터를 뽑아와서 몬스터 생성 시 몬스터에게 경로 정보를 주입시킴
        MonsterSpawner.RouteDataProcessing(DataLoader.RouteData[0].Route);


        // 혼돈의 주문서 데이터 파싱
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
