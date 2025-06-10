using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Enhancement : MonoBehaviour
{
    #region VARIABLES
    [Header("��ȭ ��ư + ������ + ���")]
    private List<CostOptionUI> _options = new List<CostOptionUI>();

    [Header("��ȭ�ɼ� �ؽ�Ʈ")]
    private Dictionary<int, Dictionary<string, TextMeshProUGUI>> _optionTexts = new Dictionary<int, Dictionary<string, TextMeshProUGUI>>();
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        for(int i = 0; i < 4; ++i)
        {
            // ��ȭ �ɼ� �ؽ�Ʈ ����
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
            // �ش� ������
            EnhancementData data = JGameSceneManager.Instance.DataLoader.EnhancementData[i];

            // �� ��ư Transform
            Transform buttonTransform = transform.Find("Option_" + i.ToString()).transform;

            // ��ȭ ��ư �̺�Ʈ ���ε�
            int index = i;
            Button button = buttonTransform.GetComponent<Button>();
            button.onClick.AddListener(() => 
            { 
                EnhancementButtonClicked(index);
                EventSystem.current.SetSelectedGameObject(null); 
            }); 

            // ��ȭ Ȯ�� ����
            TextMeshProUGUI probabilityText = buttonTransform.Find("Percentage").GetComponent<TextMeshProUGUI>();
            probabilityText.text = data.Probability.ToString() + "%";

            // ��ȭ ��ġ ����
            if(i != 3)
            {
                buttonTransform.Find("AtkPower").GetComponent<TextMeshProUGUI>().text = "+ " + data.dAtkPower.ToString();
                buttonTransform.Find("AtkRange").GetComponent<TextMeshProUGUI>().text = "+ " + data.dAtkRange.ToString();
                buttonTransform.Find("AtkSpeed").GetComponent<TextMeshProUGUI>().text = "+ " + data.dAtkSpeed.ToString();
            }
            else
            {
                buttonTransform.Find("AtkPower").GetComponent<TextMeshProUGUI>().text = "�� " + data.dAtkPower.ToString();
                buttonTransform.Find("AtkRange").GetComponent<TextMeshProUGUI>().text = "�� " + data.dAtkRange.ToString();
                buttonTransform.Find("AtkSpeed").GetComponent<TextMeshProUGUI>().text = "�� " + data.dAtkSpeed.ToString();
            }

            // ��ȭ ��� ����
            TextMeshProUGUI costText = transform.Find("Option_" + i.ToString()).Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = data.Cost.ToString();

            // ��ȭ ���� UI ����
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
            // ���� ���� ��尡 ���ݺ��� ������ Ȱ��ȭ ����
            bool canActivate = currentGold >= option.cost;

            // Ȱ��ȭ�� �����ϴٸ� �������� �����ؾ� ��
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
