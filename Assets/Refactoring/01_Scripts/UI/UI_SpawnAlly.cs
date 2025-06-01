using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpawnAlly : MonoBehaviour
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
        for(int i = 0; i < 3; ++i)
        {
            // �ش� ������
            AllyUnitData data = JGameManager.Instance.DataLoader.AllyUnitData[i];

            // �� ��ư Transform
            Transform buttonTransform = transform.Find("SummonButton_" + i.ToString()).transform;

            // ��ȯ ��ư �̺�Ʈ ���ε�
            int index = i;
            Button button = buttonTransform.GetComponent<Button>();
            button.onClick.AddListener(() => 
            { 
                BeginSpawnAlly(index); 
                JAudioManager.Instance.PlaySFX("ButtonClick"); 
            });

            // ���� �̸� ����
            TextMeshProUGUI nameText = buttonTransform.Find("Name").GetComponent<TextMeshProUGUI>();
            nameText.text = data.UnitName;

            // ���� ��� ����
            TextMeshProUGUI costText = buttonTransform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = data.Cost.ToString();

            // ��ȯ ���� UI ����
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
    private void BeginSpawnAlly(int btnIdx)
    {
        // UI_SpawnAlly -> JGameManager
        JEventBus.SendEvent(new StartSpawnAllyEvent(btnIdx));
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
