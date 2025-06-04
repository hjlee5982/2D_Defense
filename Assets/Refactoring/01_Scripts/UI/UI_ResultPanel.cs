using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ResultPanel : MonoBehaviour
{
    #region VARIABLES
    [Header("텍스트들")]
    private TextMeshProUGUI ID_Result_ResultPanel;
    private TextMeshProUGUI ID_Round_ResultPanel;
    private TextMeshProUGUI ID_Life_ResultPanel;
    private TextMeshProUGUI ID_Gold_ResultPanel;
    private TextMeshProUGUI ID_Return_ResultPanel;

    [Header("카운트")]
    private TextMeshProUGUI _roundCount;
    private TextMeshProUGUI _lifeCount;
    private TextMeshProUGUI _goldCount;
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        Transform child = transform.GetChild(0);

        child.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(() => 
        {
            SceneManager.LoadSceneAsync("NewTitleScene");
            Time.timeScale = 1f;
            JAudioManager.Instance.PlaySFX("ButtonClick");
        });

        _roundCount = child.Find("RoundCount").GetComponent<TextMeshProUGUI>();
        _lifeCount  = child.Find("LifeCount") .GetComponent<TextMeshProUGUI>();
        _goldCount  = child.Find("GoldCount") .GetComponent<TextMeshProUGUI>();

        LanguageChange(null);
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<LanguageChangeEvent>(LanguageChange);

        LanguageChange(null);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<LanguageChangeEvent>(LanguageChange);
    }
    #endregion





    #region FUNCTIONS
    public void SetCounter(int round, int life, int gold)
    {
        _roundCount.text = round.ToString();
        _lifeCount.text  = life.ToString();
        _goldCount.text  = gold.ToString();
    }


    private void LanguageChange(LanguageChangeEvent e)
    {
        ID_Result_ResultPanel.text = JSettingManager.Instance.GetText(ID_Result_ResultPanel.name);
        ID_Round_ResultPanel .text = JSettingManager.Instance.GetText(ID_Round_ResultPanel.name);
        ID_Life_ResultPanel  .text = JSettingManager.Instance.GetText(ID_Life_ResultPanel.name);
        ID_Gold_ResultPanel  .text = JSettingManager.Instance.GetText(ID_Gold_ResultPanel.name);
        ID_Return_ResultPanel.text = JSettingManager.Instance.GetText(ID_Return_ResultPanel.name);
    }
    #endregion
}
