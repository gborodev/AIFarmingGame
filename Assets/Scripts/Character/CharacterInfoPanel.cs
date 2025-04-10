using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class CharacterInfoPanel : MonoBehaviour
{
    private CharacterStats _stats;

    [Header("Panels")]
    [SerializeField] private Transform _characterLifeStatPanel;
    [SerializeField] private Transform _characterPrimaryStatPanel;

    [Header("Prefabs")]
    [SerializeField] private CharacterInfoSlot _infoSlotPrefab;

    //Stats List
    private Dictionary<LifeStatType, CharacterInfoSlot> _lifeStatSlots = new();
    private Dictionary<PrimaryStatType, CharacterInfoSlot> _primaryStatSlots = new();

    private void Start()
    {
        _stats = CharacterManager.Instance.CharacterStats;

        List<LifeStat> lifeStats = _stats.LifeStats;
        List<PrimaryStat> primaryStats = _stats.PrimaryStats;

        for (int i = 0; i < lifeStats.Count; i++)
        {
            LifeStat lifeStat = lifeStats[i];

            CharacterInfoSlot infoSlot = Instantiate(_infoSlotPrefab, _characterLifeStatPanel);
            _lifeStatSlots.Add(lifeStat.StatTypes, infoSlot);

            LifeStatUpdate(lifeStat);
        }
        for (int i = 0; i < primaryStats.Count; i++)
        {
            PrimaryStat primaryStat = primaryStats[i];

            CharacterInfoSlot infoSlot = Instantiate(_infoSlotPrefab, _characterPrimaryStatPanel);
            _primaryStatSlots.Add(primaryStat.StatTypes, infoSlot);

            PrimaryStatUpdate(primaryStat);
        }

        _stats.OnLifeStatsChanged += LifeStatUpdate;
        _stats.OnPrimaryStatsChanged += PrimaryStatUpdate;
    }

    private void LifeStatUpdate(LifeStat stat)
    {
        if (_lifeStatSlots.TryGetValue(stat.StatTypes, out CharacterInfoSlot slot))
        {
            float ratio = stat.CurrentValue / stat.MaxValue;

            string colorHex = string.Empty;
            if (ratio < 0.25f)
            {
                colorHex = ColorUtility.ToHtmlStringRGB(Color.red);
            }
            else if (ratio < 0.5f)
            {
                colorHex = ColorUtility.ToHtmlStringRGB(Color.yellow);
            }
            else
            {
                colorHex = ColorUtility.ToHtmlStringRGB(Color.white);
            }


            slot.SetText($"{stat.StatName}: <color=#{colorHex}>{stat.CurrentValue}</color> / {stat.MaxValue}");
        }
    }

    private void PrimaryStatUpdate(PrimaryStat stat)
    {
        if (_primaryStatSlots.TryGetValue(stat.StatTypes, out CharacterInfoSlot slot))
        {
            slot.SetText($"{stat.StatName}: {stat.CurrentLevel}");
        }
    }
}
