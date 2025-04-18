using UnityEngine;

public class JRouteManager : MonoBehaviour
{
    // �ʿ��� ��� ������ ������

    #region Singleton
    private static JRouteManager instance;
    public  static JRouteManager Instance { get { return instance; } }

    private void SingletonInitialize()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<JRouteManager>();
            }

            DontDestroyOnLoad(go);
        }
    }
    #endregion



    private void Awake()
    {
        SingletonInitialize();
    }
}
