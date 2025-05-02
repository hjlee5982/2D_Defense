using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UI_Summon : MonoBehaviour
{
    #region VARIABLES
    private Button _summonButton_1;
    private Button _summonButton_2;
    private Button _summonButton_3;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        _summonButton_1 = transform.Find("SummonButton_1").GetComponent<Button>();
        _summonButton_2 = transform.Find("SummonButton_2").GetComponent<Button>();
        _summonButton_3 = transform.Find("SummonButton_3").GetComponent<Button>();

        _summonButton_1.onClick.AddListener(() => BeginSpawnAlly(0));
        _summonButton_2.onClick.AddListener(() => BeginSpawnAlly(1));
        _summonButton_3.onClick.AddListener(() => BeginSpawnAlly(2));
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
        JUIManager.Instance.BeginSpawnAlly(btnIdx);
    }
    #endregion
}
