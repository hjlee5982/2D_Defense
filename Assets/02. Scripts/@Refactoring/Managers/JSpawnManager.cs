using UnityEngine;

public class JSpawnManager : MonoBehaviour
{
    // ���⼭ ���ֵ��� ������

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

    // �ν����Ϳ��� ���ε� �Ϸ��� ��̷� �������
    public JUnitFactory[] UnitFactories;

    private void Awake()
    {
        SingletonInitialize();
    }

    public void Spawn()
    {
        // �� ��ȣ���� ������ ���� ����
        // �ű⼭ ������
        // UnitFactories[0].GetProduct();
        // UnitFactories[0].GetProduct();
        // UnitFactories[0].GetProduct();
        // UnitFactories[0].GetProduct();
    }

}
