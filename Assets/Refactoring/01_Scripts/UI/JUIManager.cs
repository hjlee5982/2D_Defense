using System.Collections.Generic;
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
    [Header("유닛 스테이터스 UI")]
    private List<GameObject> _unitStatusUI = new List<GameObject>();

    [Header("스폰 UI")]
    private UI_SpawnAlly _spawnAllyUI;

    [Header("강화 UI")]
    private UI_Enhancement _enhancementUI;
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();

        transform.GetChild(1).Find("StartButton").GetComponent<Button>().onClick.AddListener(StartButtonClick);
    
        _spawnAllyUI   = transform.Find("GameController").Find("SpawnAlly").GetComponent<UI_SpawnAlly>();
        _enhancementUI = transform.Find("GameController").Find("Enhancement").GetComponent<UI_Enhancement>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnEnable()
    {
        JEventBus.Subscribe<UnitSelectEvent>(UnitSelected);
        JEventBus.Subscribe<UnitDeselectEvent>(UnitDeselected);
    }

    private void OnDisable()
    {
        JEventBus.Subscribe<UnitSelectEvent>(UnitSelected);
        JEventBus.Subscribe<UnitDeselectEvent>(UnitDeselected);
    }
    #endregion





    #region STARTBUTTON
    public void StartButtonClick()
    {
        JEventBus.SendEvent(new StartRoundEvent());
    }

    private void UnitSelected(UnitSelectEvent e)
    {
        _spawnAllyUI.gameObject.SetActive(false);
        _enhancementUI.gameObject.SetActive(true);
    }

    private void UnitDeselected(UnitDeselectEvent e)
    {
        _spawnAllyUI.gameObject.SetActive(true);
        _enhancementUI.gameObject.SetActive(false);
    }
    #endregion
}
