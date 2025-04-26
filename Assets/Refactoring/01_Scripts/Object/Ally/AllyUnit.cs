using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : MonoBehaviour
{
    #region VARIABLES
    [Header("�ִϸ�����")]
    private Animator _animator;

    [Header("���� ť")]
    private List<MonsterUnit> _monsterList = new List<MonsterUnit>();

    [Header("���� ����")]
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
                Debug.Log("���ݸ���Ʈ�� " + monsterUnit.name + " ��(��) ����");
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
                Debug.Log("���ݸ���Ʈ���� " + monsterUnit.name + " ��(��) ������");
            }
        }
    }
    #endregion





    #region FUNCTIONS
    #endregion
}
