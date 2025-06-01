using UnityEngine;

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





    #region MONOBEHAVIOUR
    private void Awake()
    {
        SingletonInitialize();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        JEventBus.Subscribe<SettingMenuActivationEvent>(OepnSettingMenu);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<SettingMenuActivationEvent>(OepnSettingMenu);
    }
    #endregion





    #region FUNCTION
    private void OepnSettingMenu(SettingMenuActivationEvent e)
    {
        Debug.Log("¼³Á¤Ã¢ ¿ÀÇÂ");
    }
    #endregion
}
