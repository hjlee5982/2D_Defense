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

    [Space(10)]

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
