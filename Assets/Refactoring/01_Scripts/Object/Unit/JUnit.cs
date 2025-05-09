using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    AtkPower,
    AtkRange,
    AtkSpeed,

    UpgradeCount,
}


public class JUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("애니메이터")]
    protected Animator _animator;

    [Header("유닛 데이터")]
    protected JUnitData UnitData { get; set; }
    #endregion

    [Header("강화 적용기")]
    private Dictionary<StatType, Action<int>> _statApplier;


    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    protected virtual void Awake()
    {
        _animator = transform.GetComponent<Animator>();

        _statApplier = new Dictionary<StatType, Action<int>>
        {
            { StatType.AtkPower, value => UnitData.AtkPower += value},
            { StatType.AtkRange, value => UnitData.AtkRange += value},
            { StatType.AtkSpeed, value => UnitData.AtkSpeed += value},

            { StatType.UpgradeCount, value => UnitData.UpgradeCount += value},
        }; 
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
        UnitData = unitData.Clone();
    }

    public JUnitData GetUnitData()
    {
        return UnitData;
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
    #endregion
}
