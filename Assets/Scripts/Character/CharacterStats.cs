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

    private StatBonusConfig _statBonusConfig;

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

        _statBonusConfig = new StatBonusConfig();

        foreach (PrimaryStat stat in PrimaryStats)
        {
            stat.OnLevelUp += ApplyLifeStatBonuses;
        }
        foreach (LifeStat stat in LifeStats)
        {
            stat.OnDrainOver += StatDrainOver;
        }
    }

    public PrimaryStat GetPrimaryStat(PrimaryStatType statType) => PrimaryStats.FirstOrDefault(x => x.StatTypes == statType);

    public LifeStat GetLifeStat(LifeStatType statType) => LifeStats.FirstOrDefault(x => x.StatTypes == statType);


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
        if (!_statBonusConfig.BonusMap.TryGetValue(stat.StatTypes, out var bonuses))
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

    public void DrainLifeStat(LifeStatType statType, float drainValue)
    {
        LifeStat stat = GetLifeStat(statType);

        if (stat == null) return;

        stat.DrainStat(drainValue);

        OnLifeStatsChanged?.Invoke(stat);
    }

    private void StatDrainOver(LifeStatType statType)
    {
        Debug.Log($"{statType} sýfýrlandý fakat þuan bir etkisi yok.");
    }
}

[Serializable]
public class LifeStat : Stat<LifeStatType>
{
    public event Action<LifeStatType> OnDrainOver;

    public int MaxValue { get; set; }
    public float CurrentValue { get; set; }

    public LifeStat(string statName, LifeStatType statTypes, int baseStatValue) : base(statName, statTypes, baseStatValue)
    {
        MaxValue = baseStatValue;
        CurrentValue = MaxValue;
    }

    public void DrainStat(float drainValue)
    {
        CurrentValue -= Mathf.Clamp(CurrentValue, 0, drainValue);

        if (CurrentValue <= 0)
        {
            OnDrainOver?.Invoke(StatTypes);
        }
    }
}

[Serializable]
public class PrimaryStat : Stat<PrimaryStatType>
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

public class StatBonusConfig
{
    public class StatBonus
    {
        public LifeStatType TargetStat { get; set; }
        public int BonusValue { get; set; }
    }

    public Dictionary<PrimaryStatType, List<StatBonus>> BonusMap = new()
    {
        {
            PrimaryStatType.Strength, new List<StatBonus>()
            {
                new StatBonus{ TargetStat = LifeStatType.Health, BonusValue = 4 },
            }

        },
        {
            PrimaryStatType.Agility, new List<StatBonus>()
            {

            }
        },
        {
            PrimaryStatType.Endurance, new List<StatBonus>()
            {
                new StatBonus{ TargetStat = LifeStatType.Health, BonusValue = 12 },
                new StatBonus{ TargetStat = LifeStatType.Satiety, BonusValue = 5}
            }
        }
    };
}