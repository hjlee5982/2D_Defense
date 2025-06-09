using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpawnAlly : MonoBehaviour
{
    #region VARIABLES
    [Header("��ȭ ��ư + ������ + ���")]
    private List<CostOptionUI> _options = new List<CostOptionUI>();

    [Header("Ÿ��Ʋ �ؽ�Ʈ")]
    private TextMeshProUGUI ID_Summon_SpawnAlly;

    [Header("�̸� �ؽ�Ʈ")]
    private List<TextMeshProUGUI> ID_UnitName_SummonAlly = new List<TextMeshProUGUI>();

    [Header("�ʱ�ȭ �÷���")]
    private bool _isInitComplete = false;

    [Header("���� ���")]
    private int _currentGold;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        ID_Summon_SpawnAlly = transform.GetChild(0).Find("ID_Summon_SpawnAlly").GetComponent<TextMeshProUGUI>();
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
            TextMeshProUGUI nameText = buttonTransform.Find("ID_UnitName_SummonAlly").GetComponent<TextMeshProUGUI>();
            nameText.text = data.GetName(JSettingManager.Instance.CurrentLanguage);
            ID_UnitName_SummonAlly.Add(nameText);


            // ���� ��� ����
            TextMeshProUGUI costText = buttonTransform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = data.Cost.ToString();

            // ��ȯ ���� UI ����
            GameObject restrictor = transform.Find("Restrictor_" + i.ToString()).gameObject;
            restrictor.SetActive(false);

            _options.Add(new CostOptionUI(button, restrictor, data.Cost));
        }

        UpdateRestrictor(_currentGold);
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        UpdateRestrictor(JGameManager.Instance.Gold);
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
        _currentGold = currentGold;

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
        ID_Summon_SpawnAlly.text = JSettingManager.Instance.GetText(ID_Summon_SpawnAlly.name);

        for(int i = 0; i < 3; ++i)
        {
            if (ID_UnitName_SummonAlly.Count != 0)
            {
                AllyUnitData data = JGameManager.Instance.DataLoader.AllyUnitData[i];

                ID_UnitName_SummonAlly[i].text = data.GetName(JSettingManager.Instance.CurrentLanguage);
            }
            else
            {
                break;
            }
        }
    }
    #endregion
}
