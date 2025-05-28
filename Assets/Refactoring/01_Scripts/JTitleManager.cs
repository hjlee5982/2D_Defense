using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JTitleManager : MonoBehaviour
{
    #region VARIABLES
    [Header("��ư")]
    public Button StartButton;
    public Button SettingButton;
    public Button ExitButton;
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        StartButton  .onClick.AddListener(StartButtonEvent);
        SettingButton.onClick.AddListener(SettingButtonEvent);
        ExitButton   .onClick.AddListener(ExitButtonEvent);
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
        Debug.Log("���۹�ư Ŭ��");

        SceneManager.LoadSceneAsync("NewGameScene");
    }

    private void SettingButtonEvent()
    {
        JEventBus.SendEvent(new SettingMenuActivationEvent());
    }

    private void ExitButtonEvent()
    {
        Debug.Log("�����ư Ŭ��");
        
        Application.Quit();
    }
    #endregion
}
