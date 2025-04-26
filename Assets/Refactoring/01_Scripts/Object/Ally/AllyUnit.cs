using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("애니메이터")]
    private Animator _animator;

    [Header("몬스터 큐")]
    private List<MonsterUnit> _monsterList = new List<MonsterUnit>();

    [Header("공격 범위")]
    public float AttackRange = 5f;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            MonsterUnit monsterUnit = collision.GetComponent<MonsterUnit>();

            if(_monsterList.Contains(monsterUnit) == false)
            {
                _monsterList.Add(monsterUnit);
                Debug.Log("공격리스트에 " + monsterUnit.name + " 이(가) 들어옴");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            MonsterUnit monsterUnit = collision.GetComponent<MonsterUnit>();

            if (_monsterList.Contains(monsterUnit) == true)
            {
                _monsterList.Remove(monsterUnit);
                Debug.Log("공격리스트에서 " + monsterUnit.name + " 이(가) 삭제됨");
            }
        }
    }
    #endregion





    #region FUNCTIONS
    #endregion
}
