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

    [Header("���õ� ����")]
    private JUnit _selectedUnit;

    // �Ʒ��� ���߿� �����ͷ� ���͵�

    // ����������
    [Header("���� ���� ��")]
    public int NumOfMonster = 1;

    [Header("���� ���� ����")]
    public float MonsterSpawnDelay = 1.0f;

    [Header("���� �̵��ӵ�")]
    public float MonsterSpeed = 1.0f;


    // ���� ������
    [Header("����ü �ӵ�")]
    public float ProjectileSpeed = 10f;

    [Header("���� ����")]
    public float AttackInterval = 1f;
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();
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
        // ���� �����͸� ���⼭ �־��ָ� �ɵ�?
        // List<LevelData> levelData
        // JEventBus.SendEvent(new BeginSpawnMonsterEvent(levelData[0]));

        // levelData�� ��ȯ�Ǵ� ������ ��, ��ȯ ����, �̵��ӵ� ����� ������ ������ �ɵ�

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
                    if(true)
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, 1);
                    }
                    break;
                }
            // 60%
            case 1:
                {
                    if(Random.Range(0,100) < 60)
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, 2);
                        _selectedUnit.ApplyStatChange(StatType.AtkRange, 1);
                    }
                    break;
                }
            // 10%
            case 2:
                {
                    if (Random.Range(0, 100) < 10)
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, 5);
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, 3);
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, 1);
                    }
                    break;
                }
            // Chaos 60%
            case 3:
                {
                    if (Random.Range(0, 100) < 60)
                    {
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, GetRandomValue());
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, GetRandomValue());
                        _selectedUnit.ApplyStatChange(StatType.AtkPower, GetRandomValue());
                    }
                    break;
                }
        }

        _selectedUnit.ApplyStatChange(StatType.UpgradeCount, -1);

        JEventBus.SendEvent(new EnhanceCompleteEvent(_selectedUnit));
    }

    private int GetRandomValue()
    {
        int rnd = Random.Range(0, 100);

        if(rnd < 40)
        {
            return 0;
        }
        else if(rnd < 70)
        {
            return 1;
        }
        else if(rnd < 85)
        {
            return 2;
        }
        else if(rnd < 95)
        {
            return 3;
        }
        else if(rnd <99)
        {
            return 4;
        }
        else
        {
            return 5; 
        }
    }
    #endregion
}
