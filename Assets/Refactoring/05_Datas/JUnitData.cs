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
    public float AtkPower;
    public float AtkRange;
    public float AtkSpeed;
    public float UpgradeCount;

    public JUnitData Clone()
    {
        JUnitData clone = Instantiate(this);

        return clone;
    }
}
