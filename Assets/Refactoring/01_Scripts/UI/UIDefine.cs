using UnityEngine;
using UnityEngine.UI;

// 가격에 따라 버튼 비활성화
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
