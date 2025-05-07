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
