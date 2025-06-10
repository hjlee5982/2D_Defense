using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static SettingData;

public class UI_SettingPanel : MonoBehaviour
{
    #region VARIABLES
    [Header("옵션 UI들")]
    private Slider       _bgmSlider;
    private Slider       _sfxSlider;
    private Toggle       _bgmToggle;
    private Toggle       _sfxToggle;
    private TMP_Dropdown _languageDropdown;
    private Button       _saveButton;

    [Header("UI 텍스트")]
    private TextMeshProUGUI ID_BGM_Setting;
    private TextMeshProUGUI ID_SFX_Setting;
    private TextMeshProUGUI ID_Language_Setting;
    private TextMeshProUGUI ID_Save_Setting;
    private TextMeshProUGUI ID_Mute_Setting_1;
    private TextMeshProUGUI ID_Mute_Setting_2;

    [Header("자식 게임오브젝트")]
    private GameObject _child1;
    private GameObject _child2;

    [Header("설정 데이터")]
    private SettingData _settingData;

    [Header("초기화 플래그")]
    private bool _isInitComplete = false;
    #endregion




    #region MONOBEHAVIOUR
    private void Awake()
    {
        Transform child = transform.Find("SettingWindowUI");
        {
            _bgmSlider        = child.Find("BGM_Slider").GetComponent<Slider>();
            _bgmToggle        = child.Find("BGM_MuteToggle").GetComponent<Toggle>();
                              
            _sfxSlider        = child.Find("SFX_Slider").GetComponent<Slider>();
            _sfxToggle        = child.Find("SFX_MuteToggle").GetComponent<Toggle>();

            _languageDropdown = child.Find("Language_Dropdown").GetComponent<TMP_Dropdown>();

            _saveButton       = child.Find("SaveButton").GetComponent<Button>();
        }
        {
            _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
            _bgmToggle.onValueChanged.AddListener(ToggleBGM);

            _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            _sfxToggle.onValueChanged.AddListener(ToggleSFX);

            _languageDropdown.onValueChanged.AddListener((index) => LanguageSelect(index));

            _saveButton.onClick.AddListener(ClickSaveButton);
        }
        {
            ID_BGM_Setting      = child.Find("ID_BGM_Setting").GetComponent<TextMeshProUGUI>();
            ID_SFX_Setting      = child.Find("ID_SFX_Setting").GetComponent<TextMeshProUGUI>();
            ID_Language_Setting = child.Find("ID_Language_Setting").GetComponent<TextMeshProUGUI>();
            ID_Save_Setting     = child.Find("SaveButton").Find("ID_Save_Setting").GetComponent<TextMeshProUGUI>();
            ID_Mute_Setting_1   = _bgmToggle.transform.Find("ID_Mute_Setting").GetComponent<TextMeshProUGUI>();
            ID_Mute_Setting_2   = _sfxToggle.transform.Find("ID_Mute_Setting").GetComponent<TextMeshProUGUI>();
        }

        _child1 = transform.GetChild(0).gameObject;
        _child2 = transform.GetChild(1).gameObject;

        _child1.SetActive(false);
        _child2.SetActive(false);
    }

    private void Start()
    {
        SettingData data = JSettingManager.Instance.GetSettingData();

        if (data == null)
        {
            _settingData = JDataLoader.Instance.SettingData[0];
        }
        else
        {
            _settingData = data;
        }
        {
            _bgmSlider.value = _settingData.BGM_Slider_Value;
            _sfxSlider.value = _settingData.SFX_Slider_Value;
            _bgmToggle.isOn = _settingData.BGM_Toggle_Value;
            _sfxToggle.isOn = _settingData.SFX_Toggle_Value;
            _languageDropdown.value = _settingData.LanguageIndex;
            // _bgmToggle.SetIsOnWithoutNotify(_settingData.BGM_Toggle_Value);
            // _sfxToggle.SetIsOnWithoutNotify(_settingData.SFX_Toggle_Value);
            // _languageDropdown.SetValueWithoutNotify(_settingData.LanguageIndex);
        }

        JEventBus.Subscribe<LanguageChangeEvent>(LanguageChange);
        JEventBus.Subscribe<OpenSettingPanelEvent>(ToggleSettingPanel);

        LanguageChange(null);

        _isInitComplete = true;
    }

    private void OnEnable()
    {
        int a = 0;
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<LanguageChangeEvent>(LanguageChange);
        JEventBus.Unsubscribe<OpenSettingPanelEvent>(ToggleSettingPanel);
    }
    #endregion





    #region FUNCTION
    private void ToggleSettingPanel(OpenSettingPanelEvent e)
    {
        _child1.SetActive(!_child1.activeSelf);
        _child2.SetActive(!_child2.activeSelf);
    }

    private void ClickSaveButton()
    {
        JAudioManager.Instance.PlaySFX("ButtonClick");
        _child1.SetActive(!_child1.activeSelf);
        _child2.SetActive(!_child2.activeSelf);

        JEventBus.SendEvent(new SaveButtonClickEvent());
    }

    private void SetBGMVolume(float value)
    {
        _bgmToggle.isOn = false;
        _settingData.BGM_Slider_Value = value;
        _settingData.Option = SettingOption.BGM_Slider;
        JEventBus.SendEvent(new SettingValueChangeEvent(_settingData));
    }

    private void SetSFXVolume(float value)
    {
        _sfxToggle.isOn = false;

        _settingData.SFX_Slider_Value = value;
        _settingData.Option = SettingOption.SFX_Slider;
        JEventBus.SendEvent(new SettingValueChangeEvent(_settingData));
    }

    private void ToggleBGM(bool value)
    {
        if(_isInitComplete == true)
        {
            JAudioManager.Instance.PlaySFX("ButtonClick");
        }
        _settingData.BGM_Toggle_Value = value;
        _settingData.Option = SettingOption.BGM_Toggle;
        JEventBus.SendEvent(new SettingValueChangeEvent(_settingData));
    }

    private void ToggleSFX(bool value)
    {
        if (_isInitComplete == true)
        {
            JAudioManager.Instance.PlaySFX("ButtonClick");
        }
        _settingData.SFX_Toggle_Value = value;
        _settingData.Option = SettingOption.SFX_Toggle;
        JEventBus.SendEvent(new SettingValueChangeEvent(_settingData));
    }

    private void LanguageSelect(int index)
    {
        if (_isInitComplete == true)
        {
            JAudioManager.Instance.PlaySFX("ButtonClick");
        }
        _settingData.LanguageIndex = index;
        _settingData.Option = SettingOption.Language_Dropdown;
        JEventBus.SendEvent(new SettingValueChangeEvent(_settingData));
    }

    private void LanguageChange(LanguageChangeEvent e)
    {
        ID_BGM_Setting.text      = JSettingManager.Instance.GetText(ID_BGM_Setting.name);
        ID_SFX_Setting.text      = JSettingManager.Instance.GetText(ID_SFX_Setting.name);
        ID_Language_Setting.text = JSettingManager.Instance.GetText(ID_Language_Setting.name);
        ID_Save_Setting.text     = JSettingManager.Instance.GetText(ID_Save_Setting.name);
        ID_Mute_Setting_1.text   = JSettingManager.Instance.GetText(ID_Mute_Setting_1.name);
        ID_Mute_Setting_2.text   = JSettingManager.Instance.GetText(ID_Mute_Setting_2.name);
    }
    #endregion
}