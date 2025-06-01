using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JTitleManager : MonoBehaviour
{
    #region VARIABLES
    [Header("버튼")]
    public Button StartButton;
    public Button SettingButton;
    public Button ExitButton;
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        StartButton  .onClick.AddListener(() => { StartButtonEvent();   JAudioManager.Instance.PlaySFX("ButtonClick"); });
        SettingButton.onClick.AddListener(() => { SettingButtonEvent(); JAudioManager.Instance.PlaySFX("ButtonClick"); });
        ExitButton   .onClick.AddListener(() => { ExitButtonEvent();    JAudioManager.Instance.PlaySFX("ButtonClick"); });
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
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
        JEventBus.SendEvent(new SettingMenuActivationEvent());

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ExitButtonEvent()
    {
        Debug.Log("종료버튼 클릭");

        EventSystem.current.SetSelectedGameObject(null);

        Application.Quit();
    }
    #endregion
}
