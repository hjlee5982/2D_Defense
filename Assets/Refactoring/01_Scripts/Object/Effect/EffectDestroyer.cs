using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    // 이펙트 애니메이션의 마지막 프레임에 이벤트 바인딩
    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}
