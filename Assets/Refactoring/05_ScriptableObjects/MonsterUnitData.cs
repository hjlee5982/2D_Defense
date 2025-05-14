using UnityEngine;

[CreateAssetMenu(fileName = "MonsterUnitData", menuName = "Scriptable Objects/MonsterUnitData")]
public class MonsterUnitData : ScriptableObject
{
    [Header("프리펩")]
    public MonsterUnit UnitPrefab;

    [Space(20)]
    [Header("프로필")]
    public string UnitName;
    public Sprite Thumbnail;

    [Space(20)]
    [Header("스테이터스")]
    public int Health;
    public int MoveSpeed;

    [Space(20)]
    [Header("스테이터스 증감률")]
    public int dHealth;

    public MonsterUnitData Clone()
    {
        MonsterUnitData clone = Instantiate(this);

        Thumbnail = UnitPrefab.transform.GetComponent<SpriteRenderer>().sprite;

        return clone;
    }
}
