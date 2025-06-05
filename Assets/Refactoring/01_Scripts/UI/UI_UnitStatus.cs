using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitStatus : MonoBehaviour
{
    #region VARIABLES
    [Header("프로필을 표시 할 UI")]
    private Image           _unitThumbnail;
    private TextMeshProUGUI _unitName;

    [Header("유닛 데이터를 표시 할 UI")]
    private TextMeshProUGUI _atkPower;
    private TextMeshProUGUI _atkRange;
    private TextMeshProUGUI _atkSpeed;
    private TextMeshProUGUI _upgradeCount;

    [Header("토글용 컨테이너")]
    private List<GameObject> _ui = new List<GameObject>();

    [Header("스테이터스 텍스트")]
    private TextMeshProUGUI ID_AtkPower_UnitStatus;
    private TextMeshProUGUI ID_AtkRange_UnitStatus;
    private TextMeshProUGUI ID_AtkSpeed_UnitStatus;
    private TextMeshProUGUI ID_Upgrade_UnitStatus;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        Transform Profile = transform.GetChild(0).Find("Profile");
        {
            _unitThumbnail = Profile.Find("Icon").GetChild(0).GetComponent<Image>();
            _unitName      = Profile.Find("Name").GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        Transform Status  = transform.GetChild(0).Find("Status").Find("Data");
        {
            _atkPower     = Status.Find("AtkPower")    .GetComponent<TextMeshProUGUI>();
            _atkSpeed     = Status.Find("AtkSpeed")    .GetComponent<TextMeshProUGUI>();
            _atkRange     = Status.Find("AtkRange")    .GetComponent<TextMeshProUGUI>();
            _upgradeCount = Status.Find("UpgradeCount").GetComponent<TextMeshProUGUI>();
        }

        // 토글이 필요한 UI들
        {
            _ui.Add(Profile.Find("Icon").GetChild(0).gameObject);
            _ui.Add(Profile.Find("Name").GetChild(0).gameObject);

            _ui.Add(Status.Find("AtkPower").gameObject);
            _ui.Add(Status.Find("AtkRange").gameObject);
            _ui.Add(Status.Find("AtkSpeed").gameObject);
            _ui.Add(Status.Find("AtkRange").gameObject);
            _ui.Add(Status.Find("UpgradeCount").gameObject);

            _ui.Add(transform.GetChild(0).Find("Status").Find("StatusDesc").gameObject);
            _ui.Add(transform.GetChild(0).Find("Status").Find("Colons").gameObject);
        }
        
        foreach(GameObject go in _ui)
        {
            go.SetActive(false);
        }


        Transform desc = transform.GetChild(0).Find("Status").Find("StatusDesc");
        {
            ID_AtkPower_UnitStatus = desc.Find("ID_AtkPower_UnitStatus").GetComponent<TextMeshProUGUI>();
            ID_AtkRange_UnitStatus = desc.Find("ID_AtkRange_UnitStatus").GetComponent<TextMeshProUGUI>();
            ID_AtkSpeed_UnitStatus = desc.Find("ID_AtkSpeed_UnitStatus").GetComponent<TextMeshProUGUI>();
            ID_Upgrade_UnitStatus  = desc.Find("ID_Upgrade_UnitStatus").GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<UnitSelectEvent>(UnitSelected);
        JEventBus.Subscribe<UnitDeselectEvent>(UnitDeselected);
        JEventBus.Subscribe<EnhanceCompleteEvent>(EnhanceComplete);
        JEventBus.Subscribe<LanguageChangeEvent>(LanguageChange);

        LanguageChange(null);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<UnitSelectEvent>(UnitSelected);
        JEventBus.Unsubscribe<UnitDeselectEvent>(UnitDeselected);
        JEventBus.Unsubscribe<EnhanceCompleteEvent>(EnhanceComplete);
        JEventBus.Unsubscribe<LanguageChangeEvent>(LanguageChange);
    }
    #endregion





    #region FUNCTIONS
    private void UnitSelected(UnitSelectEvent e)
    {
        foreach (GameObject go in _ui)
        {
            go.SetActive(true);
        }

        UpdateUI(e.SelectedUnit);
    }

    private void UnitDeselected(UnitDeselectEvent e)
    {
        foreach (GameObject go in _ui)
        {
            go.SetActive(false);
        }
    }

    private void EnhanceComplete(EnhanceCompleteEvent e)
    {
        UpdateUI(e.SelectedUnit);
    }

    private void UpdateUI(AllyUnit allyUnit)
    {
        AllyUnitData allyUnitData = allyUnit.GetUnitData();

        // Profile
        {
            _unitThumbnail.sprite = allyUnit.transform.Find("Thumbnail").GetComponent<SpriteRenderer>().sprite;
            _unitName.text        = allyUnitData.GetName(JSettingManager.Instance.CurrentLanguage) + allyUnitData.Grade.ToString(" + 0");
        }
        // Status
        {
            _atkPower.text     = allyUnitData.AtkPower.ToString() + " (" + allyUnitData.dAtkPower.ToString("+0;-0;0") + ")";
            _atkRange.text     = allyUnitData.AtkRange.ToString() + " (" + allyUnitData.dAtkRange.ToString("+0;-0;0") + ")";
            _atkSpeed.text     = allyUnitData.AtkSpeed.ToString() + " (" + allyUnitData.dAtkSpeed.ToString("+0;-0;0") + ")"; ;
            _upgradeCount.text = allyUnitData.UpgradeCount.ToString();
        }
    }


    private void LanguageChange(LanguageChangeEvent e)
    {
        ID_AtkPower_UnitStatus.text = JSettingManager.Instance.GetText(ID_AtkPower_UnitStatus.name);
        ID_AtkRange_UnitStatus.text = JSettingManager.Instance.GetText(ID_AtkRange_UnitStatus.name);
        ID_AtkSpeed_UnitStatus.text = JSettingManager.Instance.GetText(ID_AtkSpeed_UnitStatus.name);
        ID_Upgrade_UnitStatus .text = JSettingManager.Instance.GetText(ID_Upgrade_UnitStatus.name);
    }
    #endregion
}
