using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JDataLoader : MonoBehaviour
{
    #region VARIABLES
    // JSON -> DATA
    public Dictionary<int, JAllyUnitData> AllyUnitData { get; private set; } = new Dictionary<int, JAllyUnitData>();
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        AllyUnitData = LoadJson<JAllyUnitDataLoader, int, JAllyUnitData>("AllyUnitData").MakeDic();
    }
    #endregion





    #region FUNCTIONS
    private T LoadJson<T, Key, Value>(string fileName) where T : ILoader<Key, Value>
    {
        string fullPath = JPathManager.JsonFilePath(fileName);

        string json = File.ReadAllText(fullPath);

        return JsonConvert.DeserializeObject<T>(json);
    }
    #endregion
}
