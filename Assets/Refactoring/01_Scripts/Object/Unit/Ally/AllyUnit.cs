using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : JUnit
{
    #region VARIABLES
    [Header("투사체")]
    public Projectile Projectile;

    [Header("공격 대상 몬스터 리스트")]
    protected List<MonsterUnit> _monsterList = new List<MonsterUnit>();

    

    [Header("공격 코루틴")]
    private Coroutine _attackCoroutine;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
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
    public void NotifyMonsterDied(MonsterUnit monsterUnit)
    {
        _monsterList.Remove(monsterUnit);
    }

    private IEnumerator Attack()
    {
        _monsterList.RemoveAll(t => t == null);

        while(_monsterList.Count > 0)
        {
            SetThrowAnimation(_monsterList[0].transform.position);

            Projectile projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.SetTarget(_monsterList[0]);

            yield return new WaitForSeconds(Mathf.Clamp(-0.1f * UnitData.AtkSpeed + 1.3f, 0.1f, 1.9f));

            _monsterList.RemoveAll(t => t == null);
        }
    }

    protected void MakeProjectile()
    {
        if(_monsterList.Count > 0)
        {
            _monsterList.RemoveAll(t => t == null);

            Projectile projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.SetTarget(_monsterList[0]);
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
