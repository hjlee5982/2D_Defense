using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JMonster : MonoBehaviour
{
    #region VARIABLES
    [Header("��� ����")]
    protected Queue<Vector3> RouteQueue;

    [Header("���� ����")]
    protected Vector3 _startPoint;

    [Header("�ִϸ�����")]
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
            Debug.Log("��");
        }
        else if (-92 <= angle && angle <= -88)
        {
            _animator.SetTrigger("Down");
            Debug.Log("��");
        }
        else if(178 <= angle && angle <= 182)
        {
            _animator.SetTrigger("Left");
            Debug.Log("��");
        }
        else if(-2 <= angle && angle <= 2)
        {
            _animator.SetTrigger("Right");
            Debug.Log("��");
        }
    }

    public void SetRouteData(Queue<Vector3> routeQueue)
    {
        RouteQueue = new Queue<Vector3>(routeQueue);

        _startPoint = RouteQueue.Dequeue();
    }
    #endregion
}
