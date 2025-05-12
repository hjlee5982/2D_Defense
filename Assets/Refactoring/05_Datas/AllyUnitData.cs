using UnityEngine;

[CreateAssetMenu(fileName = "AllyUnitData", menuName = "Scriptable Objects/AllyUnitData")]
public class AllyUnitData : ScriptableObject
{
    [Header("프리펩")]
    public AllyUnit UnitPrefab;

    [Space(20)]
    [Header("투사체")]
    public Projectile Projectile;

    [Header("프로필")]
    public string UnitName;
    public Sprite Thumbnail;
    public int    Grade;

    [Space(20)]
    [Header("스테이터스")]
    public int AtkPower;
    public int AtkRange;
    public int AtkSpeed;
    public int UpgradeCount;

    [Space(20)]
    [Header("스테이터스 증감률")]
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
