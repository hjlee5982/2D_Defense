using TMPro;
using UnityEngine;

public class UI_GameStatus : MonoBehaviour
{
    #region VARIABLES
    [Header("������ ī����")]
    private TextMeshProUGUI _lifeCounter;

    [Header("���� ī����")]
    private TextMeshProUGUI _monsterCounter;

    [Header("��� ī����")]
    private TextMeshProUGUI _goldCounter;
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
        _lifeCounter.text    = e.Life.ToString();
        _monsterCounter.text = e.NumOfMonster.ToString();
        _goldCounter.text    = e.Gold.ToString();
    }
    #endregion
}
