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

    [Space(10)]

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
    #endregion





    #region FUNCTION
    public void StartRound()
    {
        MonsterSpawner.SpawnMonster();
    }

    public void BeginSpawnAlly(int btnIdx)
    {
        AllySpawner.BeginSpawnAlly(btnIdx);
    }
    #endregion
}
