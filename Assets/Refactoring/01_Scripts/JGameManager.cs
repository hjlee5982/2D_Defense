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

    [Header("몬스터 생성 수")]
    public int NumOfMonster = 1;

    [Header("몬스터 생성 간격")]
    public float MonsterSpawnDelay = 1.0f;

    [Header("몬스터 이동속도")]
    public float MonsterSpeed = 1.0f;

    [Space(20)]

    [Header("유닛 스포너")]
    public AllySpawner AllySpawner;

    [Header("투사체 속도")]
    public float ProjectileSpeed = 10f;

    [Header("공격 간격")]
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
        // 레벨 데이터를 여기서 넣어주면 될듯?
        // List<LevelData> levelData
        // JEventBus.SendEvent(new BeginSpawnMonsterEvent(levelData[0]));

        // levelData는 소환되는 몬스터의 수, 소환 간격, 이동속도 등등을 가지고 있으면 될듯

        JEventBus.SendEvent(new BeginSpawnMonsterEvent());
    }

    private void BeginSpawnAlly(StartSpawnAllyEvent e)
    {
        // 누른 버튼에 따라 생성되는 유닛을 다르게 하면 될듯
        // List<UnitData> unitData
        // JEventBus.SendEvent(new BeginSpawnAllyEvent(unitData[e.BtnIdx]));

        // unitData는 공격범위, 공격속도, 공격력 등등을 가지고 있으면 될듯
        
        JEventBus.SendEvent(new BeginSpawnAllyEvent(e.BtnIdx));
    }
    #endregion
}
