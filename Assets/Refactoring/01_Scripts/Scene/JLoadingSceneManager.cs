using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class JLoadingSceneManager : MonoBehaviour
{
    #region VARIABLES
    [Header("텍스트")]
    private TextMeshProUGUI ID_LClick;
    private TextMeshProUGUI ID_LClickDesc;
    private TextMeshProUGUI ID_RClick;
    private TextMeshProUGUI ID_RClickDesc;
    private TextMeshProUGUI ID_Loading;
    private TextMeshProUGUI _loadingEffect;

    [Header("로딩 이펙트")]
    private string _effect     = "";
    private float  _elapseTime = 0f;
    private int    _count      = 0;
    #endregion





    #region MONOBEHAVIOUR
    private void Awake()
    {
        ID_LClick      = transform.Find("ID_LClick"     ).GetComponent<TextMeshProUGUI>();
        ID_LClickDesc  = transform.Find("ID_LClickDesc" ).GetComponent<TextMeshProUGUI>();
        ID_RClick      = transform.Find("ID_RClick"     ).GetComponent<TextMeshProUGUI>();
        ID_RClickDesc  = transform.Find("ID_RClickDesc" ).GetComponent<TextMeshProUGUI>();
        ID_Loading     = transform.Find("ID_Loading"    ).GetComponent<TextMeshProUGUI>();
        _loadingEffect = transform.Find("LoadingEffect").GetComponent<TextMeshProUGUI>();

        ID_LClick    .text = JSettingManager.Instance.GetText(ID_LClick    .name);
        ID_LClickDesc.text = JSettingManager.Instance.GetText(ID_LClickDesc.name);
        ID_RClick    .text = JSettingManager.Instance.GetText(ID_RClick    .name);
        ID_RClickDesc.text = JSettingManager.Instance.GetText(ID_RClickDesc.name);
        ID_Loading   .text = JSettingManager.Instance.GetText(ID_Loading   .name);
    }

    private void Start()
    {
        GameObject padeEffectObj = Instantiate(JEffectManager.Instance.GetEffect("Pade"), Vector3.zero, Quaternion.identity);
        padeEffectObj.transform.SetParent(GameObject.Find("UI").transform, false);
        Animator padeOut = padeEffectObj.GetComponent<Animator>();
        padeOut.SetTrigger("PadeIn");

        MoveNextScene();
    }

    private void Update()
    {
        LoadingEffect();
    }
    #endregion





    #region FUNCTIONS
    private void LoadingEffect()
    {
        _elapseTime += Time.deltaTime;

        if (_count == 5)
        {
            _effect = "";

            _count = 0;
        }
        if (_elapseTime >= 0.2f)
        {
            _effect += ".  ";

            _loadingEffect.text = _effect;

            _elapseTime = 0;

            ++_count;
        }

    }

    private void MoveNextScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("NewGameScene");
        loadOperation.allowSceneActivation = false;

        StartCoroutine(WaitForSceneReady(loadOperation));
    }

    private IEnumerator WaitForSceneReady(AsyncOperation operation)
    {
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(3);

        GameObject padeEffectObj = Instantiate(JEffectManager.Instance.GetEffect("Pade"), Vector3.zero, Quaternion.identity);
        padeEffectObj.transform.SetParent(GameObject.Find("UI").transform, false);
        Animator padeOut = padeEffectObj.GetComponent<Animator>();
        padeOut.SetTrigger("PadeOut");

        yield return new WaitForSecondsRealtime(1);

        operation.allowSceneActivation = true;
    }
    #endregion
}
