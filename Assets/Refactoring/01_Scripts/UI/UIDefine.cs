using UnityEngine;
using UnityEngine.UI;

// ���ݿ� ���� ��ư ��Ȱ��ȭ
public class CostOptionUI
{
    public Button Button;
    public GameObject Restrictor;
    public int cost;

    public CostOptionUI(Button button, GameObject restrictor, int cost)
    {
        this.Button     = button;
        this.Restrictor = restrictor;
        this.cost       = cost;
    }
}
