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
    private TextMeshProUGUI _atkSpeed;
    private TextMeshProUGUI _atkRange;
    private TextMeshProUGUI _upgradeCount;

    [Header("토글용 컨테이너")]
    private List<GameObject> _ui = new List<GameObject>();
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
            _ui.Add(Status.Find("AtkSpeed").gameObject);
            _ui.Add(Status.Find("AtkRange").gameObject);
            _ui.Add(Status.Find("AtkRange").gameObject);
            _ui.Add(Status.Find("UpgradeCount").gameObject);

            _ui.Add(transform.GetChild(0).Find("Status").Find("StatusDesc").gameObject);
            _ui.Add(transform.GetChild(0).Find("Status").Find("Colons").gameObject);
        }
        
        foreach(GameObject go in _ui)
        {
            go.SetActive(false);
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
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<UnitSelectEvent>(UnitSelected);
        JEventBus.Unsubscribe<UnitDeselectEvent>(UnitDeselected);
        JEventBus.Unsubscribe<EnhanceCompleteEvent>(EnhanceComplete);
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
        AllyUnitData allyUnitData = allyUnit.AllyUnitData;

        // Profile
        {
            _unitThumbnail.sprite = allyUnitData.Thumbnail;
            _unitName.text        = allyUnitData.UnitName + " " + allyUnitData.Grade.ToString("+0");
        }
        // Status
        {
            _atkPower.text     = allyUnitData.AtkPower.ToString() + " (" + allyUnitData.dAtkPower.ToString("+0;-0;0") + ")";
            _atkRange.text     = allyUnitData.AtkRange.ToString() + " (" + allyUnitData.dAtkRange.ToString("+0;-0;0") + ")";
            _atkSpeed.text     = allyUnitData.AtkSpeed.ToString() + " (" + allyUnitData.dAtkSpeed.ToString("+0;-0;0") + ")"; ;
            _upgradeCount.text = allyUnitData.UpgradeCount.ToString();
        }
    }
    #endregion
}
