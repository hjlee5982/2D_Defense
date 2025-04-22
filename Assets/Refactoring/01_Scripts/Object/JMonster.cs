using System.Collections.Generic;
using UnityEngine;

public class JMonster : MonoBehaviour
{
    #region VARIABLES
    [Header("경로 정보")]
    protected Queue<Vector3> RouteQueue;
    #endregion





    #region OVERRIDES
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
    }
    #endregion
}
