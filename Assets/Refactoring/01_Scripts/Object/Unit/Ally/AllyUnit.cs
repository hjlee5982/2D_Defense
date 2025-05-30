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

    [Header("인디케이터")]
    private GameObject _indicator;

    [Header("유닛 데이터")]
    private AllyUnitData _allyUnitData;

    [Header("투사체")]
    public Projectile Projectile;

    [Header("공격 대상 몬스터 리스트")]
    protected List<MonsterUnit> _monsterList = new List<MonsterUnit>();

    [Header("공격 코루틴")]
    private Coroutine _attackCoroutine;

    [Header("공격 범위 콜라이더")]
    private CircleCollider2D _attackRange;

    [Header("강화 액션")]
    private Dictionary<StatType, Action<int>> _statApplier;

    [Header("공격 간격")]
    private float _atkInterval;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    protected virtual void Awake()
    {
        _animator    = transform.GetComponent<Animator>();
        _attackRange = transform.GetComponent<CircleCollider2D>();

        _indicator = transform.Find("Indicator").gameObject;
        _indicator.SetActive(false);

        _statApplier = new Dictionary<StatType, Action<int>>
        {
            {
                StatType.AtkPower, value =>
                {
                    _allyUnitData.AtkPower += value;
                    _allyUnitData.dAtkPower += value;
                }
            },
            {
                StatType.AtkRange, value =>
                {
                    _allyUnitData.AtkRange += value;
                    _allyUnitData.dAtkRange += value;
                    ModifyAttackRange();
                }
            },
            {
                StatType.AtkSpeed, value =>
                {
                    _allyUnitData.AtkSpeed += value;
                    _allyUnitData.dAtkSpeed += value;
                }
            },
            {
                StatType.Grade, value =>
                {
                    _allyUnitData.Grade += value;
                }
            },
            {
                StatType.UpgradeCount, value =>
                {
                    _allyUnitData.UpgradeCount += value;
                }
            },
        };
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        //if(_monsterList.Count > 0 && _attackCoroutine == null)
        //{
        //    _attackCoroutine = StartCoroutine(Attack());
        //}
        //else if(_monsterList.Count == 0 && _attackCoroutine != null) 
        //{
        //    StopCoroutine(_attackCoroutine);
        //    _attackCoroutine = null;
        //}

        _atkInterval -= Time.deltaTime;

        if(_monsterList.Count > 0 && _atkInterval <= 0f)
        {
            SetThrowAnimation(_monsterList[0].transform.position);

            Projectile projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.SetTarget(_monsterList[0], _allyUnitData.AtkPower, _allyUnitData.AtkSpeed + 10);

            _atkInterval = Mathf.Clamp(-0.1f * _allyUnitData.AtkSpeed + 1.3f, 0.1f, 1.9f);
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
    public AllyUnitData GetUnitData()
    {
        return _allyUnitData;
    }

    public void SetInitialData(AllyUnitData allyUnitData)
    {
        _allyUnitData = allyUnitData.Clone();

        _atkInterval = allyUnitData.AtkSpeed;
    }

    public void ToggleIndicator(bool value)
    {
        _indicator.SetActive(value);
    }

    public bool IsEnhancable()
    {
        return _allyUnitData.UpgradeCount > 0;
    }

    public void NotifyMonsterDied(MonsterUnit monsterUnit)
    {
        _monsterList.Remove(monsterUnit);
    }

    private void ModifyAttackRange()
    {
        _attackRange.radius = Mathf.Clamp(0.3f * _allyUnitData.AtkRange + 1.5f, 1.8f, 5.7f);
    }

    //private IEnumerator Attack()
    //{
    //    _monsterList.RemoveAll(t => t == null);

    //    while(_monsterList.Count > 0)
    //    {
    //        yield return new WaitForSeconds(Mathf.Clamp(-0.1f * _allyUnitData.AtkSpeed + 1.3f, 0.1f, 1.9f));

    //        _monsterList.RemoveAll(t => t == null);
    //    }
    //}

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
