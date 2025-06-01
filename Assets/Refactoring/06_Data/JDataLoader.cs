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
    public Dictionary<int,    AllyUnitData   > AllyUnitData    { get; private set; } = new Dictionary<int,    AllyUnitData   >();
    public Dictionary<string, MonsterUnitData> MonsterUnitData { get; private set; } = new Dictionary<string, MonsterUnitData>();
    public Dictionary<int,    StageData      > StageData       { get; private set; } = new Dictionary<int,    StageData      >();
    public Dictionary<int,    GameRuleData   > GameRuleData    { get; private set; } = new Dictionary<int,    GameRuleData   >();
    public Dictionary<int,    RouteData      > RouteData       { get; private set; } = new Dictionary<int,    RouteData      >();
    public Dictionary<int,    EnhancementData> EnhancementData { get; private set; } = new Dictionary<int,    EnhancementData>();
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
            AllyUnitData    = LoadJson<AllyUnitDataLoader,    int,    AllyUnitData   >("AllyUnitData"   ).MakeDic();
            MonsterUnitData = LoadJson<MonsterUnitDataLoader, string, MonsterUnitData>("MonsterUnitData").MakeDic();
            StageData       = LoadJson<StageDataLoader,       int,    StageData      >("StageData"      ).MakeDic();
            GameRuleData    = LoadJson<GameRuleDataLoader,    int,    GameRuleData   >("GameRuleData"   ).MakeDic();
            RouteData       = LoadJson<RouteDataLoader,       int,    RouteData      >("RouteData"      ).MakeDic();
            EnhancementData = LoadJson<EnhancementDataLoader, int,    EnhancementData>("EnhancementData").MakeDic();
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
