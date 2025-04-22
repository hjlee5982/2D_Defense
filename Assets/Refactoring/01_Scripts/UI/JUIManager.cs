using UnityEngine;

public class JUIManager : MonoBehaviour
{
    #region SINGLETON
    private static JUIManager instance;
    public static  JUIManager Instance
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
    #endregion





    #region CHILDREN UI
    public UI_GameStatus     GameStatus { get; private set; }
    public UI_GameController Controller { get; private set; }
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();

        GameStatus  = transform.Find("GameStatus")    .GetComponent<UI_GameStatus>();
        Controller  = transform.Find("GameController").GetComponent<UI_GameController>();
    }

    void Start()
    {

    }

    void Update()
    {

    }
    #endregion
}
