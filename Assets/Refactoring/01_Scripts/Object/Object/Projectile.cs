using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region VARIABLES
    [Header("조준 목표")]
    private MonsterUnit _targetMonster;
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
        if(_targetMonster != null)
        {
            Throwing();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion





    #region FUNCTIONS
    public void SetTarget(MonsterUnit targetMonster)
    {
        _targetMonster = targetMonster;
    }

    public MonsterUnit GetTarget()
    {
        return _targetMonster;
    }

    private void Throwing()
    {
        if( _targetMonster == null )
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = _targetMonster.transform.position;

        Vector3 dir = (targetPosition - transform.position).normalized;

        transform.position += dir * Time.deltaTime * JGameManager.Instance.ProjectileSpeed;
    }
    #endregion
}
