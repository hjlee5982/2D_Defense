using UnityEngine;

public class JUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("�ִϸ�����")]
    protected Animator _animator;

    [Header("���� ������")]
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
            Debug.Log("�̸� : " + UnitData.UnitName);
            Debug.Log("��� : " + UnitData.Grade);
            Debug.Log("���ݷ� : " + UnitData.AtkPower);
            Debug.Log("���ݹ���: " + UnitData.AtkRange);
            Debug.Log("���ݼӵ� : " + UnitData.AtkSpeed);
        }

    }

    public JUnitData GetUnitData()
    {
        return UnitData;
    }
    #endregion
}
