using UnityEngine;

public class JMage : MonoBehaviour, IUnitProduct
{
    #region INTERFACE
    string unitName = "Warrior";
    public string UnitName
    {
        get
        {
            return unitName;
        }
        set
        {
            unitName = value;
        }
    }

    public void Initialize()
    {
        // ���ӿ�����Ʈ ���� �� �ؾ� �� �۾����� ���⼭ ������
        gameObject.name = unitName;
    }
    #endregion

    // �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ� //

}
