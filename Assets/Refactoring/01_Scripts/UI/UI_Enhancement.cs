using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enhancement : MonoBehaviour
{
    #region VARIABLES
    [Header("강화 버튼 + 가림막 + 비용")]
    private List<CostOptionUI> _options = new List<CostOptionUI>();
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
    }

    void Start()
    {
        for (int i = 0; i < 4; ++i)
        {
            // 해당 데이터
            EnhancementData data = JGameManager.Instance.DataLoader.EnhancementData[i];

            // 각 버튼 Transform
            Transform buttonTransform = transform.Find("Option_" + i.ToString()).transform;

            // 강화 버튼 이벤트 바인딩
            int index = i;
            Button button = buttonTransform.GetComponent<Button>();
            button.onClick.AddListener(() => EnhancementButtonClicked(index));

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

    void Update()
    {
    }

    private void OnEnable()
    {
        UpdateRestrictor(JGameManager.Instance.Gold);
        JEventBus.Subscribe<GoldRestrictionEvent>(GoldChanged);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<GoldRestrictionEvent>(GoldChanged);
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
    #endregion
}
