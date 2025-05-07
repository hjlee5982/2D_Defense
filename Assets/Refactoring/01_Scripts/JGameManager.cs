using System;
using System.Collections.Generic;
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

    [Header("유닛 스포너")]
    public AllySpawner AllySpawner;

    

    // 아래는 나중에 데이터로 뺄것들

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
                JUnit cat = hit.collider.GetComponentInParent<JUnit>();

                if(cat != null)
                {
                    cat.GetUnitData();
                }

                JEventBus.SendEvent(new UnitSelectEvent(cat.GetUnitData()));
            }
        }
        if(Input.GetMouseButtonDown(1) == true)
        {
            JEventBus.SendEvent(new UnitDeselectEvent());
        }
    }
    #endregion
}
