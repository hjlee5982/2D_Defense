using UnityEngine;
using UnityEngine.UI;

public class UI_Enhancement : MonoBehaviour
{
    #region VARIABLES
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        transform.Find("Option_1").GetComponent<Button>().onClick.AddListener(() => EnhancementButtonClicked(0));
        transform.Find("Option_2").GetComponent<Button>().onClick.AddListener(() => EnhancementButtonClicked(1));
        transform.Find("Option_3").GetComponent<Button>().onClick.AddListener(() => EnhancementButtonClicked(2));
        transform.Find("Option_4").GetComponent<Button>().onClick.AddListener(() => EnhancementButtonClicked(3));
    }

    void Start()
    {
    }

    void Update()
    {
    }
    #endregion





    #region FUNCTIONS
    public void EnhancementButtonClicked(int btnIdx)
    {
        // UI_Enhancement -> JGameManager
        JEventBus.SendEvent(new StartEnhancementEvent(btnIdx));
    }

    #endregion
}
