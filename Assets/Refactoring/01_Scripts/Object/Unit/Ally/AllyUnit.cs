using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public enum StatType
{
    AtkPower,
    AtkRange,
    AtkSpeed,

    Grade,
    UpgradeCount,
}

public class AllyUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("애니메이터")]
    protected Animator _animator;

    [Header("유닛 데이터")]
    public AllyUnitData AllyUnitData;

    [Header("공격 대상 몬스터 리스트")]
    protected List<MonsterUnit> _monsterList = new List<MonsterUnit>();

    [Header("공격 코루틴")]
    private Coroutine _attackCoroutine;

    [Header("공격 범위 콜라이더")]
    private CircleCollider2D _attackRange;

    [Header("강화 액션")]
    private Dictionary<StatType, Action<int>> _statApplier;

    #endregion





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
                    AllyUnitData.AtkPower += value;
                    AllyUnitData.dAtkPower += value;
                }
            },
            {
                StatType.AtkRange, value =>
                {
                    AllyUnitData.AtkRange += value;
                    AllyUnitData.dAtkRange += value;
                    ModifyAttackRange();
                }
            },
            {
                StatType.AtkSpeed, value =>
                {
                    AllyUnitData.AtkSpeed += value;
                    AllyUnitData.dAtkSpeed += value;
                }
            },
            {
                StatType.Grade, value =>
                {
                    AllyUnitData.Grade += value;
                }
            },
            {
                StatType.UpgradeCount, value =>
                {
                    AllyUnitData.UpgradeCount += value;
                }
            },
        };
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if(_monsterList.Count > 0 && _attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(Attack());
        }
        else if(_monsterList.Count == 0 && _attackCoroutine != null) 
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            MonsterUnit monsterUnit = collision.GetComponent<MonsterUnit>();

            if(_monsterList.Contains(monsterUnit) == false)
            {
                _monsterList.Add(monsterUnit);
                monsterUnit.RegisterAllyUnit(this);
            }
        }   
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            MonsterUnit monsterUnit = collision.GetComponent<MonsterUnit>();

            if (_monsterList.Contains(monsterUnit) == true)
            {
                _monsterList.Remove(monsterUnit);
                monsterUnit.UnregisterAllyUnit(this);
            }
        }
    }
    #endregion





    #region FUNCTIONS
    public void SetInitialData(AllyUnitData allyUnitData)
    {
        AllyUnitData = allyUnitData.Clone();
    }

    public bool IsEnhancable()
    {
        return AllyUnitData.UpgradeCount > 0;
    }

    public void NotifyMonsterDied(MonsterUnit monsterUnit)
    {
        _monsterList.Remove(monsterUnit);
    }

    private void ModifyAttackRange()
    {
        _attackRange.radius = Mathf.Clamp(0.3f * AllyUnitData.AtkRange + 1.5f, 1.8f, 5.7f);
    }

    private IEnumerator Attack()
    {
        _monsterList.RemoveAll(t => t == null);

        while(_monsterList.Count > 0)
        {
            SetThrowAnimation(_monsterList[0].transform.position);

            Projectile projectile = Instantiate(AllyUnitData.Projectile, transform.position, Quaternion.identity);
            projectile.SetTarget(_monsterList[0], 2);

            yield return new WaitForSeconds(Mathf.Clamp(-0.1f * AllyUnitData.AtkSpeed + 1.3f, 0.1f, 1.9f));

            _monsterList.RemoveAll(t => t == null);
        }
    }

    public void ApplyStatChange(StatType statType, int value)
    {
        if (_statApplier.ContainsKey(statType) == true)
        {
            _statApplier[statType](value);
        }
    }

    private void SetThrowAnimation(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * (180f / Mathf.PI);
        angle = Mathf.Round(angle);

        if (45f < angle && angle <= 135f)
        {
            _animator.SetTrigger("Attack_Up");
        }
        else if (-135f <= angle && angle < -45f)
        {
            _animator.SetTrigger("Attack_Down");
        }
        else if (135 <= angle || angle < -135f)
        {
            _animator.SetTrigger("Attack_Left");
        }
        else if (-45f <= angle && angle <= 45f)
        {
            _animator.SetTrigger("Attack_Right");
        }
    }
    #endregion
}
