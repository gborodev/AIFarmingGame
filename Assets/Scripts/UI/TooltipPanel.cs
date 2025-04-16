using System.Collections;
using TMPro;
using UnityEngine;

public class TooltipPanel : MonoBehaviour
{
    [SerializeField] private Transform _panelContent;

    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _infoText1, _infoText2;
    [SerializeField] private TextMeshProUGUI _goldText;

    private Coroutine _tco;
    private bool _showing;

    private void Awake()
    {
        if (_panelContent.gameObject.activeInHierarchy) _panelContent.gameObject.SetActive(false);
    }

    public void ShowTooltip(IStatTooltipData statData, Vector2 position)
    {
        if (_tco != null && _showing == false)
        {
            StopCoroutine(_tco);
            _tco = null;
        }
        ClearTooltip();

        string name = statData.StatName;
        string info = statData.GetStatInfo();

        _tco = StartCoroutine(ShowTimer(name: name, info1: info));
    }

    public void HideTooltip()
    {
        if (_tco != null && _showing == false)
        {
            StopCoroutine(_tco);
            return;
        }

        _showing = false;
        _panelContent.gameObject.SetActive(false);
    }

    private IEnumerator ShowTimer(string name = "", string type = "", string info1 = "", string info2 = "", string gold = "")
    {
        yield return new WaitForSeconds(0.2f);
        _showing = true;

        _headerText.text = name;
        _infoText1.text = info1;

        _panelContent.gameObject.SetActive(true);
    }

    private void ClearTooltip()
    {
        _headerText.text = string.Empty;
        _typeText.text = string.Empty;
        _goldText.text = string.Empty;
        _infoText1.text = string.Empty;
        _infoText2.text = string.Empty;
    }
}
