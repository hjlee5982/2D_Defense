using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region VARIABLES
    [Header("조준 목표")]
    private MonsterUnit _targetMonster = null;

    [Header("충돌 플래그")]
    private bool _isHit = false;

    [Header("공격력")]
    private int _atkPoint = 0;
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
    public void SetTarget(MonsterUnit targetMonster, int atkPoint)
    {
        _targetMonster = targetMonster;
        _atkPoint = atkPoint;
    }

    public MonsterUnit GetTarget()
    {
        return _targetMonster;
    }

    public void MarkHit()
    {
        _isHit = true;
    }

    public bool IsHit()
    {
        return _isHit;
    }

    public int GetDamage()
    {
        return _atkPoint;
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
