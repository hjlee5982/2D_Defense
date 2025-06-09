using System.Collections.Generic;
using TMPro;
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
    [Header("���� �������ͽ� UI")]
    private List<GameObject> _unitStatusUI = new List<GameObject>();

    [Header("���� UI")]
    private UI_SpawnAlly _spawnAllyUI;

    [Header("��ȭ UI")]
    private UI_Enhancement _enhancementUI;

    [Header("��� UI")]
    private UI_ResultPanel _resultPanelUI;

    [Header("���� ��ư")]
    private Button _startButton;

    [Header("���� ��ư �ؽ�Ʈ")]
    private TextMeshProUGUI ID_Start_Button_Game;

    [Header("���� �÷���")]
    private bool _isRoundStart = false;

    [Header("��ȯ ��ư")]
    private Button _recallButton;

    [Header("��ȯ ��ư �ؽ�Ʈ")]
    private TextMeshProUGUI ID_Recall_GameController;
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();

        _startButton = transform.GetChild(1).Find("StartButton").GetComponent<Button>();
        _startButton.onClick.AddListener(StartButtonClick);
        ID_Start_Button_Game = _startButton.transform.Find("ID_Start_Button_Game").GetComponent<TextMeshProUGUI>();

        _recallButton = transform.GetChild(1).Find("RecallButton").GetComponent<Button>();
        _recallButton.onClick.AddListener(ReturnButtonClick);
        ID_Recall_GameController = _recallButton.transform.Find("ID_Recall_GameController").GetComponent<TextMeshProUGUI>();

        _recallButton.gameObject.SetActive(false);

        _spawnAllyUI   = transform.Find("GameController").Find("SpawnAlly").GetComponent<UI_SpawnAlly>();
        _enhancementUI = transform.Find("GameController").Find("Enhancement").GetComponent<UI_Enhancement>();
        _resultPanelUI = transform.Find("ResultPanel").GetComponent<UI_ResultPanel>();
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
        JEventBus.Subscribe<EndRoundEvent>(EndRoundEvent);
        JEventBus.Subscribe<GameStartEvent>(GameStartEvent);
        JEventBus.Subscribe<GameEndEvent>(GameEndEvent);
        JEventBus.Subscribe<LanguageChangeEvent>(LanguageChange);

        LanguageChange(null);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<UnitSelectEvent>(UnitSelected);
        JEventBus.Unsubscribe<UnitDeselectEvent>(UnitDeselected);
        JEventBus.Unsubscribe<EndRoundEvent>(EndRoundEvent);
        JEventBus.Unsubscribe<GameStartEvent>(GameStartEvent);
        JEventBus.Unsubscribe<GameEndEvent>(GameEndEvent);
        JEventBus.Unsubscribe<LanguageChangeEvent>(LanguageChange);
    }
    #endregion





    #region STARTBUTTON
    public void StartButtonClick()
    {
        JEventBus.SendEvent(new StartRoundEvent());
        JAudioManager.Instance.PlaySFX("ButtonClick");

        _startButton.gameObject.SetActive(false);
        _spawnAllyUI.gameObject.SetActive(false);
        _enhancementUI.gameObject.SetActive(false);

        _isRoundStart = true;
    }

    public void ReturnButtonClick()
    {
        JEventBus.SendEvent(new UnitRecallPhase1Event());
    }

    private void EndRoundEvent(EndRoundEvent e)
    {
        _startButton.gameObject.SetActive(true);
        _spawnAllyUI.gameObject.SetActive(true);
        _enhancementUI.gameObject.SetActive(false);

        _isRoundStart = false;
    }

    private void GameStartEvent(GameStartEvent e)
    {

    }

    private void GameEndEvent(GameEndEvent e)
    {
        _resultPanelUI.gameObject.SetActive(true);
        _resultPanelUI.SetCounter(e.Round, e.Life, e.Gold);
    }

    private void UnitSelected(UnitSelectEvent e)
    {
        if(_isRoundStart == false)
        {
            _spawnAllyUI.gameObject.SetActive(false);
            _enhancementUI.gameObject.SetActive(true);
            _recallButton.gameObject.SetActive(true);
        }
    }

    private void UnitDeselected(UnitDeselectEvent e)
    {
        if (_isRoundStart == false)
        {
            _spawnAllyUI.gameObject.SetActive(true);
            _enhancementUI.gameObject.SetActive(false);
            _recallButton.gameObject.SetActive(false);
        }
    }

    private void LanguageChange(LanguageChangeEvent e)
    {
        ID_Start_Button_Game.text     = JSettingManager.Instance.GetText(ID_Start_Button_Game.name);
        ID_Recall_GameController.text = JSettingManager.Instance.GetText(ID_Recall_GameController.name);
    }
    #endregion
}
