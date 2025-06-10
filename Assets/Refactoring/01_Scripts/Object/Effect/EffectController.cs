using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    // 이펙트 애니메이션의 마지막 프레임에 이벤트 바인딩
    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
    public void SetAlpha(float alpha)
    {
        Color color = transform.GetComponent<Image>().color;

        color.a = alpha;
    }
}
