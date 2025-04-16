using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CharacterEnums;

[Serializable]
public class CharacterStats
{
    public event Action<PrimaryStat> OnPrimaryStatsChanged;
    public event Action<LifeStat> OnLifeStatsChanged;

    public List<LifeStat> LifeStats { get; private set; }
    public List<PrimaryStat> PrimaryStats { get; private set; }
    public CharacterStats()
    {
        //Instances
        LifeStats = new List<LifeStat>()
        {
            new LifeStat("Health", LifeStatType.Health, 0, 100, 100),
            new LifeStat("Hunger", LifeStatType.Hunger, -100, 100, 100),
            new LifeStat("Energy", LifeStatType.Energy, 0, 100, 100),
        };
        PrimaryStats = new List<PrimaryStat>()
        {
            new PrimaryStat("Strength", PrimaryStatType.Strength, 0),
            new PrimaryStat("Agility", PrimaryStatType.Agility, 0),
            new PrimaryStat("Endurance", PrimaryStatType.Endurance, 0),
        };

        foreach (PrimaryStat stat in PrimaryStats)
        {
            //Events
            stat.OnLevelUp += ApplyLifeStatBonuses;
        }
        foreach (LifeStat stat in LifeStats)
        {
            //Events
        }
    }

    public void UpdateLifeStatValue(LifeStatType statType, float value, HashSet<LifeStatType> visited = null)
    {
        //Control overload method
        visited ??= new HashSet<LifeStatType>();

        if (visited.Contains(statType)) return; // döngüyü engelle

        visited.Add(statType);

        //Get Stat
        LifeStat stat = GetLifeStat(statType);
        if (stat == null) return;

        float statValue = stat.CurrentValue + value;

        switch (statType)
        {
            case LifeStatType.Health:

                if (statValue <= 0)
                {
                    CharacterEvents.CharacterStatEvents.OnDeath?.Invoke();
                }
                break;

            default:

                if (statValue <= 0)
                {
                    if (LifeStatPenaltyConfig.PenaltyMap.TryGetValue(statType, out var penalties))
                    {
                        foreach (var penalty in penalties)
                        {
                            UpdateLifeStatValue(penalty.AffectedStat, penalty.PenaltyAmount, visited);
                        }
                    }
                }
                break;
        }

        if (statValue >= stat.MaxValue)
        {
            statValue = stat.MaxValue;
        }
        else if (statValue <= stat.MinValue)
        {
            statValue = stat.MinValue;
        }

        stat.CurrentValue = statValue;

        OnLifeStatsChanged?.Invoke(stat);
    }

    public void AddExpToStat(PrimaryStatType statType, float expAmount)
    {
        PrimaryStat stat = GetPrimaryStat(statType);

        if (stat != null)
        {
            stat.AddExperience(expAmount);

            OnPrimaryStatsChanged?.Invoke(stat);
        }
    }

    private void ApplyLifeStatBonuses(PrimaryStat stat)
    {
        if (!PrimaryStatBonusConfig.BonusMap.TryGetValue(stat.StatType, out var bonuses))
            return;

        foreach (var bonus in bonuses)
        {
            LifeStat lifestat = GetLifeStat(bonus.TargetStat);

            if (lifestat == null) continue;

            lifestat.MaxValue += bonus.BonusValue;
            lifestat.CurrentValue += bonus.BonusValue;

            OnLifeStatsChanged?.Invoke(lifestat);
        }
    }

    //Getters
    public PrimaryStat GetPrimaryStat(PrimaryStatType statType) => PrimaryStats.FirstOrDefault(x => x.StatType == statType);

    public LifeStat GetLifeStat(LifeStatType statType) => LifeStats.FirstOrDefault(x => x.StatType == statType);
}

[Serializable]
public class LifeStat : Stat<LifeStatType>, IStatTooltipData
{
    public int MinValue { get; private set; }
    public int MaxValue { get; set; }
    public float CurrentValue { get; set; }

    public LifeStat(string statName, LifeStatType statTypes, int baseMinValue, int baseMaxValue, int baseCurrentValue) : base(statName, statTypes, baseMaxValue)
    {
        MinValue = baseMinValue;
        MaxValue = baseMaxValue;
        CurrentValue = baseCurrentValue;
    }

