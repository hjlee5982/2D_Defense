using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDic();
}


#region UNIT_DATA
[Serializable]
public class AllyUnitData
{
    public int    Index;

    public string UnitID;
    public string UnitPrefabID;

    public string NameKR;
    public string NameEN;
    public string NameJP;
    public string NameCN;

    public int    AtkPower;
    public int    AtkRange;
    public int    AtkSpeed;
    public int    UpgradeCount;
    
    public int    Cost;

    [NonSerialized]
    public AllyUnit UnitPrefab;

    [NonSerialized]
    public List<string> UnitNames;

    [NonSerialized]
    public int Grade;
    public int dAtkPower;
    public int dAtkRange;
    public int dAtkSpeed;
    public int dUpgradeCount;
    public int PaybackGold;

    public AllyUnitData Clone()
    {
        // AllyUnitData clone = Instantiate(this)
        // 이건 Monobehaviour를 상속받고 있어야 함

        string json = JsonUtility.ToJson(this);
        return JsonUtility.FromJson<AllyUnitData>(json);
    }

    public string GetName(string currentLanguage)
    {
        switch (currentLanguage)
        {
            case "KR":
                return NameKR;
            case "EN":
                return NameEN;
            case "JP":
                return NameJP;
            case "CN":
                return NameCN;
        }
        return "ID_NOT_FOUND";
    }
}

[Serializable]
public class AllyUnitDataLoader : ILoader<int, AllyUnitData>
{
    public List<AllyUnitData> Items = new List<AllyUnitData>();

    public Dictionary<int, AllyUnitData> MakeDic()
    {
        Dictionary<int, AllyUnitData> dic = new Dictionary<int, AllyUnitData>();

        foreach(AllyUnitData data in Items)
        {
            dic.Add(data.Index, data);
        }

        return dic;
    }
}
#endregion



#region MONSTER_DATA
[Serializable]
public class MonsterUnitData
{
    public string UnitName;
    public string UnitPrefabName;
    public int    MaxHealth;
    public int    Health;
    public int    MoveSpeed;

    [NonSerialized]
    public MonsterUnit UnitPrefab;

    public MonsterUnitData Clone()
    {
        string json = JsonUtility.ToJson(this);
        return JsonUtility.FromJson<MonsterUnitData>(json);
    }
}

[Serializable]
public class MonsterUnitDataLoader : ILoader<string, MonsterUnitData>
{
    public List<MonsterUnitData> Items = new List<MonsterUnitData>();

    public Dictionary<string, MonsterUnitData> MakeDic()
    {
        Dictionary<string, MonsterUnitData> dic = new Dictionary<string, MonsterUnitData>();

        foreach (MonsterUnitData data in Items)
        {
            dic.Add(data.UnitName, data);
        }

        return dic;
    }
}
#endregion



#region STAGE_DATA
[Serializable]
public class StageData
{
    public int    Index;
    public int    DropGold;
    public int    NumOfMonster;
    public string SpawnMonster;
    public float  MonsterSpawnInterval;
    public int    dHealth;
    public int    MoveSpeed;
    public bool   IsBossStage;
}

[Serializable]
public class StageDataLoader : ILoader<int, StageData>
{
    public List<StageData> Items = new List<StageData>();

    public Dictionary<int, StageData> MakeDic()
    {
        Dictionary<int, StageData> dic = new Dictionary<int, StageData>();

        foreach (StageData data in Items)
        {
            dic.Add(data.Index, data);
        }

        return dic;
    }
}
#endregion



#region GAME_RULE_DATA
[Serializable]
public class GameRuleData
{
    public int Index;
    public int LifeLimit;
    public int InitialGold;
}

[Serializable]
public class GameRuleDataLoader : ILoader<int, GameRuleData>
{
    public List<GameRuleData> Items = new List<GameRuleData>();

    public Dictionary<int, GameRuleData> MakeDic()
    {
        Dictionary<int, GameRuleData> dic = new Dictionary<int, GameRuleData>();

        foreach (GameRuleData data in Items)
        {
            dic.Add(data.Index, data);
        }

        return dic;
    }
}
#endregion



#region GAME_RULE_DATA
[Serializable]
public class RouteData
{
    public int    Index;
    public string Route;
}

[Serializable]
public class RouteDataLoader : ILoader<int, RouteData>
{
    public List<RouteData> Items = new List<RouteData>();

    public Dictionary<int, RouteData> MakeDic()
    {
        Dictionary<int, RouteData> dic = new Dictionary<int, RouteData>();

        foreach (RouteData data in Items)
        {
            dic.Add(data.Index, data);
        }

        return dic;
    }
}
#endregion



#region GAME_RULE_DATA
[Serializable]
public class EnhancementData
{
    public int Index;
    public int Probability;
    public int Cost;
    public int dAtkPower;
    public int dAtkRange;
    public int dAtkSpeed;
    public bool isRandom;
    public string RandomWeight;
}

[Serializable]
public class EnhancementDataLoader : ILoader<int, EnhancementData>
{
    public List<EnhancementData> Items = new List<EnhancementData>();

    public Dictionary<int, EnhancementData> MakeDic()
    {
        Dictionary<int, EnhancementData> dic = new Dictionary<int, EnhancementData>();

        foreach (EnhancementData data in Items)
        {
            dic.Add(data.Index, data);
        }

        return dic;
    }
}
#endregion



#region LOCALIZER
[Serializable]
public class LocalizeData
{
    public string ID;

    public string KR;
    public string EN;
    public string JP;
    public string CN;
}

[Serializable]
public class LocalizeDataLoader : ILoader<string, LocalizeData>
{
    public List<LocalizeData> Items = new List<LocalizeData>();

    public Dictionary<string, LocalizeData> MakeDic()
    {
        Dictionary<string, LocalizeData> dic = new Dictionary<string, LocalizeData>();

        foreach (LocalizeData data in Items)
        {
            dic.Add(data.ID, data);
        }

        return dic;
    }
}
#endregion