using System;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Header("���� ���� ��")]
    public int NumOfMonster = 1;

    [Header("���� ���� ����")]
    public float MonsterSpawnDelay = 1.0f;

    [Header("���� �̵��ӵ�")]
    public float MonsterSpeed = 1.0f;

    [Space(20)]

    [Header("���� ������")]
    public AllySpawner AllySpawner;

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
        
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<StartRoundEvent>(StartRound);
        JEventBus.Subscribe<StartSpawnAllyEvent>(BeginSpawnAlly);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<StartRoundEvent>(StartRound);
        JEventBus.Unsubscribe<StartSpawnAllyEvent>(BeginSpawnAlly);
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
        // ���� ��ư�� ���� �����Ǵ� ������ �ٸ��� �ϸ� �ɵ�
        // List<UnitData> unitData
        // JEventBus.SendEvent(new BeginSpawnAllyEvent(unitData[e.BtnIdx]));

        // unitData�� ���ݹ���, ���ݼӵ�, ���ݷ� ����� ������ ������ �ɵ�
        
        JEventBus.SendEvent(new BeginSpawnAllyEvent(e.BtnIdx));
    }
    #endregion
}
