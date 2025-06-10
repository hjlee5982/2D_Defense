using System.Collections.Generic;
using UnityEngine;

public class JEffectManager : MonoBehaviour
{
    #region SINGLETON
    public static JEffectManager Instance { get; private set; }

    private bool SingletonInitialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }
    #endregion





    #region VARIABLES
    [Header("¿Ã∆Â∆Æ «¡∏Æ∆È")]
    public List<GameObject> EffectPrefabs = new List<GameObject>();

    [Header("¿Ã∆Â∆Æ «¡∏Æ∆È µÒº≈≥ ∏Æ")]
    private Dictionary<string, GameObject> _effectPrefabDict = new Dictionary<string, GameObject>();
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        if (SingletonInitialize() == false)
        {
            return;
        }

        foreach(GameObject effectPrefab in EffectPrefabs)
        {
            if (_effectPrefabDict.ContainsKey(effectPrefab.name) == false)
            {
                _effectPrefabDict.Add(effectPrefab.name, effectPrefab);
            }
        }
    }
    #endregion





    #region FUNTIONS
    public GameObject GetEffect(string name)
    {
        return _effectPrefabDict[name];
    }
    #endregion
}