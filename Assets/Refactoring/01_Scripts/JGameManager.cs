using System;
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
    [Header("시작 버튼 액션")]
    public Action StartButtonAction;

    [Header("몬스터 스포너")]
    public MonsterSpawner Spawner;

    [Header("몬스터 생성 수")]
    public int NumOfMonster = 1;

    [Header("몬스터 생성 간격")]
    public float MonsterSpawnDelay = 1.0f;

    [Header("몬스터 이동속도")]
    public float MonsterSpeed = 1.0f;
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

    void OnEnable()
    {
        StartButtonAction += StartRound;
    }

    void OnDisable()
    {
        StartButtonAction -= StartRound;
    }
    #endregion





    #region FUNCTION
    public void StartRound()
    {
        // UI_GameController StartButtonClick()에서 Invoke 해주고 있음
        Debug.Log("라운드가 시작돼요");

        Spawner.SpawnMonster();
    }
    #endregion
}
