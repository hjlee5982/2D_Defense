using TMPro;
using UnityEngine;
using static GameStatusChangeEvent;

public class UI_GameStatus : MonoBehaviour
{
    #region VARIABLES
    [Header("라이프 카운터")]
    private TextMeshProUGUI _lifeCounter;
    private int _currentLifeCount;

    [Header("몬스터 카운터")]
    private TextMeshProUGUI _monsterCounter;
    private int _currentMonsterCount;

    [Header("골드 카운터")]
    private TextMeshProUGUI _goldCounter;
    private int _currentGoldCount;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        _lifeCounter    = transform.Find("Life")   .GetChild(1).GetComponent<TextMeshProUGUI>();
        _monsterCounter = transform.Find("Monster").GetChild(1).GetComponent<TextMeshProUGUI>();
        _goldCounter    = transform.Find("Gold")   .GetChild(1).GetComponent<TextMeshProUGUI>();
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
        JEventBus.Subscribe<GameStatusChangeEvent>(UpdateGameStatusUI);
    }
    #endregion





    #region FUNCTIONS
    private void UpdateGameStatusUI(GameStatusChangeEvent e)
    {
        switch(e.Type)
        {
            case GameStatusType.Life:
                _currentLifeCount += e.Value;
                _lifeCounter.text = _currentLifeCount.ToString();
                break;

            case GameStatusType.NumOfMonster:
                _currentMonsterCount += e.Value;
                _monsterCounter.text = _currentMonsterCount.ToString();
                break;

            case GameStatusType.Gold:
                _currentGoldCount += e.Value;
                _goldCounter.text = _currentGoldCount.ToString();
                break;
        }
    }
    #endregion
}
