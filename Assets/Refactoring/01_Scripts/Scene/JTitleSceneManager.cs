using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JTitleSceneManager : MonoBehaviour
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
    private TextMeshProUGUI ID_Start_Button_Title;
    private TextMeshProUGUI ID_Setting_Button;
    private TextMeshProUGUI ID_Exit_Button;

    [Header("마우스 이미지")]
    public Texture2D CursorTexture;
    public Vector2 HotSpot = Vector2.zero;
    public CursorMode CursorMode = CursorMode.Auto;
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        Time.timeScale = 1f;

        Cursor.SetCursor(CursorTexture, HotSpot, CursorMode);


        Button btn;

        btn = transform.Find("StartButton").GetComponent<Button>();
        btn.onClick.AddListener(StartButtonEvent);
        ID_Start_Button_Title = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        btn = transform.Find("SettingButton").GetComponent<Button>();
        btn.onClick.AddListener(SettingButtonEvent);
        ID_Setting_Button = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        btn = transform.Find("ExitButton").GetComponent<Button>();
        btn.onClick.AddListener(ExitButtonEvent);
        ID_Exit_Button = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        _titleImage = transform.Find("TitleImage").GetComponent<Image>();


        
    }

    private void Start()
    {
        GameObject padeEffectObj = Instantiate(JEffectManager.Instance.GetEffect("Pade"), Vector3.zero, Quaternion.identity);
        padeEffectObj.transform.SetParent(GameObject.Find("UI").transform, false);
        Animator padeOut = padeEffectObj.GetComponent<Animator>();
        padeOut.SetTrigger("PadeIn");
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
        EventSystem.current.SetSelectedGameObject(null);
        JAudioManager.Instance.PlaySFX("ButtonClick");

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("NewLoadingScene");
        loadOperation.allowSceneActivation = false;

        GameObject padeEffectObj = Instantiate(JEffectManager.Instance.GetEffect("Pade"), Vector3.zero, Quaternion.identity);
        padeEffectObj.transform.SetParent(GameObject.Find("UI").transform, false);
        Animator padeOut = padeEffectObj.GetComponent<Animator>();
        padeOut.SetTrigger("PadeOut");

        StartCoroutine(WaitForSceneReady(loadOperation));
    }

    private IEnumerator WaitForSceneReady(AsyncOperation operation)
    {
        while(operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1);
        operation.allowSceneActivation = true;
    }

    private void SettingButtonEvent()
    {
        EventSystem.current.SetSelectedGameObject(null);
        JAudioManager.Instance.PlaySFX("ButtonClick");

        JEventBus.SendEvent(new OpenSettingPanelEvent());
    }

    private void ExitButtonEvent()
    {
        EventSystem.current.SetSelectedGameObject(null);
        JAudioManager.Instance.PlaySFX("ButtonClick");

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

        ID_Start_Button_Title.text = JSettingManager.Instance.GetText(ID_Start_Button_Title.name);
        ID_Setting_Button.text     = JSettingManager.Instance.GetText(ID_Setting_Button.name);
        ID_Exit_Button.text        = JSettingManager.Instance.GetText(ID_Exit_Button.name);
    }
    #endregion
}
