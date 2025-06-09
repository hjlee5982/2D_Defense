using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameStatusChangeEvent;

public class UI_GameStatus : MonoBehaviour
{
    #region VARIABLES
    [Header("라이프 카운터")]
    private TextMeshProUGUI _lifeCounter;

    [Header("몬스터 카운터")]
    private TextMeshProUGUI _monsterCounter;

    [Header("골드 카운터")]
    private TextMeshProUGUI _goldCounter;

    [Header("라운드 카운터")]
    private TextMeshProUGUI ID_Round_GameStatus; // 라운드 "텍스트"
    private TextMeshProUGUI _counter; // 실제 라운드 "int"
    private string _temp;

    [Header("배속 버튼 카운트")]
    private TextMeshProUGUI _speedText;
    private int _buttonClickCount = 0;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        _lifeCounter        = transform.Find("Life")   .GetChild(1).GetComponent<TextMeshProUGUI>();
        _monsterCounter     = transform.Find("Monster").GetChild(1).GetComponent<TextMeshProUGUI>();
        _goldCounter        = transform.Find("Gold")   .GetChild(1).GetComponent<TextMeshProUGUI>();
        ID_Round_GameStatus = transform.Find("Round").Find("ID_Round_GameStatus").GetComponent<TextMeshProUGUI>();
        _counter            = transform.Find("Round").Find("Counter").GetComponent<TextMeshProUGUI>();
        _speedText          = transform.Find("SpeedChange").Find("SpeedText").GetComponent<TextMeshProUGUI>();

        transform.Find("SettingButton").GetComponent<Button>().onClick.AddListener(() => 
        {
            JAudioManager.Instance.PlaySFX("ButtonClick");
            JEventBus.SendEvent(new OpenSettingPanelEvent());
        });
        transform.Find("SpeedChange").Find("SpeedChangeButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            SpeedChangeButton();
        });
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<GameStatusChangeEvent>(UpdateGameStatusUI);
        JEventBus.Subscribe<LanguageChangeEvent>(LanguageChange);

        LanguageChange(null);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<GameStatusChangeEvent>(UpdateGameStatusUI);
        JEventBus.Unsubscribe<LanguageChangeEvent>(LanguageChange);
    }
    #endregion





    #region FUNCTIONS
    private void UpdateGameStatusUI(GameStatusChangeEvent e)
    {
        switch(e.Type)
        {
            case GameStatusType.Life:
                _lifeCounter.text = e.Value.ToString();
                break;

            case GameStatusType.NumOfMonster:
                _monsterCounter.text = e.Value.ToString();
                break;

            case GameStatusType.Gold:
                _goldCounter.text = e.Value.ToString();
                break;

            case GameStatusType.Round:
                // TODO : ID_RoundCount
                _counter.text = (e.Value + 1).ToString() + " / " + e.MaxRound.ToString();
                break;
        }
    }

    private void SpeedChangeButton()
    {
        JAudioManager.Instance.PlaySFX("ButtonClick");

        ++_buttonClickCount;

        // 0 -> 1, 1 -> 1.5, 2 -> 2
        if(_buttonClickCount > 2)
        {
            _buttonClickCount = 0;
        }

        float speed = 0.5f * _buttonClickCount + 1;

        _speedText.text = "x" + speed.ToString("0.0");

        JEventBus.SendEvent(new GameSpeedChangeEvent(speed + 0.2f));
    }

    private void LanguageChange(LanguageChangeEvent e)
    {
        ID_Round_GameStatus.text = JSettingManager.Instance.GetText(ID_Round_GameStatus.name);
    }
    #endregion
}