    public string GetStatInfo()
    {
        return TooltipGenerator.GenerateLifeStatTooltip(this);
    }
}

[Serializable]
public class PrimaryStat : Stat<PrimaryStatType>, IStatTooltipData
{
    public event Action<PrimaryStat> OnLevelUp;

    public int CurrentLevel { get; private set; }
    public float CurrentXP { get; private set; }
    public float TotalXP { get; private set; }

    private AnimationCurve _levelCurve;

    private const int MAX_LEVEL = 50;
    private const int MAX_EXPERIENCE = 1881;

    public int RequirementXP { get; private set; }

    public PrimaryStat(string statName, PrimaryStatType statTypes, int baseStatValue) : base(statName, statTypes, baseStatValue)
    {
        _levelCurve = new AnimationCurve(
            new Keyframe(0, 1, 10, 10),
            new Keyframe(MAX_LEVEL, MAX_EXPERIENCE, 60, 60)
            );

        CurrentXP = 0f;
        TotalXP = 0f;

        CurrentLevel = baseStatValue;
    }

    public void AddExperience(float experience)
    {
        if (CurrentLevel >= MAX_LEVEL) return;

        CurrentXP += experience;
        TotalXP += experience;

        RequirementXP = (int)_levelCurve.Evaluate(CurrentLevel);

        while (true)
        {
            if (CurrentXP < RequirementXP)
                break;

            CurrentXP -= RequirementXP;
            CurrentLevel++;

            OnLevelUp?.Invoke(this);

            if (CurrentLevel >= MAX_LEVEL)
            {
                CurrentLevel = MAX_LEVEL;
                CurrentXP = 0f;
                break;
            }

            RequirementXP = (int)_levelCurve.Evaluate(CurrentLevel);
        }
    }

    public string GetStatInfo()
    {
        return TooltipGenerator.GeneratePrimaryStatTooltip(this);
    }
}

[Serializable]
public abstract class Stat<T>
{
    private string _statName;
    private T _statType;
    private int _baseStatValue;

    public string StatName => _statName;
    public T StatType => _statType;
    public int BaseStatValue => _baseStatValue;

    public Stat(string statName, T statTypes, int baseStatValue)
    {
        _statName = statName;
        _statType = statTypes;
        _baseStatValue = baseStatValue;
    }
}

public class PrimaryStatBonusConfig
{
    public class StatBonus
    {
        public LifeStatType TargetStat;
        public int BonusValue;
    }

    public static readonly Dictionary<PrimaryStatType, List<StatBonus>> BonusMap = new()
    {
        {
            PrimaryStatType.Strength, new List<StatBonus>()
            {
                new StatBonus{ TargetStat = LifeStatType.Health, BonusValue = 5 },
                new StatBonus{ TargetStat = LifeStatType.Energy, BonusValue = 3 }
            }

        },
        {
            PrimaryStatType.Agility, new List<StatBonus>()
            {
                new StatBonus{ TargetStat = LifeStatType.Hunger, BonusValue = 3 },
                new StatBonus{ TargetStat = LifeStatType.Energy, BonusValue= 5 }
            }
        },
        {
            PrimaryStatType.Endurance, new List<StatBonus>()
            {
                new StatBonus{ TargetStat = LifeStatType.Health, BonusValue = 8 },
                new StatBonus{ TargetStat = LifeStatType.Hunger, BonusValue = 5}
            }
        }
    };
}
public class LifeStatPenaltyConfig
{
    public class StatPenalty
    {
        public LifeStatType AffectedStat;
        public float PenaltyAmount;
    }

    public static readonly Dictionary<LifeStatType, List<StatPenalty>> PenaltyMap = new()
    {
        {
            LifeStatType.Hunger, new List<StatPenalty>
            {
                new StatPenalty { AffectedStat = LifeStatType.Health, PenaltyAmount = -5f },
                new StatPenalty { AffectedStat = LifeStatType.Energy, PenaltyAmount = -3f }
            }
        },
        {
            LifeStatType.Energy, new List<StatPenalty>
            {
                new StatPenalty { AffectedStat = LifeStatType.Hunger, PenaltyAmount = -5f },
            }
        }
    };
}