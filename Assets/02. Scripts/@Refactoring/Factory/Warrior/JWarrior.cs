using UnityEngine;

public class JWarrior : MonoBehaviour, IUnitProduct
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
        // 게임오브젝트 생성 시 해야 할 작업들을 여기서 진행함
        gameObject.name = unitName;
    }
    #endregion

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ //

}
