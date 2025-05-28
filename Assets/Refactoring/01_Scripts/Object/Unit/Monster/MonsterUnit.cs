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
    [Header("애니메이터")]
    protected Animator _animator;

    [Header("몬스터 데이터")]
    private MonsterUnitData _monsterUnitData;

    [Header("경로 정보")]
    protected Queue<Vector3> RouteQueue;

    [Header("생성 지점")]
    protected Vector3 _startPoint;

    [Header("위치 보정")]
    protected Vector3 _realPosition;
    public float OffsetY = 2f;

    [Header("자신을 등록한 유닛 리스트")]
    private List<AllyUnit> _registeredUnits = new List<AllyUnit>();

    [Header("상태 플래그")]
    private MonsterBehaviourState _currentState = MonsterBehaviourState.None;

    [Header("방향 플래그")]
    private MonsterDirection _currentDirection = MonsterDirection.None;

    [Header("죽음 플래그")]
    private bool _isDeath = false;

    [Header("HP 게이지")]
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
                // 콜라이더 여러번 타서 막기용 bool변수 추가했음
                projectile.MarkHit();

                UnderAttack(projectile);
            }
        }
    }
    #endregion





    #region FUNCTIONS
    public void SetInitialData(MonsterUnitData monsterUnitData, Queue<Vector3> routeQueue)
    {
        // 초기 데이터 설정
        {
            _monsterUnitData = monsterUnitData.Clone();
        }
        // 가야 할 경로 설정
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
        // 이동 경로가 없으면 움직이지 않음
        if (RouteQueue == null || RouteQueue?.Count == 0)
        {
            // 도착하면 여기로 들어옴

            JEventBus.SendEvent(new MonsterStateChangeEvent(MonsterStateType.Finish));

            Destroy(gameObject);

            return;
        }


        // 바라보는 방향 설정
        Vector3 targetPoint = RouteQueue.Peek();

        Vector3 dir = (targetPoint - _realPosition).normalized;

        SetDirection(dir);


        // 이동 애니메이션 재생
        SetAnimation("Move");


        // 방향전환
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


        // 실제 변위 변경
        transform.position = _realPosition + new Vector3(0, OffsetY, 0);
    }

    private void UnderAttack(Projectile projectile)
    {
        int damage = projectile.GetDamage();

        // 맞은 투사체 지움
        Destroy(projectile.gameObject);

        // 대미지 계산
        if(DamageProcess(damage) == true)
        {
            _currentState = MonsterBehaviourState.Death;
        }
    }

    private bool DamageProcess(int damage)
    {
        _monsterUnitData.Health -= damage;

        // HP 게이지 업데이트
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

        // 죽는 애니메이션은 단 1번만 트리거가 들어가야 함
        if(_isDeath == false)
        {
            SetAnimation("Die");
            JEventBus.SendEvent(new MonsterStateChangeEvent(MonsterStateType.Die));
            _isDeath = true;
        }
        // 죽는 애니메이션이 끝나면 삭제
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

    // 자신(몬스터)을 공격 대상으로 삼은 유닛들을 가져옴
    // => 자신(몬스터)가 죽어서 삭제되면 위의 유닛의 배열에 null이 존재하게 되니까
    // => DieProcess에서 자신(몬스터)가 죽었다고 유닛한테 알려주고
    // => null처리는 유닛이 알아서 함
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
