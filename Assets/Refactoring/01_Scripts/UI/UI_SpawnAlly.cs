using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpawnAlly : MonoBehaviour
{
    #region VARIABLES
    [Header("강화 버튼 + 가림막 + 비용")]
    private List<CostOptionUI> _options = new List<CostOptionUI>();

    [Header("타이틀 텍스트")]
    private TextMeshProUGUI ID_Summon_SpawnAlly;

    [Header("이름 텍스트")]
    private List<TextMeshProUGUI> ID_UnitName_SummonAlly = new List<TextMeshProUGUI>();

    [Header("초기화 플래그")]
    private bool _isInitComplete = false;

    [Header("현재 골드")]
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
            // 해당 데이터
            AllyUnitData data = JGameManager.Instance.DataLoader.AllyUnitData[i];

            // 각 버튼 Transform
            Transform buttonTransform = transform.Find("SummonButton_" + i.ToString()).transform;

            // 소환 버튼 이벤트 바인딩
            int index = i;
            Button button = buttonTransform.GetComponent<Button>();
            button.onClick.AddListener(() => 
            { 
                BeginSpawnAlly(index); 
                JAudioManager.Instance.PlaySFX("ButtonClick"); 
            });

            // 유닛 이름 설정
            TextMeshProUGUI nameText = buttonTransform.Find("ID_UnitName_SummonAlly").GetComponent<TextMeshProUGUI>();
            nameText.text = data.GetName(JSettingManager.Instance.CurrentLanguage);
            ID_UnitName_SummonAlly.Add(nameText);


            // 유닛 비용 설정
            TextMeshProUGUI costText = buttonTransform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = data.Cost.ToString();

            // 소환 제한 UI 설정
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
            // 현재 소지 골드가 가격보다 많으면 활성화 가능
            bool canActivate = currentGold >= option.cost;

            // 활성화가 가능하다면 가림막은 해제해야 함
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
