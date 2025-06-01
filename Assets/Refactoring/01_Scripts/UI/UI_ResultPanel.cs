using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ResultPanel : MonoBehaviour
{
    #region VARIABLES
    [Header("Ä«¿îÆ®")]
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
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    #endregion





    #region FUNCTIONS
    public void SetCounter(int round, int life, int gold)
    {
        _roundCount.text = round.ToString();
        _lifeCount.text  = life.ToString();
        _goldCount.text  = gold.ToString();
    }
    #endregion
}
