using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    // ����Ʈ �ִϸ��̼��� ������ �����ӿ� �̺�Ʈ ���ε�
    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}
