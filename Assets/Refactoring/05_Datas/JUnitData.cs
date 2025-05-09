using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class JUnitData : ScriptableObject
{
    [Header("프리펩")]
    public JUnit UnitPrefab;

    [Space(20)]
    [Header("프로필")]
    public string UnitName;
    public Sprite Thumbnail;
    public int    Grade;

    [Space(20)]
    [Header("스테이터스")]
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
