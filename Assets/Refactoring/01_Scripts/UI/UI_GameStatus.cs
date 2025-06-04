using TMPro;
using UnityEngine;
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
    private TextMeshProUGUI ID_Round_GameStatus;
    private string _temp;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        _lifeCounter        = transform.Find("Life")   .GetChild(1).GetComponent<TextMeshProUGUI>();
        _monsterCounter     = transform.Find("Monster").GetChild(1).GetComponent<TextMeshProUGUI>();
        _goldCounter        = transform.Find("Gold")   .GetChild(1).GetComponent<TextMeshProUGUI>();
        ID_Round_GameStatus = transform.Find("ID_Round_GameStatus").GetComponent<TextMeshProUGUI>();
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
                ID_Round_GameStatus.text = _temp + " : " + (e.Value + 1).ToString() + " / " + e.MaxRound.ToString(); ;
                break;
        }
    }


    private void LanguageChange(LanguageChangeEvent e)
    {
        ID_Round_GameStatus.text = JSettingManager.Instance.GetText(ID_Round_GameStatus.name);

        _temp = ID_Round_GameStatus.text;
    }
    #endregion
}
