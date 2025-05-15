using System;
using System.Collections.Generic;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDic();
}


#region UNIT_DATA
[Serializable]
public class JAllyUnitData
{
    public int Index;

    public string UnitName;

    public string UnitPrefabPath;

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
#endregion



#region MONSTER_DATA
[Serializable]
public class JMonsterUnitData
{
    public int Index;

    public string UnitName;

    public string UnitPrefabPath;

    public int Health;
    public int MoveSpeed;

    public int dHealth;
}

[Serializable]
public class JMonsterUnitDataLoader : ILoader<int, JMonsterUnitData>
{
    public List<JMonsterUnitData> Items = new List<JMonsterUnitData>();

    public Dictionary<int, JMonsterUnitData> MakeDic()
    {
        Dictionary<int, JMonsterUnitData> dic = new Dictionary<int, JMonsterUnitData>();

        foreach (JMonsterUnitData data in Items)
        {
            dic.Add(data.Index, data);
        }

        return dic;
    }
}
#endregion