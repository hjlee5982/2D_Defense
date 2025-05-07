using UnityEngine;

public class JUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("애니메이터")]
    protected Animator _animator;

    [Header("유닛 데이터")]
    protected JUnitData UnitData;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    protected virtual void Awake()
    {
        _animator = transform.GetComponent<Animator>();
    }

    void Start()
    {
    }

    protected virtual void Update()
    {
    }
    #endregion





    #region FUNCTIONS
    public void SetInitialData(JUnitData unitData)
    {
        UnitData = unitData;
        {
            Debug.Log("이름 : " + UnitData.UnitName);
            Debug.Log("등급 : " + UnitData.Grade);
            Debug.Log("공격력 : " + UnitData.AtkPower);
            Debug.Log("공격범위: " + UnitData.AtkRange);
            Debug.Log("공격속도 : " + UnitData.AtkSpeed);
        }

    }

    public JUnitData GetUnitData()
    {
        return UnitData;
    }
    #endregion
}
