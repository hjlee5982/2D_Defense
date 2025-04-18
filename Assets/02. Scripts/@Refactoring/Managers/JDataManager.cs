using UnityEngine;

public class JDataManager : MonoBehaviour
{
    #region Singleton
    private static JDataManager instance;
    public  static JDataManager Instance { get { return instance; } }

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
                go.AddComponent<JGameManager>();
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
