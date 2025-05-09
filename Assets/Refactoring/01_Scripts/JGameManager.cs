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

    [Header("선택된 유닛")]
    private JUnit _selectedUnit;

    [Header("랜덤 모듈")]
    public RandomAssistant RandomAssistant;

    // 아래는 나중에 데이터로 뺄것들z

    // 레벨데이터
    [Header("몬스터 생성 수")]
    public int NumOfMonster = 1;

    [Header("몬스터 생성 간격")]
    public float MonsterSpawnDelay = 1.0f;

    [Header("몬스터 이동속도")]
    public float MonsterSpeed = 1.0f;


    // 유닛 데이터
    [Header("투사체 속도")]
    public float ProjectileSpeed = 10f;

    [Header("공격 간격")]
    public float AttackInterval = 1f;


    // 강화 데이터
    [Header("랜덤 옵션 확률 가중치")]
    private List<(int value, int weight)> weightsTable = new List<(int value, int weight)>();
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();

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
        // 레벨 데이터를 여기서 넣어주면 될듯?
        // List<LevelData> levelData
        // JEventBus.SendEvent(new BeginSpawnMonsterEvent(levelData[0]));

        // levelData는 소환되는 몬스터의 수, 소환 간격, 이동속도 등등을 가지고 있으면 될듯

        JEventBus.SendEvent(new BeginSpawnMonsterEvent());
    }

    private void BeginSpawnAlly(StartSpawnAllyEvent e)
    {
        // JGameManager -> AllySpawner
        JEventBus.SendEvent(new BeginSpawnAllyEvent(e.BtnIdx));
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
                _selectedUnit = hit.collider.GetComponentInParent<JUnit>();

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
    #endregion
}
