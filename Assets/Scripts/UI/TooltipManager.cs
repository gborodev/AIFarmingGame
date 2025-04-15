using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private TooltipPanel _tooltipPanel;

    public void ShowTooltip(LifeStat stat)
    {
        _tooltipPanel.ShowTooltip(stat, Vector2.zero);
    }
    public void ShowTooltip(PrimaryStat stat)
    {
        _tooltipPanel.ShowTooltip(stat, Vector2.zero);
    }

    public void HideTooltip()
    {
        _tooltipPanel.HideTooltip();
    }
}
