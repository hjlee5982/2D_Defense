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
    [Header("���� ��ư �׼�")]
    public Action StartButtonAction;

    [Header("���� ������")]
    public MonsterSpawner Spawner;

    [Header("���� ���� ��")]
    public int NumOfMonster = 1;

    [Header("���� ���� ����")]
    public float MonsterSpawnDelay = 1.0f;

    [Header("���� �̵��ӵ�")]
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
        // UI_GameController StartButtonClick()���� Invoke ���ְ� ����
        Debug.Log("���尡 ���۵ſ�");

        Spawner.SpawnMonster();
    }
    #endregion
}
