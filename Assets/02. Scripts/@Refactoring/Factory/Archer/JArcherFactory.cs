using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class JArcherFactory : JUnitFactory
{
    #region OVERRIDE
    public override IUnitProduct GetProduct()
    {
        JArcher product = _archerPool.Get();
        product.Initialize();
        return product;
    }

    public override void ReturnProduct(IUnitProduct product)
    {
        if(product is JArcher archer)
        {
            _archerPool.Release(archer);
        }
    }
    #endregion

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ //

    public GameObject           ArcherPrefab;
    private ObjectPool<JArcher> _archerPool;

    private void Awake()
    {
        _archerPool = new ObjectPool<JArcher>
            (
                createFunc      : CreateNewProduct,
                actionOnGet     : product => product.gameObject.SetActive(true),
                actionOnRelease : product => product.gameObject.SetActive(false),
                actionOnDestroy : product => Destroy(product.gameObject),
                collectionCheck : false,
                defaultCapacity : 10
            );

        // 여기까지 하면 Pool에 10개의 공간만 생기고 안에는 비어있음
        // 미리 만들려먼 아래 코드까지 돌아야 됨

        List<JArcher> tempList = new List<JArcher>();

        for (int i = 0; i < 10; ++i)
        {
            tempList.Add(_archerPool.Get());
        }
        for (int i = 0; i < 10; ++i)
        {
            _archerPool.Release(tempList[i]);
        }
    }

    JArcher CreateNewProduct()
    {
        GameObject go = Instantiate(ArcherPrefab);
        return go.GetComponent<JArcher>();
    }

}
