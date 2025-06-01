using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpawnAlly : MonoBehaviour
{
    #region VARIABLES
    [Header("강화 버튼 + 가림막 + 비용")]
    private List<CostOptionUI> _options = new List<CostOptionUI>();
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
    }

    void Start()
    {
        for(int i = 0; i < 3; ++i)
        {
            // 해당 데이터
            AllyUnitData data = JGameManager.Instance.DataLoader.AllyUnitData[i];

            // 각 버튼 Transform
            Transform buttonTransform = transform.Find("SummonButton_" + i.ToString()).transform;

            // 소환 버튼 이벤트 바인딩
            int index = i;
            Button button = buttonTransform.GetComponent<Button>();
            button.onClick.AddListener(() => 
            { 
                BeginSpawnAlly(index); 
                JAudioManager.Instance.PlaySFX("ButtonClick"); 
            });

            // 유닛 이름 설정
            TextMeshProUGUI nameText = buttonTransform.Find("Name").GetComponent<TextMeshProUGUI>();
            nameText.text = data.UnitName;

            // 유닛 비용 설정
            TextMeshProUGUI costText = buttonTransform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = data.Cost.ToString();

            // 소환 제한 UI 설정
            GameObject restrictor = transform.Find("Restrictor_" + i.ToString()).gameObject;
            restrictor.SetActive(false);

            _options.Add(new CostOptionUI(button, restrictor, data.Cost));
        }
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        UpdateRestrictor(JGameManager.Instance.Gold);
        JEventBus.Subscribe<GoldRestrictionEvent>(GoldChanged);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<GoldRestrictionEvent>(GoldChanged);
    }
    #endregion





    #region FUNCTIONS
    private void BeginSpawnAlly(int btnIdx)
    {
        // UI_SpawnAlly -> JGameManager
        JEventBus.SendEvent(new StartSpawnAllyEvent(btnIdx));
    }

    private void GoldChanged(GoldRestrictionEvent e)
    {
        UpdateRestrictor(e.CurrentGold);
    }

    private void UpdateRestrictor(int currentGold)
    {
        foreach (var option in _options)
        {
            // 현재 소지 골드가 가격보다 많으면 활성화 가능
            bool canActivate = currentGold >= option.cost;

            // 활성화가 가능하다면 가림막은 해제해야 함
            option.Restrictor.SetActive(!canActivate);
        }
    }
    #endregion
}
