using UnityEngine;
using UnityEngine.UI;

public class UI_GameController : MonoBehaviour
{
    #region VARIABLES
    public Button StartButton { get; private set; }
    #endregion





    #region CHILDREN UI
    public UI_UnitStatus  UnitStatus  { get; private set; }
    public UI_Enhancement Enhancement { get; private set; }
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        StartButton = transform.Find("StartButton").GetComponent<Button>();
        StartButton.onClick.AddListener(StartButtonClick);

        UnitStatus  = transform.Find("UnitStatus") .GetComponent<UI_UnitStatus>();
        Enhancement = transform.Find("Enhancement").GetComponent<UI_Enhancement>();
    }

    void Start()
    {

    }

    void Update()
    {

    }
    #endregion





    #region FUNCTIONS
    public void StartButtonClick()
    {
        Debug.Log("시작 버튼 눌렸어요");
        JGameManager.Instance.StartButtonAction.Invoke();
    }
    #endregion
}
