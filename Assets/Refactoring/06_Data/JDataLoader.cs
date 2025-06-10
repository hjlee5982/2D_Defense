using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class JDataLoader : MonoBehaviour
{
    #region SINGLETON
    public static JDataLoader Instance { get; private set; }

    private void SingletonInitialize()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region VARIABLES
    // Addressable
    public Dictionary<string, GameObject> PrefabData = new Dictionary<string, GameObject>();
    public Dictionary<string, TextAsset > JsonData   = new Dictionary<string, TextAsset >();

    // JSON -> DATA
    public Dictionary<int,    AllyUnitData   > AllyUnitData    { get; private set; } = new Dictionary<int,    AllyUnitData   >();
    public Dictionary<string, MonsterUnitData> MonsterUnitData { get; private set; } = new Dictionary<string, MonsterUnitData>();
    public Dictionary<int,    StageData      > StageData       { get; private set; } = new Dictionary<int,    StageData      >();
    public Dictionary<int,    GameRuleData   > GameRuleData    { get; private set; } = new Dictionary<int,    GameRuleData   >();
    public Dictionary<int,    RouteData      > RouteData       { get; private set; } = new Dictionary<int,    RouteData      >();
    public Dictionary<int,    EnhancementData> EnhancementData { get; private set; } = new Dictionary<int,    EnhancementData>();
    public Dictionary<string, LocalizeData   > LocalizeData    { get; private set; } = new Dictionary<string, LocalizeData   >();
    public Dictionary<int,    SettingData    > SettingData     { get; private set; } = new Dictionary<int,    SettingData    >();
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        SingletonInitialize();

        // Addressable
        {
            var prefabsLoadOperationHandle = Addressables.LoadAssetsAsync<GameObject>("Prefab", prefab => PrefabData[prefab.name] = prefab);
            prefabsLoadOperationHandle.WaitForCompletion();
            
            var jsonLoadOperationHanel = Addressables.LoadAssetsAsync<TextAsset>("Json", json => JsonData[json.name] = json );
            jsonLoadOperationHanel.WaitForCompletion();
        }
        // JSON
        {
            // AllyUnitData    = LoadJson<AllyUnitDataLoader,    int,    AllyUnitData   >("AllyUnitData"   ).MakeDic();
            // MonsterUnitData = LoadJson<MonsterUnitDataLoader, string, MonsterUnitData>("MonsterUnitData").MakeDic();
            // StageData       = LoadJson<StageDataLoader,       int,    StageData      >("StageData"      ).MakeDic();
            // GameRuleData    = LoadJson<GameRuleDataLoader,    int,    GameRuleData   >("GameRuleData"   ).MakeDic();
            // RouteData       = LoadJson<RouteDataLoader,       int,    RouteData      >("RouteData"      ).MakeDic();
            // EnhancementData = LoadJson<EnhancementDataLoader, int,    EnhancementData>("EnhancementData").MakeDic();
            // SettingData     = LoadJson<SettingDataLoader,     int,    SettingData    >("Setting"        ).MakeDic();
            // LocalizeData    = LoadJson<LocalizeDataLoader,    string, LocalizeData   >("Localizer"      ).MakeDic();

            AllyUnitData    = LoadJson<AllyUnitDataLoader,    int,    AllyUnitData   >(JsonData["AllyUnitData"].text).MakeDic();
            MonsterUnitData = LoadJson<MonsterUnitDataLoader, string, MonsterUnitData>(JsonData["MonsterUnitData"].text).MakeDic();
            StageData       = LoadJson<StageDataLoader,       int,    StageData      >(JsonData["StageData"].text).MakeDic();
            GameRuleData    = LoadJson<GameRuleDataLoader,    int,    GameRuleData   >(JsonData["GameRuleData"].text).MakeDic();
            RouteData       = LoadJson<RouteDataLoader,       int,    RouteData      >(JsonData["RouteData"].text).MakeDic();
            EnhancementData = LoadJson<EnhancementDataLoader, int,    EnhancementData>(JsonData["EnhancementData"].text).MakeDic();
            SettingData     = LoadJson<SettingDataLoader,     int,    SettingData    >(JsonData["Setting"].text).MakeDic();
            LocalizeData    = LoadJson<LocalizeDataLoader,    string, LocalizeData   >(JsonData["Localizer"].text).MakeDic();
        }
    }
    #endregion





    #region FUNCTIONS
    private T LoadJson<T, Key, Value>(string fileName) where T : ILoader<Key, Value>
    {
        // string fullPath = JPathManager.JsonFilePath(fileName);

        // string json = File.ReadAllText(fullPath);

        return JsonConvert.DeserializeObject<T>(fileName);
    }
    #endregion
}
