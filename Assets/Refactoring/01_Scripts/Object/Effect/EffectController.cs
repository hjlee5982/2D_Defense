using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    // ����Ʈ �ִϸ��̼��� ������ �����ӿ� �̺�Ʈ ���ε�
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
