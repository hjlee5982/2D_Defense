using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JMonster : MonoBehaviour
{
    #region VARIABLES
    [Header("경로 정보")]
    protected Queue<Vector3> RouteQueue;

    [Header("생성 지점")]
    protected Vector3 _startPoint;

    [Header("애니메이터")]
    private Animator _animator;
    #endregion





    #region OVERRIDES
    protected virtual void Move()
    {
        if(RouteQueue?.Count == 0)
        {
            return;
        }

        Vector3 targetPoint = RouteQueue.Peek();

        Vector3 dir = (targetPoint - transform.position).normalized;

        SetLookAnimation(dir);

        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            transform.position = targetPoint;
            RouteQueue.Dequeue();
        }
        else
        {
            dir.Normalize();
            transform.position += dir * Time.deltaTime * JGameManager.Instance.MonsterSpeed;
        }
    }
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    #endregion





    #region FUNCTIONS
    private void SetLookAnimation(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * (180f / Mathf.PI);
        angle = Mathf.Round(angle);

        if(88 <= angle && angle <= 92)
        {
            _animator.SetTrigger("Up");
            Debug.Log("상");
        }
        else if (-92 <= angle && angle <= -88)
        {
            _animator.SetTrigger("Down");
            Debug.Log("하");
        }
        else if(178 <= angle && angle <= 182)
        {
            _animator.SetTrigger("Left");
            Debug.Log("좌");
        }
        else if(-2 <= angle && angle <= 2)
        {
            _animator.SetTrigger("Right");
            Debug.Log("우");
        }
    }

    public void SetRouteData(Queue<Vector3> routeQueue)
    {
        RouteQueue = new Queue<Vector3>(routeQueue);

        _startPoint = RouteQueue.Dequeue();
    }
    #endregion
}
