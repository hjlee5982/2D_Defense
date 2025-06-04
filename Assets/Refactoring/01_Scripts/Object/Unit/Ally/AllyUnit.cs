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

    [Header("공격 범위 시각화")]
    private Transform _atkRangeView;

    [Header("외곽선 효과")]
    private SpriteRenderer _outline;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    protected virtual void Awake()
    {
        _animator     = transform.GetComponent<Animator>();
        _attackRange  = transform.GetComponent<CircleCollider2D>();

        _indicator = transform.Find("Indicator").gameObject;
        _indicator.SetActive(false);

        _atkRangeView = transform.Find("AtkRange");
        _atkRangeView.gameObject.SetActive(false);

        _outline = transform.GetComponent<SpriteRenderer>();

        _statApplier = new Dictionary<StatType, Action<int>>
        {
            {
                StatType.AtkPower, value =>
                {
                    _allyUnitData.AtkPower += CulcValue(_allyUnitData.AtkPower, value);
                    _allyUnitData.dAtkPower += CulcValue(_allyUnitData.dAtkPower, value);
                }
            },
            {
                StatType.AtkRange, value =>
                {
                    _allyUnitData.AtkRange += CulcValue(_allyUnitData.AtkRange, value);
                    _allyUnitData.dAtkRange += CulcValue(_allyUnitData.dAtkRange, value);
                }
            },
            {
                StatType.AtkSpeed, value =>
                {
                    _allyUnitData.AtkSpeed += CulcValue(_allyUnitData.AtkSpeed, value);
                    _allyUnitData.dAtkSpeed += CulcValue(_allyUnitData.dAtkSpeed, value);
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
        ModifyAttackRange();
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

        AttackProcess();
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

    public void ToggleIndicator(bool flag)
    {
        _indicator.SetActive(flag);

        _atkRangeView.gameObject.SetActive(flag);
    }

    public bool IsEnhancable()
    {
        return _allyUnitData.UpgradeCount > 0;
    }

    public void NotifyMonsterDied(MonsterUnit monsterUnit)
    {
        _monsterList.Remove(monsterUnit);
    }

    public void ApplyStatChange(StatType statType, int value)
    {
        if (_statApplier.ContainsKey(statType) == true)
        {
            _statApplier[statType](value);
        }

        ModifyAttackRange();
        ToggleOutline();
    }

    private void AttackProcess()
    {
        _atkInterval -= Time.deltaTime;

        if (_monsterList.Count > 0 && _atkInterval <= 0f)
        {
            SetThrowAnimation(_monsterList[0].transform.position);

            Projectile projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.SetTarget(_monsterList[0], _allyUnitData.AtkPower, _allyUnitData.AtkSpeed + 10);

            JAudioManager.Instance.PlaySFX("Throw");

            _atkInterval = Mathf.Clamp(-0.1f * _allyUnitData.AtkSpeed + 1.3f, 0.1f, 1.9f);
        }
    }

    private int CulcValue(int value, int delta)
    {
        int dValue = value + delta;

        if(dValue <= 0)
        {
            return -(value - 1);
        }
        else
        {
            return delta;
        }
    }

    private void ToggleOutline()
    {
        _outline.material.SetFloat("_IsOutline", 1f);
        _outline.material.SetFloat("_Thickness", 1.3f);

        int statDelta = _allyUnitData.dAtkPower + _allyUnitData.dAtkRange + _allyUnitData.dAtkSpeed;

        Color32 outlineColor;

        //if(statDelta == 1)
        //{
        //    outlineColor = new Color32(100, 202, 224, 255);
        //}
        //else if(statDelta == 2)
        //{
        //    outlineColor = new Color32(157, 94, 223, 255);
        //}
        //else if(statDelta == 3)
        //{
        //    outlineColor = new Color32(222, 0, 222, 255);
        //}
        //else if(statDelta == 4)
        //{
        //    outlineColor = new Color32(212, 100, 0, 255);
        //}
        //else if(statDelta == 5)
        //{
        //    outlineColor = new Color32(236, 167, 0, 255);
        //}
        //else
        //{
        //    outlineColor = new Color32(88, 233, 151, 255);
        //}

        if (statDelta <= 7)
        {
            outlineColor = new Color32(255, 165, 29, 255);
        }
        else if (statDelta <= 13)
        {
            outlineColor = new Color32(41, 77, 255, 255);
        }
        else if (statDelta <= 20)
        {
            outlineColor = new Color32(164, 28, 255, 255);
        }
        else if (statDelta <= 27)
        {
            outlineColor = new Color32(240, 240, 63, 255);
        }
        else if (statDelta <= 36)
        {
            outlineColor = new Color32(0, 157, 0, 255);
        }
        else
        {
            outlineColor = new Color32(255, 0, 0, 255);
        }

        _outline.material.SetColor("_Color", outlineColor);
    }

    private void ModifyAttackRange()
    {
        _attackRange.radius = Mathf.Clamp(0.3f * _allyUnitData.AtkRange + 1.5f, 1.8f, 5.7f);
        _atkRangeView.localScale = new Vector3(2f, 2f, 2f) * _attackRange.radius;
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
