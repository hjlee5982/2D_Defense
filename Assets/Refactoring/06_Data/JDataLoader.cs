using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class JDataLoader : MonoBehaviour
{
    #region VARIABLES
    // Addressable
    public Dictionary<string, GameObject> PrefabData = new Dictionary<string, GameObject>();

    // JSON -> DATA
    public Dictionary<int, JAllyUnitData>    AllyUnitData    { get; private set; } = new Dictionary<int, JAllyUnitData>();
    public Dictionary<int, JMonsterUnitData> MonsterUnitData { get; private set; } = new Dictionary<int, JMonsterUnitData>();
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        // Addressable
        {
            var prefabsLoadOperationHandle = Addressables.LoadAssetsAsync<GameObject>("Prefab" /*어드레서블 라벨*/, prefab => PrefabData[prefab.name] = prefab);
            prefabsLoadOperationHandle.WaitForCompletion();
        }
        // JSON
        {
            AllyUnitData    = LoadJson<JAllyUnitDataLoader,    int, JAllyUnitData   >("AllyUnitData"   ).MakeDic();
            MonsterUnitData = LoadJson<JMonsterUnitDataLoader, int, JMonsterUnitData>("MonsterUnitData").MakeDic();
        }
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
