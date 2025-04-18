using UnityEngine;
using UnityEngine.Pool;

public interface IUnitProduct
{
    public string UnitName { get; set; }

    public void Initialize();
}

public abstract class JUnitFactory : MonoBehaviour
{
    public abstract IUnitProduct GetProduct();
    public abstract void         ReturnProduct(IUnitProduct product);
}
