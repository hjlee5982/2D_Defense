using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JSettingManager : MonoBehaviour
{
    #region SINGLETON
    public static JSettingManager Instance { get; private set; }

    private void SingletonInitialize()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion





    #region VARIABLES
    [Header("로컬라이저")]
    private Dictionary<string, Dictionary<string, string>> _localizer = new Dictionary<string, Dictionary<string, string>>();

    [Header("언어설정")]
    public string CurrentLanguage = "KR";

    [Header("옵션 UI들")]
    private Slider   _bgmSlider;
    private Slider   _sfxSlider;
    private Toggle   _bgmToggle;
    private Toggle   _sfxToggle;
    private TMP_Dropdown _languageDropdown;
    private Button   _saveButton;

    [Header("UI 텍스트")]
    private TextMeshProUGUI ID_BGM_Setting;
    private TextMeshProUGUI ID_SFX_Setting;
    private TextMeshProUGUI ID_Language_Setting;
    private TextMeshProUGUI ID_Save_Setting;
    private TextMeshProUGUI ID_Mute_Setting_1;
    private TextMeshProUGUI ID_Mute_Setting_2;

    [Header("토글용 변수")]
    private float _prevBGMValue = 0f;
    private float _prevSFXValue = 0f;
    #endregion




    #region MONOBEHAVIOUR
    private void Awake()
    {
        SingletonInitialize();

        LocalizeDataProcess();

        Transform child = transform.GetChild(0);
        {
            _bgmSlider        = child.Find("BGM_Slider"       ).GetComponent<Slider>();
            _bgmToggle        = child.Find("BGM_MuteToggle"   ).GetComponent<Toggle>();

            _sfxSlider        = child.Find("SFX_Slider"       ).GetComponent<Slider>();
            _sfxToggle        = child.Find("SFX_MuteToggle"   ).GetComponent<Toggle>();

            _languageDropdown = child.Find("Language_Dropdown").GetComponent<TMP_Dropdown>();
            
            _saveButton       = child.Find("SaveButton"       ).GetComponent<Button>();
        }
        {
            _bgmSlider.value = 0.5f;
            _sfxSlider.value = 0.5f;
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
            ID_BGM_Setting      = child.Find("ID_BGM_Setting"     ).GetComponent<TextMeshProUGUI>();
            ID_SFX_Setting      = child.Find("ID_SFX_Setting"     ).GetComponent<TextMeshProUGUI>();
            ID_Language_Setting = child.Find("ID_Language_Setting").GetComponent<TextMeshProUGUI>();
            ID_Save_Setting     = child.Find("SaveButton").Find("ID_Save_Setting").GetComponent<TextMeshProUGUI>();
            ID_Mute_Setting_1   = _bgmToggle.transform.Find("ID_Mute_Setting").GetComponent<TextMeshProUGUI>();
            ID_Mute_Setting_2   = _sfxToggle.transform.Find("ID_Mute_Setting").GetComponent<TextMeshProUGUI>();
        }


        gameObject.SetActive(false);
    }

    private void Start()
    {
        JEventBus.SendEvent(new LanguageChangeEvent());
    }

    private void OnEnable()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas != null)
        {
            transform.SetParent(canvas.transform, false);
        }
    }
    #endregion





    #region FUNCTION
    public string GetText(string ID)
    {
        return _localizer[ID][CurrentLanguage];
    }

    private void LocalizeDataProcess()
    {
        _localizer = JDataLoader.Instance.LocalizeData.ToDictionary(kvp => kvp.Key, kvp =>
        {
            var dict = new Dictionary<string, string>();
            var fields = typeof(LocalizeData).GetFields();

            foreach (var field in fields)
            {
                if(field.Name == "ID")
                {
                    continue;
                }

                var value = field.GetValue(kvp.Value)?.ToString() ?? "";
                dict[field.Name] = value;
            }

            return dict;
        });
    }

    private void SetBGMVolume(float value)
    {
        JAudioManager.Instance.SetBGMVolume(value);
    }

    private void SetSFXVolume(float value)
    {
        JAudioManager.Instance.SetSFXVolume(value);
    }

    private void ToggleBGM(bool value)
    {
        JAudioManager.Instance.PlaySFX("ButtonClick");

        // 체크가 된 상태 = true
        if (value == true)
        {
            JAudioManager.Instance.ToggleBGM(true);
        }
        else
        {
            JAudioManager.Instance.ToggleBGM(false);
        }
    }

    private void ToggleSFX(bool value)
    {
        JAudioManager.Instance.PlaySFX("ButtonClick");

        // 체크가 된 상태 = true
        if (value == true)
        {
            JAudioManager.Instance.ToggleSFX(true);
        }
        else
        {
            JAudioManager.Instance.ToggleSFX(false);
        }
    }

    private void LanguageSelect(int index)
    {
        LanguageChange(index);
    }

    private void ClickSaveButton()
    {
        JAudioManager.Instance.PlaySFX("ButtonClick");

        gameObject.SetActive(false);
    }

    private void LanguageChange(int index)
    {
        switch (index)
        {
            case 0:
                CurrentLanguage = "KR";
                break;
            case 1:
                CurrentLanguage = "EN";
                break;
            case 2:
                CurrentLanguage = "JP";
                break;
            case 3:
                CurrentLanguage = "CN";
                break;
        }

        ID_BGM_Setting      .text = _localizer[ID_BGM_Setting     .name][CurrentLanguage];
        ID_SFX_Setting      .text = _localizer[ID_SFX_Setting     .name][CurrentLanguage];
        ID_Language_Setting .text = _localizer[ID_Language_Setting.name][CurrentLanguage];
        ID_Save_Setting     .text = _localizer[ID_Save_Setting    .name][CurrentLanguage];
        ID_Mute_Setting_1   .text = _localizer["ID_Mute_Setting"][CurrentLanguage];
        ID_Mute_Setting_2   .text = _localizer["ID_Mute_Setting"][CurrentLanguage];

        JEventBus.SendEvent(new LanguageChangeEvent());
    }
    #endregion
}