using UnityEngine;

public class JSpawnManager : MonoBehaviour
{
    // 여기서 유닛들을 생성함

    #region Singleton
    private static JSpawnManager instance;
    public  static JSpawnManager Instance { get { return instance; } }

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
                go.AddComponent<JSpawnManager>();
            }

            DontDestroyOnLoad(go);
        }
    }
    #endregion

    // 인스펙터에서 바인딩 하려고 어레이로 만들었음
    public JUnitFactory[] UnitFactories;

    private void Awake()
    {
        SingletonInitialize();
    }

    public void Spawn()
    {
        // 각 번호마다 각각의 유닛 공장
        // 거기서 가져옴
        // UnitFactories[0].GetProduct();
        // UnitFactories[0].GetProduct();
        // UnitFactories[0].GetProduct();
        // UnitFactories[0].GetProduct();
    }

}
