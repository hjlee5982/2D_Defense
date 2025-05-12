using UnityEngine;

[CreateAssetMenu(fileName = "MonsterUnitData", menuName = "Scriptable Objects/MonsterUnitData")]
public class MonsterUnitData : ScriptableObject
{
    [Header("������")]
    public MonsterUnit UnitPrefab;

    [Space(20)]
    [Header("������")]
    public string UnitName;
    public Sprite Thumbnail;

    [Space(20)]
    [Header("�������ͽ�")]
    public int Health;
    public int MoveSpeed;

    [Space(20)]
    [Header("�������ͽ� ������")]
    public int dHealth;

    public MonsterUnitData Clone()
    {
        MonsterUnitData clone = Instantiate(this);

        Thumbnail = UnitPrefab.transform.GetComponent<SpriteRenderer>().sprite;

        return clone;
    }
}
