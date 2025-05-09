using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    AtkPower,
    AtkRange,
    AtkSpeed,

    Grade,
    UpgradeCount,
}


public class JUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("�ִϸ�����")]
    protected Animator _animator;

    [Header("���� ���� �ݶ��̴�")]
    private CircleCollider2D _attackRange;

    [Header("���� ������")]
    public JUnitData UnitData;
    #endregion

    [Header("���� ������")]
    public int dAtkPower = 0;
    public int dAtkRange = 0;
    public int dAtkSpeed = 0;

    [Header("��ȭ �����")]
    private Dictionary<StatType, Action<int>> _statApplier;

    

    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    protected virtual void Awake()
    {
        _animator    = transform.GetComponent<Animator>();
        _attackRange = transform.GetComponent<CircleCollider2D>();

        _statApplier = new Dictionary<StatType, Action<int>>
        {
            {
                StatType.AtkPower, value =>
                {
                    UnitData.AtkPower += value;
                    dAtkPower += value;
                }
            },
            {
                StatType.AtkRange, value =>
                {
                    UnitData.AtkRange += value;
                    dAtkRange += value;
                    ModifyAttackRange();
                }
            },
            {
                StatType.AtkSpeed, value =>
                {
                    UnitData.AtkSpeed += value;
                    dAtkSpeed += value;
                }
            },
            { 
                StatType.Grade,        value => 
                {
                    UnitData.Grade        += value; 
                } 
            },
            { 
                StatType.UpgradeCount, value => 
                {
                    UnitData.UpgradeCount += value; 
                } 
            },
        };
    }

    protected virtual void Start()
    {
        ModifyAttackRange();
    }

    protected virtual void Update()
    {
    }
    #endregion





    #region FUNCTIONS
    public void SetInitialData(JUnitData unitData)
    {
        UnitData = unitData.Clone();
    }

    public void ApplyStatChange(StatType statType, int value)
    {
        if(_statApplier.ContainsKey(statType) == true)
        {
            _statApplier[statType](value);
        }
    }

    public bool IsEnhancable()
    {
        return UnitData.UpgradeCount > 0;
    }

    private void ModifyAttackRange()
    {
        _attackRange.radius = Mathf.Clamp(0.3f * UnitData.AtkRange + 1.5f, 1.8f, 5.7f);
    }
    #endregion
}
