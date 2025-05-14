using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDic();
}

[Serializable]
public class JAllyUnitData
{
    public int Index;

    public string UnitName;

    public string UnitPrefabPath;
    public string ProjectilePrefabPath;

    public int Grade;

    public int AtkPower;
    public int AtkRange;
    public int AtkSpeed;
    public int UpgradeCount;

    public int dAtkPower;
    public int dAtkRange;
    public int dAtkSpeed;
    public int dUpgradeCount;
}

[Serializable]
public class JAllyUnitDataLoader : ILoader<int, JAllyUnitData>
{
    public List<JAllyUnitData> Items = new List<JAllyUnitData>();

    public Dictionary<int, JAllyUnitData> MakeDic()
    {
        Dictionary<int, JAllyUnitData> dic = new Dictionary<int, JAllyUnitData>();

        foreach(JAllyUnitData data in Items)
        {
            dic.Add(data.Index, data);
        }

        return dic;
    }
}
