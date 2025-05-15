using System.Collections.Generic;
using UnityEngine;

public class MonsterUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("�ִϸ�����")]
    protected Animator _animator;

    [Header("���� ������")]
    private MonsterUnitData _monsterUnitData;

    [Header("��� ����")]
    protected Queue<Vector3> RouteQueue;

    [Header("���� ����")]
    protected Vector3 _startPoint;

    [Header("��ġ ����")]
    protected Vector3 _realPosition;
    public float OffsetY = 2f;

    [Header("�ڽ��� ����� ���� ����Ʈ")]
    private List<AllyUnit> _registeredUnits = new List<AllyUnit>();
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    protected virtual void Awake()
    {
        _animator = transform.GetComponent<Animator>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = collision.GetComponent<Projectile>();

            if(projectile != null && projectile.GetTarget() == this && projectile.IsHit() == false)
            {
                projectile.MarkHit();
                TakeDamage(projectile);
            }
        }
    }
    #endregion





    #region FUNCTIONS
    public void SetInitialData(MonsterUnitData monsterUnitData, Queue<Vector3> routeQueue)
    {
        // �ʱ� ������ ����
        {
            _monsterUnitData = monsterUnitData.Clone();
        }
        // ���� �� ��� ����
        {
            RouteQueue = new Queue<Vector3>(routeQueue);

            _startPoint = RouteQueue.Dequeue();

            _realPosition = _startPoint - new Vector3(0, OffsetY, 0);
        }
    }

    private void SetWalkAnimation(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * (180f / Mathf.PI);
        angle = Mathf.Round(angle);

        if(88 <= angle && angle <= 92)
        {
            _animator.SetTrigger("Walk_Up");
        }
        else if (-92 <= angle && angle <= -88)
        {
            _animator.SetTrigger("Walk_Down");
        }
        else if(178 <= angle && angle <= 182)
        {
            _animator.SetTrigger("Walk_Left");
        }
        else if(-2 <= angle && angle <= 2)
        {
            _animator.SetTrigger("Walk_Right");
        }
    }

    private void SetDieAnimation(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * (180f / Mathf.PI);
        angle = Mathf.Round(angle);

        if (88 <= angle && angle <= 92)
        {
            _animator.SetTrigger("Die_Up");
        }
        else if (-92 <= angle && angle <= -88)
        {
            _animator.SetTrigger("Die_Down");
        }
        else if (178 <= angle && angle <= 182)
        {
            _animator.SetTrigger("Die_Left");
        }
        else if (-2 <= angle && angle <= 2)
        {
            _animator.SetTrigger("Die_Right");
        }
    }

    private void Move()
    {
        if (RouteQueue == null || RouteQueue?.Count == 0)
        {
            return;
        }

        Vector3 targetPoint = RouteQueue.Peek();

        Vector3 dir = (targetPoint - _realPosition).normalized;

        SetWalkAnimation(dir);

        if (Vector3.Distance(_realPosition, targetPoint) < 0.1f)
        {
            _realPosition = targetPoint;
            RouteQueue.Dequeue();
        }
        else
        {
            dir.Normalize();
            _realPosition += dir * Time.deltaTime * _monsterUnitData.MoveSpeed;
        }

        transform.position = _realPosition + new Vector3(0, OffsetY, 0);
    }

    protected void TakeDamage(Projectile projectile)
    {
        int damage = projectile.GetDamage();

        // ���� ����ü ����
        Destroy(projectile.gameObject);


        //Die();
    }

    protected void Die()
    {
        foreach(AllyUnit unit in _registeredUnits)
        {
            unit.NotifyMonsterDied(this);
        }

        // TODO

        Destroy(gameObject);
    }

    public void RegisterAllyUnit(AllyUnit allyUnit)
    {
        if(_registeredUnits.Contains(allyUnit) == false)
        {
            _registeredUnits.Add(allyUnit);
        }
    }

    public void UnregisterAllyUnit(AllyUnit allyUnit)
    {
        _registeredUnits.Remove(allyUnit);
    }
    #endregion
}
