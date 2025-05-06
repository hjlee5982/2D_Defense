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
    public float AtkRange;
    public float AtkSpeed;
    public float AtkPower;
}
