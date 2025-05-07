using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitStatus : MonoBehaviour
{
    #region VARIABLES
    [Header("�������� ǥ�� �� UI")]
    private Image           _unitThumbnail;
    private TextMeshProUGUI _unitName;

    [Header("���� �����͸� ǥ�� �� UI")]
    private TextMeshProUGUI _atkPower;
    private TextMeshProUGUI _atkSpeed;
    private TextMeshProUGUI _atkRange;
    private TextMeshProUGUI _upgradeCount;

    [Header("��ۿ� �����̳�")]
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

        // ����� �ʿ��� UI��
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
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<UnitSelectEvent>(UnitSelected);
        JEventBus.Unsubscribe<UnitDeselectEvent>(UnitDeselected);
    }
    #endregion





    #region FUNCTIONS
    private void UnitSelected(UnitSelectEvent e)
    {
        foreach (GameObject go in _ui)
        {
            go.SetActive(true);
        }

        // Profile
        {
            _unitThumbnail.sprite = e.UnitData.Thumbnail;
            _unitName.text        = e.UnitData.UnitName;
        }
        // Status
        {
            _atkPower.text     = e.UnitData.AtkPower.ToString();   
            _atkSpeed.text     = e.UnitData.AtkSpeed.ToString();   
            _atkRange.text     = e.UnitData.AtkRange.ToString();   
            _upgradeCount.text = e.UnitData.UpgradeCount.ToString();
        }
    }

    private void UnitDeselected(UnitDeselectEvent e)
    {
        foreach (GameObject go in _ui)
        {
            go.SetActive(false);
        }
    }
    #endregion
}
