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
    private TextMeshProUGUI _roundCounter;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        _lifeCounter    = transform.Find("Life")   .GetChild(1).GetComponent<TextMeshProUGUI>();
        _monsterCounter = transform.Find("Monster").GetChild(1).GetComponent<TextMeshProUGUI>();
        _goldCounter    = transform.Find("Gold")   .GetChild(1).GetComponent<TextMeshProUGUI>();
        _roundCounter   = transform.Find("Round").GetComponent<TextMeshProUGUI>();
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
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<GameStatusChangeEvent>(UpdateGameStatusUI);
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
                string str = "현재 라운드 : " + (e.Value + 1).ToString() + " / " + e.MaxRound.ToString();
                _roundCounter.text = str;
                break;
        }
    }
    #endregion
}
