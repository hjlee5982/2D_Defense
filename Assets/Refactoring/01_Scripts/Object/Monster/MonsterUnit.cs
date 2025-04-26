using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("경로 정보")]
    protected Queue<Vector3> RouteQueue;

    [Header("생성 지점")]
    protected Vector3 _startPoint;

    [Header("애니메이터")]
    protected Animator _animator;

    [Header("위치 보정")]
    protected Vector3 _realPosition;
    public float OffsetY = 2f;
    #endregion





    #region OVERRIDES
    protected virtual void Move()
    {
        if(RouteQueue == null || RouteQueue?.Count == 0)
        {
            return;
        }

        Vector3 targetPoint = RouteQueue.Peek();

        Vector3 dir = (targetPoint - _realPosition).normalized;

        SetWalkAnimation(dir);

        if (Vector3.Distance(_realPosition, targetPoint) < 0.01f)
        {
            _realPosition = targetPoint;
            RouteQueue.Dequeue();
        }
        else
        {
            dir.Normalize();
            _realPosition += dir * Time.deltaTime * JGameManager.Instance.MonsterSpeed;
        }

        transform.position = _realPosition + new Vector3(0, OffsetY, 0);
    }
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        _animator = transform.GetComponent<Animator>();
    }
    #endregion





    #region FUNCTIONS
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

    public void SetRouteData(Queue<Vector3> routeQueue)
    {
        RouteQueue = new Queue<Vector3>(routeQueue);

        _startPoint = RouteQueue.Dequeue();

        _realPosition = _startPoint - new Vector3(0, OffsetY, 0);
    }
    #endregion
}
