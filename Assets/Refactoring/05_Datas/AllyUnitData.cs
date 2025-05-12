using UnityEngine;

[CreateAssetMenu(fileName = "AllyUnitData", menuName = "Scriptable Objects/AllyUnitData")]
public class AllyUnitData : ScriptableObject
{
    [Header("������")]
    public AllyUnit UnitPrefab;

    [Space(20)]
    [Header("����ü")]
    public Projectile Projectile;

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

    [Space(20)]
    [Header("�������ͽ� ������")]
    public int dAtkPower;
    public int dAtkRange;
    public int dAtkSpeed;
    public int dUpgradeCount;

    public AllyUnitData Clone()
    {
        AllyUnitData clone = Instantiate(this);

        Thumbnail = UnitPrefab.transform.GetComponent<SpriteRenderer>().sprite;

        return clone;
    }
}
