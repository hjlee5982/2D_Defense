using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enhancement : MonoBehaviour
{
    #region VARIABLES
    [Header("��ȭ ��ư + ������ + ���")]
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
            // �ش� ������
            EnhancementData data = JGameManager.Instance.DataLoader.EnhancementData[i];

            // �� ��ư Transform
            Transform buttonTransform = transform.Find("Option_" + i.ToString()).transform;

            // ��ȭ ��ư �̺�Ʈ ���ε�
            int index = i;
            Button button = buttonTransform.GetComponent<Button>();
            button.onClick.AddListener(() => EnhancementButtonClicked(index));

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
            // ���� ���� ��尡 ���ݺ��� ������ Ȱ��ȭ ����
            bool canActivate = currentGold >= option.cost;

            // Ȱ��ȭ�� �����ϴٸ� �������� �����ؾ� ��
            option.Restrictor.SetActive(!canActivate);
        }
    }
    #endregion
}
