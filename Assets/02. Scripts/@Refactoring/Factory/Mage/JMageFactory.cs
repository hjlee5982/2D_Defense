using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class JMageFactory : JUnitFactory
{
    #region OVERRIDE
    public override IUnitProduct GetProduct()
    {
        JMage product = _magePool.Get();
        product.Initialize();
        return product;
    }

    public override void ReturnProduct(IUnitProduct product)
    {
        if (product is JMage archer)
        {
            _magePool.Release(archer);
        }
    }
    #endregion

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ //

    public GameObject MagePrefab;
    private ObjectPool<JMage> _magePool;

    private void Awake()
    {
        _magePool = new ObjectPool<JMage>
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

        List<JMage> tempList = new List<JMage>();

        for (int i = 0; i < 10; ++i)
        {
            tempList.Add(_magePool.Get());
        }
        for (int i = 0; i < 10; ++i)
        {
            _magePool.Release(tempList[i]);
        }
    }

    JMage CreateNewProduct()
    {
        GameObject go = Instantiate(MagePrefab);
        return go.GetComponent<JMage>();
    }
}
