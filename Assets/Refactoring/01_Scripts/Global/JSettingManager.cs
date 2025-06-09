using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static SettingValueChangeEvent;

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

    [Header("설정 UI 초기값")]
    public SettingValue SettingValue = new SettingValue();
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        if(!SingletonInitialize())
        {
            return; ;
        }

        LocalizeDataProcess();

        {
            SettingValue.BGM_Slider_Value = 0.5f;
            SettingValue.BGM_Toggle_Value = false;
            SettingValue.SFX_Slider_Value = 0.5f;
            SettingValue.SFX_Toggle_Value = false;
            SettingValue.LanguageIndex    = 0;
        }
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<SettingValueChangeEvent>(GetSettingValue);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<SettingValueChangeEvent>(GetSettingValue);
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

    private void GetSettingValue(SettingValueChangeEvent e)
    {
        SettingValue = e.Values;

        switch(SettingValue.Option)
        {
            case SettingOption.BGM_Slider:
                JAudioManager.Instance.SetBGMVolume(e.Values.BGM_Slider_Value);
                break;

            case SettingOption.SFX_Slider:
                JAudioManager.Instance.SetSFXVolume(e.Values.SFX_Slider_Value);
                break;

            case SettingOption.BGM_Toggle:
                JAudioManager.Instance.ToggleBGM   (e.Values.BGM_Toggle_Value);
                break;

            case SettingOption.SFX_Toggle:
                JAudioManager.Instance.ToggleSFX   (e.Values.SFX_Toggle_Value);
                break;

            case SettingOption.Language_Dropdown:
                switch (e.Values.LanguageIndex)
                {
                    case 0:
                        CurrentLanguage = "KR";
                        JEventBus.SendEvent(new LanguageChangeEvent());
                        break;
                    case 1:
                        CurrentLanguage = "EN";
                        JEventBus.SendEvent(new LanguageChangeEvent());
                        break;
                    case 2:
                        CurrentLanguage = "JP";
                        JEventBus.SendEvent(new LanguageChangeEvent());
                        break;
                    case 3:
                        CurrentLanguage = "CN";
                        JEventBus.SendEvent(new LanguageChangeEvent());
                        break;
                }
                break;
        }
    }
    #endregion
}