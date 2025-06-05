using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static SettingValueChangeEvent;

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

            _languageDropdown.onValueChanged.AddListener(LanguageSelect);

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

        SettingValue initialValue = JSettingManager.Instance.SettingValue;
        {
            _bgmSlider.value        = initialValue.BGM_Slider_Value;
            _sfxSlider.value        = initialValue.SFX_Slider_Value;
            _bgmToggle.isOn         = initialValue.BGM_Toggle_Value;
            _sfxToggle.isOn         = initialValue.SFX_Toggle_Value;
            _languageDropdown.value = initialValue.LanguageIndex;
        }

        _child1 = transform.GetChild(0).gameObject;
        _child2 = transform.GetChild(1).gameObject;

        _child1.SetActive(false);
        _child2.SetActive(false);
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<LanguageChangeEvent>(LanguageChange);
        JEventBus.Subscribe<OpenSettingPanelEvent>(ToggleSettingPanel);

        LanguageChange(null);
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
    }

    private void SetBGMVolume(float value)
    {
        Debug.Log("bgm슬라이더");
        SentValueChangeEvent();
    }

    private void SetSFXVolume(float value)
    {
        Debug.Log("sfx슬라이더");
        SentValueChangeEvent();
    }

    private void ToggleBGM(bool value)
    {
        Debug.Log("sfx슬라이더");
        JAudioManager.Instance.PlaySFX("ButtonClick");
        SentValueChangeEvent();
    }

    private void ToggleSFX(bool value)
    {
        Debug.Log("sfx토글");
        JAudioManager.Instance.PlaySFX("ButtonClick");
        SentValueChangeEvent();
    }

    private void LanguageSelect(int index)
    {
        Debug.Log("드롭다운");
        SentValueChangeEvent();
    }

    private void SentValueChangeEvent()
    {
        SettingValue Values = new SettingValue();
        {
            Values.BGM_Slider_Value = _bgmSlider.value;
            Values.BGM_Toggle_Value = _bgmToggle.isOn;
            Values.SFX_Slider_Value = _sfxSlider.value;
            Values.SFX_Toggle_Value = _sfxToggle.isOn;
            Values.LanguageIndex    = _languageDropdown.value;
        }
        JEventBus.SendEvent(new SettingValueChangeEvent(Values));
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