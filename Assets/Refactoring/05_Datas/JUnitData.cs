using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class JUnitData : ScriptableObject
{
    [Header("������")]
    public JUnit UnitPrefab;

    [Space(20)]
    [Header("������")]
    public string UnitName;
    public Sprite Thumbnail;
    public int    Grade;

    [Space(20)]
    [Header("�������ͽ�")]
    public int AtkPower;
    public int AtkRange;
    public int AtkSpeed;
    public int UpgradeCount;

    public JUnitData Clone()
    {
        JUnitData clone = Instantiate(this);

        return clone;
    }
}
