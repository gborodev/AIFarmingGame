using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CharacterEnums;

[Serializable]
public class CharacterStats
{
    public List<LifeStat> LifeStats { get; private set; }
    public List<PrimaryStat> PrimaryStats { get; private set; }

    public CharacterStats()
    {
        //Instances
        LifeStats = new List<LifeStat>()
        {
            new LifeStat("Health", LifeStatType.Health, 100),
            new LifeStat("Satiety", LifeStatType.Satiety, 100),
            new LifeStat("Fatigue", LifeStatType.Fatigue, 100),
            new LifeStat("Sickness", LifeStatType.Sickness, 100),
        };
        PrimaryStats = new List<PrimaryStat>()
        {
            new PrimaryStat("Strength", PrimaryStatType.Strength, 0),
            new PrimaryStat("Agility", PrimaryStatType.Agility, 0),
            new PrimaryStat("Endurance", PrimaryStatType.Endurance, 0),
        };
    }

    public void AddExpToStat(PrimaryStatType statType, int expAmount)
    {
        PrimaryStat stat = PrimaryStats.FirstOrDefault(x => x.StatTypes == statType);

        if (stat == null)
        {
            Debug.LogError($"Error: {statType} is not found.");
            return;
        }

        int maxLevel = StatManagement.MAX_PRIMARY_STAT_LEVEL;
        if (stat.CurrentLevel >= maxLevel)
        {
            Debug.LogWarning($"Warning: {statType} is reach maximum!");
            return;
        }

        stat.CurrentXP += expAmount;
        stat.TotalXP += expAmount;

        UpdateStat(stat);
    }

    private void UpdateStat(PrimaryStat stat)
    {
        while (true)
        {
            int requirement = StatManagement.Instance.GetCalculatedRequirementXP(stat.CurrentLevel);

            if (stat.CurrentXP < requirement)
                break;

            stat.CurrentXP -= requirement;
            stat.CurrentLevel++;

            int maxLevel = StatManagement.MAX_PRIMARY_STAT_LEVEL;
            if (stat.CurrentLevel >= maxLevel)
            {
                stat.CurrentXP = 0;
                stat.CurrentLevel = maxLevel;
            }

            ApplyStatBonuses(stat.StatTypes);
        }
    }

    private void ApplyStatBonuses(PrimaryStatType stat)
    {
        Dictionary<PrimaryStatType, Action> statBonuses = new Dictionary<PrimaryStatType, Action>()
        {
            {   PrimaryStatType.Strength, () =>
                {
                    LifeStat health = GetLifeStat(LifeStatType.Health);

                    health.MaxValue += StatManagement.Instance.HealthPerStrength;
                    health.CurrentValue += StatManagement.Instance.HealthPerStrength;
                }
            },
            {
                PrimaryStatType.Agility, () =>
                {
                    // TODO: Agility bonuslarý eklenecek, örneðin her bir agility yorgunluk statýný etkileyecek

                    Debug.Log("Agility statý arttý, ancak þu an herhangi bir etkisi yok.");
                }
            },
            {
                PrimaryStatType.Endurance, () =>
                {
                    LifeStat health = GetLifeStat(LifeStatType.Health);
                    LifeStat satiety = GetLifeStat(LifeStatType.Satiety);

                    health.MaxValue += StatManagement.Instance.HealthPerEndurance;
                    health.CurrentValue += StatManagement.Instance.HealthPerEndurance;

                    satiety.MaxValue += StatManagement.Instance.SatietyPerEndurance;
                    satiety.CurrentValue += StatManagement.Instance.SatietyPerEndurance;
                }
            }
        };

        if (statBonuses.ContainsKey(stat))
        {
            statBonuses[stat].Invoke();
        }
    }

    public PrimaryStat GetPrimaryStat(PrimaryStatType statType) => PrimaryStats.FirstOrDefault(x => x.StatTypes == statType);

    public LifeStat GetLifeStat(LifeStatType statType) => LifeStats.FirstOrDefault(x => x.StatTypes == statType);
}

[Serializable]
public class LifeStat : Stat<LifeStatType>
{
    public float MaxValue { get; set; }
    public float CurrentValue { get; set; }

    public LifeStat(string statName, LifeStatType statTypes, int baseStatValue) : base(statName, statTypes, baseStatValue)
    {
        MaxValue = baseStatValue;
        CurrentValue = baseStatValue;
    }
}

[Serializable]
public class PrimaryStat : Stat<PrimaryStatType>
{
    public int CurrentXP { get; set; }
    public int CurrentLevel { get; set; }
    public int TotalXP { get; set; }

    public PrimaryStat(string statName, PrimaryStatType statTypes, int baseStatValue) : base(statName, statTypes, baseStatValue)
    {
        CurrentXP = 0;
        CurrentLevel = baseStatValue;
    }
}

[Serializable]
public abstract class Stat<T>
{
    private string _statName;
    private T _statTypes;
    private int _baseStatValue;

    public string StatName => _statName;
    public T StatTypes => _statTypes;
    public int BaseStatValue => _baseStatValue;

    public Stat(string statName, T statTypes, int baseStatValue)
    {
        _statName = statName;
        _statTypes = statTypes;
        _baseStatValue = baseStatValue;
    }
}
