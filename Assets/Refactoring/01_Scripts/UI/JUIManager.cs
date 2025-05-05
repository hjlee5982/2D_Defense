using UnityEngine;
using UnityEngine.UI;

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
    #endregion





    #region CHILDREN UI
    public UI_GameStatus     GameStatus  { get; private set; }
    public UI_UnitStatus     UnitStatus  { get; private set; }
    public UI_SpawnAlly      SpawnAlly   { get; private set; }
    public UI_Enhancement    Enhancement { get; private set; }
    public Button            StartButton { get; private set; }
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();

        GameStatus  = transform.Find("GameStatus") .GetComponent<UI_GameStatus>();

        UnitStatus  = transform.GetChild(1).Find("UnitStatus") .GetComponent<UI_UnitStatus>();
        SpawnAlly   = transform.GetChild(1).Find("SpawnAlly")  .GetComponent<UI_SpawnAlly>();
        Enhancement = transform.GetChild(1).Find("Enhancement").GetComponent<UI_Enhancement>();

        transform.GetChild(1).Find("StartButton").GetComponent<Button>().onClick.AddListener(StartButtonClick);
    }

    void Start()
    {

    }

    void Update()
    {

    }
    #endregion





    #region GAMESTATUS
    #endregion





    #region UNITSTATUS
    #endregion





    #region SUMMON
    public void BeginSpawnAlly(int btnIdx)
    {
        //JGameManager.Instance.BeginSpawnAlly(btnIdx);
    }
    #endregion





    #region ENHANCEMENT
    #endregion





    #region STARTBUTTON
    public void StartButtonClick()
    {
        JEventBus.SendEvent(new StartRoundEvent());
    }
    #endregion
}
