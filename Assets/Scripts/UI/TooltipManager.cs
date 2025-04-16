using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private TooltipPanel _tooltipPanel;



    public void ShowTooltip(IStatTooltipData statData)
    {
        _tooltipPanel.ShowTooltip(statData, Vector2.zero);
    }

    public void HideTooltip()
    {
        _tooltipPanel.HideTooltip();
    }
}

public interface IStatTooltipData
{
    string StatName { get; }
    string GetStatInfo();
}
