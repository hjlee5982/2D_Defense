using UnityEngine;
using UnityEngine.UI;

public class UI_SpawnAlly : MonoBehaviour
{
    #region VARIABLES
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        transform.Find("SummonButton_1").GetComponent<Button>().onClick.AddListener(() => BeginSpawnAlly(0));
        transform.Find("SummonButton_2").GetComponent<Button>().onClick.AddListener(() => BeginSpawnAlly(1));
        transform.Find("SummonButton_3").GetComponent<Button>().onClick.AddListener(() => BeginSpawnAlly(2));
        transform.Find("SummonButton_3").GetComponent<Button>().onClick.AddListener(() => BeginSpawnAlly(2));
    }

    void Start()
    {
    }

    void Update()
    {
    }
    #endregion





    #region FUNCTIONS
    private void BeginSpawnAlly(int btnIdx)
    {
        // UI_SpawnAlly -> JGameManager
        JEventBus.SendEvent(new StartSpawnAllyEvent(btnIdx));
    }
    #endregion
}
