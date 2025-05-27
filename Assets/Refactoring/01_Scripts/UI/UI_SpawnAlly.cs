using UnityEngine;
using UnityEngine.UI;
using static GameStatusChangeEvent;

public class UI_SpawnAlly : MonoBehaviour
{
    #region VARIABLES
    [Header("구매 제한 UI")]
    private GameObject _restrictor_1;
    private GameObject _restrictor_2;
    private GameObject _restrictor_3;

    [Header("유닛 소환 비용(업데이트용)")]
    private int _summonCost_1;
    private int _summonCost_2;
    private int _summonCost_3;
    #endregion





    #region OVERRIDES
    #endregion





    #region MONOBEHAVIOUR
    void Awake()
    {
        _restrictor_1 = transform.Find("Restrictor_1").gameObject;
        _restrictor_2 = transform.Find("Restrictor_2").gameObject;
        _restrictor_3 = transform.Find("Restrictor_3").gameObject;

        _restrictor_1.SetActive(false);
        _restrictor_2.SetActive(false);
        _restrictor_3.SetActive(false);

        transform.Find("SummonButton_1").GetComponent<Button>().onClick.AddListener(() => BeginSpawnAlly(0));
        transform.Find("SummonButton_2").GetComponent<Button>().onClick.AddListener(() => BeginSpawnAlly(1));
        transform.Find("SummonButton_3").GetComponent<Button>().onClick.AddListener(() => BeginSpawnAlly(2));
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        UpdateRestrictor();
        JEventBus.Subscribe<SummonRestrictionEvent>(GoldChanged);
    }

    private void OnDisable()
    {
        JEventBus.Unsubscribe<SummonRestrictionEvent>(GoldChanged);
    }
    #endregion





    #region FUNCTIONS
    private void BeginSpawnAlly(int btnIdx)
    {
        // UI_SpawnAlly -> JGameManager
        JEventBus.SendEvent(new StartSpawnAllyEvent(btnIdx));
    }

    private void GoldChanged(SummonRestrictionEvent e)
    {
        _summonCost_1 = e.Level_1;
        _summonCost_2 = e.Level_2;
        _summonCost_3 = e.Level_3;

        if (e.Gold < e.Level_1)
        {
            _restrictor_1.SetActive(true);
            _restrictor_2.SetActive(true);
            _restrictor_3.SetActive(true);
        }
        else if(e.Gold < e.Level_2)
        {
            _restrictor_1.SetActive(false);
            _restrictor_2.SetActive(true);
            _restrictor_3.SetActive(true);
        }
        else if (e.Gold < e.Level_3)
        {
            _restrictor_1.SetActive(false);
            _restrictor_2.SetActive(false);
            _restrictor_3.SetActive(true);
        }
        else
        {
            _restrictor_1.SetActive(false);
            _restrictor_2.SetActive(false);
            _restrictor_3.SetActive(false);
        }
    }

    private void UpdateRestrictor()
    {
        int currentGold = JGameManager.Instance.Gold;

        if (currentGold < _summonCost_1)
        {
            _restrictor_1.SetActive(true);
            _restrictor_2.SetActive(true);
            _restrictor_3.SetActive(true);
        }
        else if (currentGold < _summonCost_2)
        {
            _restrictor_1.SetActive(false);
            _restrictor_2.SetActive(true);
            _restrictor_3.SetActive(true);
        }
        else if (currentGold < _summonCost_3)
        {
            _restrictor_1.SetActive(false);
            _restrictor_2.SetActive(false);
            _restrictor_3.SetActive(true);
        }
        else
        {
            _restrictor_1.SetActive(false);
            _restrictor_2.SetActive(false);
            _restrictor_3.SetActive(false);
        }
    }
    #endregion
}
