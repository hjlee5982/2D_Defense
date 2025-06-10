using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Enhancement : MonoBehaviour
{
    #region VARIABLES
    [Header("강화 버튼 + 가림막 + 비용")]
    private List<CostOptionUI> _options = new List<CostOptionUI>();

    [Header("강화옵션 텍스트")]
    private Dictionary<int, Dictionary<string, TextMeshProUGUI>> _optionTexts = new Dictionary<int, Dictionary<string, TextMeshProUGUI>>();
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        for(int i = 0; i < 4; ++i)
        {
            // 강화 옵션 텍스트 설정
            Dictionary<string, TextMeshProUGUI> _optionText = new Dictionary<string, TextMeshProUGUI>();
            {
                _optionText.Add("ID_AtkPower_Enhancement", transform.GetChild(i).Find("ID_AtkPower_Enhancement").GetComponent<TextMeshProUGUI>());
                _optionText.Add("ID_AtkRange_Enhancement", transform.GetChild(i).Find("ID_AtkRange_Enhancement").GetComponent<TextMeshProUGUI>());
                _optionText.Add("ID_AtkSpeed_Enhancement", transform.GetChild(i).Find("ID_AtkSpeed_Enhancement").GetComponent<TextMeshProUGUI>());
            }
            _optionTexts.Add(i, _optionText);
        }
    }

    void Start()
    {
        for (int i = 0; i < 4; ++i)
        {
            // 해당 데이터
            EnhancementData data = JGameSceneManager.Instance.DataLoader.EnhancementData[i];

            // 각 버튼 Transform
            Transform buttonTransform = transform.Find("Option_" + i.ToString()).transform;

            // 강화 버튼 이벤트 바인딩
            int index = i;
            Button button = buttonTransform.GetComponent<Button>();
            button.onClick.AddListener(() => 
            { 
                EnhancementButtonClicked(index);
                EventSystem.current.SetSelectedGameObject(null); 
            }); 

            // 강화 확률 설정
            TextMeshProUGUI probabilityText = buttonTransform.Find("Percentage").GetComponent<TextMeshProUGUI>();
            probabilityText.text = data.Probability.ToString() + "%";

            // 강화 수치 설정
            if(i != 3)
            {
                buttonTransform.Find("AtkPower").GetComponent<TextMeshProUGUI>().text = "+ " + data.dAtkPower.ToString();
                buttonTransform.Find("AtkRange").GetComponent<TextMeshProUGUI>().text = "+ " + data.dAtkRange.ToString();
                buttonTransform.Find("AtkSpeed").GetComponent<TextMeshProUGUI>().text = "+ " + data.dAtkSpeed.ToString();
            }
            else
            {
                buttonTransform.Find("AtkPower").GetComponent<TextMeshProUGUI>().text = "± " + data.dAtkPower.ToString();
                buttonTransform.Find("AtkRange").GetComponent<TextMeshProUGUI>().text = "± " + data.dAtkRange.ToString();
                buttonTransform.Find("AtkSpeed").GetComponent<TextMeshProUGUI>().text = "± " + data.dAtkSpeed.ToString();
            }

            // 강화 비용 설정
            TextMeshProUGUI costText = transform.Find("Option_" + i.ToString()).Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = data.Cost.ToString();

            // 강화 제한 UI 설정
            GameObject restrictor = transform.Find("Restrictor_" + i.ToString()).gameObject;
            restrictor.SetActive(false);
            
            _options.Add(new CostOptionUI(button, restrictor, data.Cost));
        }
    }

    private void OnEnable()
    {
        UpdateRestrictor(JGameSceneManager.Instance.Gold);

        JEventBus.Subscribe<GoldRestrictionEvent>(GoldChanged);
        JEventBus.Subscribe<LanguageChangeEvent>(LanguageChange);

        LanguageChange(null);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<GoldRestrictionEvent>(GoldChanged);
        JEventBus.Unsubscribe<LanguageChangeEvent>(LanguageChange);
    }
    #endregion





    #region FUNCTIONS
    public void EnhancementButtonClicked(int btnIdx)
    {
        // UI_Enhancement -> JGameManager
        JEventBus.SendEvent(new StartEnhancementEvent(btnIdx));
    }

    private void GoldChanged(GoldRestrictionEvent e)
    {
        UpdateRestrictor(e.CurrentGold);
    }

    private void UpdateRestrictor(int currentGold)
    {
        foreach (var option in _options)
        {
            // 현재 소지 골드가 가격보다 많으면 활성화 가능
            bool canActivate = currentGold >= option.cost;

            // 활성화가 가능하다면 가림막은 해제해야 함
            option.Restrictor.SetActive(!canActivate);
        }
    }


    private void LanguageChange(LanguageChangeEvent e)
    {
        foreach(var kvp_1 in _optionTexts)
        {
            foreach(var kvp_2 in kvp_1.Value)
            {
                kvp_2.Value.text = JSettingManager.Instance.GetText(kvp_2.Key);
            }
        }
    }
    #endregion
}
