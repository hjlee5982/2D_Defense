using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("�ִϸ�����")]
    private Animator _animator;

    [Header("����ü")]
    public Projectile Projectile;

    [Header("���� ��� ���� ����Ʈ")]
    protected List<MonsterUnit> _monsterList = new List<MonsterUnit>();

    [Header("���� ����")]
    public float AttackRange = 5f;

    [Header("���� �ڷ�ƾ")]
    private Coroutine _attackCoroutine;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
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
    public void NotifyMonsterDied(MonsterUnit monsterUnit)
    {
        _monsterList.Remove(monsterUnit);
    }

    IEnumerator Attack()
    {
        _monsterList.RemoveAll(t => t == null);

        while(_monsterList.Count > 0)
        {
            Projectile projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.SetTarget(_monsterList[0]);

            yield return new WaitForSeconds(1f);

            _monsterList.RemoveAll(t => t == null);
        }
    }
    #endregion
}
