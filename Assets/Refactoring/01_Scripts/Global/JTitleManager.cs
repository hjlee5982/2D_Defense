using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JTitleManager : MonoBehaviour
{
    #region VARIABLES
    [Header("타이틀 이미지")]
    private Image _titleImage;

    [Header("타이틀 스프라이트")]
    public Sprite TitleSpriteKR;
    public Sprite TitleSpriteEN;
    public Sprite TitleSpriteJP;
    public Sprite TitleSpriteCN;

    [Header("버튼 텍스트")]
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
        Debug.Log("시작버튼 클릭");

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
        Debug.Log("종료버튼 클릭");

        EventSystem.current.SetSelectedGameObject(null);

        Application.Quit();
    }

    private void LanguageChange(LanguageChangeEvent e)
    {
        switch(JSettingManager.Instance.CurrentLanguage)
        {
            case "KR":
                _titleImage.sprite = TitleSpriteKR;
                break;
            case "EN":
                _titleImage.sprite = TitleSpriteEN;
                break;
            case "JP":
                _titleImage.sprite = TitleSpriteJP;
                break;
            case "CN":
                _titleImage.sprite = TitleSpriteCN;
                break;
        }

        ID_Start_Button_Scene.text = JSettingManager.Instance.GetText(ID_Start_Button_Scene.name);
        ID_Setting_Button.text     = JSettingManager.Instance.GetText(ID_Setting_Button.name);
        ID_Exit_Button.text        = JSettingManager.Instance.GetText(ID_Exit_Button.name);
    }
    #endregion
}
