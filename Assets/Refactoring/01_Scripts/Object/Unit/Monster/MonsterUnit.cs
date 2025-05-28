using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameStatusChangeEvent;
using static MonsterStateChangeEvent;

public class MonsterUnit : MonoBehaviour
{
    #region ENUM
    enum MonsterBehaviourState
    {
        None,
        Alive,
        Death,
        Finish
    }
    enum MonsterDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    #endregion

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

    [Header("���� �÷���")]
    private MonsterBehaviourState _currentState = MonsterBehaviourState.None;

    [Header("���� �÷���")]
    private MonsterDirection _currentDirection = MonsterDirection.None;

    [Header("���� �÷���")]
    private bool _isDeath = false;

    [Header("HP ������")]
    private Image _hpGauge;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    protected virtual void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        _hpGauge  = transform.Find("UI_HPGauge").GetChild(0).GetChild(0).GetComponent<Image>();

        _currentState = MonsterBehaviourState.Alive;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        switch (_currentState)
        {
            case MonsterBehaviourState.Alive:

                MoveProcess();

                break;

            case MonsterBehaviourState.Death:

                DieProcess();

                break;
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = collision.GetComponent<Projectile>();

            if(projectile != null && projectile.GetTarget() == this && projectile.IsHit() == false)
            {
                // �ݶ��̴� ������ Ÿ�� ����� bool���� �߰�����
                projectile.MarkHit();

                UnderAttack(projectile);
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

            _realPosition = _startPoint;
        }
    }

    private void SetDirection(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * (180f / Mathf.PI);
        angle = Mathf.Round(angle);

        if(88 <= angle && angle <= 92)
        {
            _currentDirection = MonsterDirection.Up;
        }
        else if (-92 <= angle && angle <= -88)
        {
            _currentDirection = MonsterDirection.Down;
        }
        else if(178 <= angle && angle <= 182)
        {
            _currentDirection = MonsterDirection.Left;
        }
        else if(-2 <= angle && angle <= 2)
        {
            _currentDirection = MonsterDirection.Right;
        }
    }

    private void SetAnimation(string trigger)
    {
        switch (_currentDirection)
        {
            case MonsterDirection.Up:
                _animator.SetTrigger(trigger + "_Up");
                break;
            case MonsterDirection.Down:
                _animator.SetTrigger(trigger + "_Down");
                break;
            case MonsterDirection.Left:
                _animator.SetTrigger(trigger + "_Left");
                break;
            case MonsterDirection.Right:
                _animator.SetTrigger(trigger + "_Right");
                break;
        }
    }

    private void MoveProcess()
    {
        // �̵� ��ΰ� ������ �������� ����
        if (RouteQueue == null || RouteQueue?.Count == 0)
        {
            // �����ϸ� ����� ����

            JEventBus.SendEvent(new MonsterStateChangeEvent(MonsterStateType.Finish));

            Destroy(gameObject);

            return;
        }


        // �ٶ󺸴� ���� ����
        Vector3 targetPoint = RouteQueue.Peek();

        Vector3 dir = (targetPoint - _realPosition).normalized;

        SetDirection(dir);


        // �̵� �ִϸ��̼� ���
        SetAnimation("Move");


        // ������ȯ
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


        // ���� ���� ����
        transform.position = _realPosition + new Vector3(0, OffsetY, 0);
    }

    private void UnderAttack(Projectile projectile)
    {
        int damage = projectile.GetDamage();

        // ���� ����ü ����
        Destroy(projectile.gameObject);

        // ����� ���
        if(DamageProcess(damage) == true)
        {
            _currentState = MonsterBehaviourState.Death;
        }
    }

    private bool DamageProcess(int damage)
    {
        _monsterUnitData.Health -= damage;

        // HP ������ ������Ʈ
        _hpGauge.fillAmount = (float)_monsterUnitData.Health / (float)_monsterUnitData.MaxHealth;
        _hpGauge.color = Color.HSVToRGB(_hpGauge.fillAmount / 3, 1.0f, 1.0f);


        if (_monsterUnitData.Health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DieProcess()
    {
        foreach(AllyUnit unit in _registeredUnits)
        {
            unit.NotifyMonsterDied(this);
        }

        // �״� �ִϸ��̼��� �� 1���� Ʈ���Ű� ���� ��
        if(_isDeath == false)
        {
            SetAnimation("Die");
            JEventBus.SendEvent(new MonsterStateChangeEvent(MonsterStateType.Die));
            _isDeath = true;
        }
        // �״� �ִϸ��̼��� ������ ����
        if(_isDeath == true)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if(stateInfo.IsName("Die_Up") || stateInfo.IsName("Die_Down") || stateInfo.IsName("Die_Left") || stateInfo.IsName("Die_Right"))
            {
                if(stateInfo.normalizedTime >= 1.0f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    // �ڽ�(����)�� ���� ������� ���� ���ֵ��� ������
    // => �ڽ�(����)�� �׾ �����Ǹ� ���� ������ �迭�� null�� �����ϰ� �Ǵϱ�
    // => DieProcess���� �ڽ�(����)�� �׾��ٰ� �������� �˷��ְ�
    // => nulló���� ������ �˾Ƽ� ��
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
