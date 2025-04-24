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
    #endregion





    #region OVERRIDES
    protected virtual void Move()
    {
        if(RouteQueue?.Count == 0)
        {
            return;
        }

        Vector3 targetPoint = RouteQueue.Peek();

        Vector3 dir = targetPoint - transform.position;

        Flip(dir);

        if(dir.magnitude <= 0.1f)
        {
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
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    #endregion





    #region FUNCTIONS
    public void SetRouteData(Queue<Vector3> routeQueue)
    {
        RouteQueue = new Queue<Vector3>(routeQueue);

        _startPoint = RouteQueue.Dequeue();
    }

    private void Flip(Vector3 dir)
    {
        Vector3 localScale = transform.localScale;

        if(dir.x >-0.1f)
        {
            transform.localScale = new Vector3(-1.0f, localScale.y, localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, localScale.y, localScale.z);
        }
    }
    #endregion
}
