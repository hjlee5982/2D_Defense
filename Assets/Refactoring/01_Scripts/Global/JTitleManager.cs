using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JTitleManager : MonoBehaviour
{
    #region VARIABLES
    [Header("Ÿ��Ʋ ��������Ʈ")]
    private Image _titleImage;

    [Header("��ư �ؽ�Ʈ")]
    private TextMeshProUGUI ID_Start_Button_Scene;
    private TextMeshProUGUI ID_Setting_Button;
    private TextMeshProUGUI ID_Exit_Button;
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        Button btn;

        btn = transform.Find("StartButton").GetComponent<Button>();
        btn.onClick.AddListener(() => { StartButtonEvent(); JAudioManager.Instance.PlaySFX("ButtonClick"); });
        ID_Start_Button_Scene = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        btn = transform.Find("SettingButton").GetComponent<Button>();
        btn.onClick.AddListener(() => { SettingButtonEvent(); JAudioManager.Instance.PlaySFX("ButtonClick"); });
        ID_Setting_Button = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        btn = transform.Find("ExitButton").GetComponent<Button>();
        btn.onClick.AddListener(() => { ExitButtonEvent(); JAudioManager.Instance.PlaySFX("ButtonClick"); });
        ID_Exit_Button = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        _titleImage = transform.Find("TitleImage").GetComponent<Image>();
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<LanguageChangeEvent>(LanguageChange);

        LanguageChange(null);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<LanguageChangeEvent>(LanguageChange);
    }
    #endregion





    #region FUNCTION
    private void StartButtonEvent()
    {
        Debug.Log("���۹�ư Ŭ��");

        SceneManager.LoadSceneAsync("NewGameScene");

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void SettingButtonEvent()
    {
        JSettingManager.Instance.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ExitButtonEvent()
    {
        Debug.Log("�����ư Ŭ��");

        EventSystem.current.SetSelectedGameObject(null);

        Application.Quit();
    }

    private void SetLanguage()
    {
        // TODO : ID_Start, ID_Setting, ID_Exit
        //_startButtonText.text;
        //_settingButtonText.text;
        //_exitButtonText.text;
    }


    private void LanguageChange(LanguageChangeEvent e)
    {
        ID_Start_Button_Scene.text = JSettingManager.Instance.GetText(ID_Start_Button_Scene.name);
        ID_Setting_Button.text    = JSettingManager.Instance.GetText(ID_Setting_Button.name);
        ID_Exit_Button.text       = JSettingManager.Instance.GetText(ID_Exit_Button.name);
    }
    #endregion
}
