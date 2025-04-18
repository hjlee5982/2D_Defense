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

    // �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ� //

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

        // ������� �ϸ� Pool�� 10���� ������ ����� �ȿ��� �������
        // �̸� ������� �Ʒ� �ڵ���� ���ƾ� ��

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
