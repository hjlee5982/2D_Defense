using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using static SettingData;

public class JSettingManager : MonoBehaviour
{
    #region SINGLETON
    public static JSettingManager Instance { get; private set; }

    private bool SingletonInitialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }
    #endregion





    #region VARIABLES
    [Header("로컬라이저")]
    private Dictionary<string, Dictionary<string, string>> _localizer = new Dictionary<string, Dictionary<string, string>>();

    [Header("초기 언어설정")]
    public string CurrentLanguage = "KR";

    [Header("설정 데이터 클래스")]
    private SettingData _settingData = new SettingData();

    [Serializable]
    public class SettingDaraWrapper
    {
        public List<SettingData> Items = new List<SettingData>();
    }
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        if(!SingletonInitialize())
        {
            return; ;
        }

        LocalizeDataProcess();
    }

    private void Start()
    {
        _settingData = JDataLoader.Instance.SettingData[0];
        SendSettingValue(null);
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<SettingValueChangeEvent>(SendSettingValue);
        JEventBus.Subscribe<SaveButtonClickEvent>(SaveSetting);
    }
    

    private void OnDisable()
    {
        JEventBus.Unsubscribe<SettingValueChangeEvent>(SendSettingValue);
        JEventBus.Unsubscribe<SaveButtonClickEvent>(SaveSetting);
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
                if (field.Name == "ID")
                {
                    continue;
                }

                var value = field.GetValue(kvp.Value)?.ToString() ?? "";
                dict[field.Name] = value;
            }

            return dict;
        });
    }

    private void SendSettingValue(SettingValueChangeEvent e)
    {
        if(e != null)
        {
            _settingData = e.Data;

            switch (_settingData.Option)
            {
                case SettingOption.BGM_Slider:
                    JAudioManager.Instance.SetBGMVolume(_settingData.BGM_Slider_Value);
                    break;

                case SettingOption.SFX_Slider:
                    JAudioManager.Instance.SetSFXVolume(_settingData.SFX_Slider_Value);
                    break;

                case SettingOption.BGM_Toggle:
                    JAudioManager.Instance.ToggleBGM(_settingData.BGM_Toggle_Value);
                    break;

                case SettingOption.SFX_Toggle:
                    JAudioManager.Instance.ToggleSFX(_settingData.SFX_Toggle_Value);
                    break;

                case SettingOption.Language_Dropdown:
                    switch (_settingData.LanguageIndex)
                    {
                        case 0:
                            CurrentLanguage = "KR";
                            _settingData.LanguageIndex = 0;
                            JEventBus.SendEvent(new LanguageChangeEvent());
                            break;
                        case 1:
                            CurrentLanguage = "EN";
                            _settingData.LanguageIndex = 1;
                            JEventBus.SendEvent(new LanguageChangeEvent());
                            break;
                        case 2:
                            CurrentLanguage = "JP";
                            _settingData.LanguageIndex = 2;
                            JEventBus.SendEvent(new LanguageChangeEvent());
                            break;
                        case 3:
                            CurrentLanguage = "CN";
                            _settingData.LanguageIndex = 3;
                            JEventBus.SendEvent(new LanguageChangeEvent());
                            break;
                    }
                    break;
            }
        }
        else
        {
            JAudioManager.Instance.SetBGMVolume(_settingData.BGM_Slider_Value);
            JAudioManager.Instance.SetSFXVolume(_settingData.SFX_Slider_Value);
            JAudioManager.Instance.ToggleBGM(_settingData.BGM_Toggle_Value);
            JAudioManager.Instance.ToggleSFX(_settingData.SFX_Toggle_Value);

            switch (_settingData.LanguageIndex)
            {
                case 0:
                    CurrentLanguage = "KR";
                    _settingData.LanguageIndex = 0;
                    JEventBus.SendEvent(new LanguageChangeEvent());
                    break;
                case 1:
                    CurrentLanguage = "EN";
                    _settingData.LanguageIndex = 1;
                    JEventBus.SendEvent(new LanguageChangeEvent());
                    break;
                case 2:
                    CurrentLanguage = "JP";
                    _settingData.LanguageIndex = 2;
                    JEventBus.SendEvent(new LanguageChangeEvent());
                    break;
                case 3:
                    CurrentLanguage = "CN";
                    _settingData.LanguageIndex = 3;
                    JEventBus.SendEvent(new LanguageChangeEvent());
                    break;
            }
        }
    }

    private void SaveSetting(SaveButtonClickEvent e)
    {
        SettingDaraWrapper wrapper = new SettingDaraWrapper();
        wrapper.Items.Add(_settingData);

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(JPathManager.JsonFilePath("Setting"), json);
    }
    #endregion
}


