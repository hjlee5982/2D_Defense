using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class JWarriorFactory : JUnitFactory
{
    #region OVERRIDE
    public override IUnitProduct GetProduct()
    {
        JWarrior product = _warriorPool.Get();
        product.Initialize();
        return product;
    }

    public override void ReturnProduct(IUnitProduct product)
    {
        if (product is JWarrior archer)
        {
            _warriorPool.Release(archer);
        }
    }
    #endregion

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ //

    public  GameObject            WarriroPrefab;
    private ObjectPool<JWarrior> _warriorPool;

    private void Awake()
    {
        _warriorPool = new ObjectPool<JWarrior>
            (
                createFunc: CreateNewProduct,
                actionOnGet: product => product.gameObject.SetActive(true),
                actionOnRelease: product => product.gameObject.SetActive(false),
                actionOnDestroy: product => Destroy(product.gameObject),
                collectionCheck: false,
                defaultCapacity: 10
            );

        // 여기까지 하면 Pool에 10개의 공간만 생기고 안에는 비어있음
        // 미리 만들려먼 아래 코드까지 돌아야 됨

        List<JWarrior> tempList = new List<JWarrior>();

        for (int i = 0; i < 10; ++i)
        {
            tempList.Add(_warriorPool.Get());
        }
        for (int i = 0; i < 10; ++i)
        {
            _warriorPool.Release(tempList[i]);
        }
    }

    JWarrior CreateNewProduct()
    {
        GameObject go = Instantiate(WarriroPrefab);
        return go.GetComponent<JWarrior>();
    }
}
