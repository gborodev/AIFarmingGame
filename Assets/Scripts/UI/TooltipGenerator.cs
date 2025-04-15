using System.Collections.Generic;
using System.Text;
using static CharacterEnums;

public static class TooltipGenerator
{
    public static string GenerateLifeStatTooltip(LifeStat stat)
    {
        StringBuilder sb = new StringBuilder();

        string baseInfo = StatTooltipConfig.LifeStatDescriptions.TryGetValue(stat.StatType, out var info) ? info : "";

        sb.AppendLine(baseInfo);

        //if (LifeStatPenaltyConfig.PenaltyMap.TryGetValue(stat.StatType, out var penalties))
        //{
        //    sb.AppendLine();
        //
        //    foreach (var penalty in penalties)
        //    {
        //        sb.AppendLine($"If {stat.StatName} is too low, it reduces {penalty.AffectedStat} by {penalty.PenaltyAmount}.");
        //    }
        //}
        return sb.ToString();
    }
    public static string GeneratePrimaryStatTooltip(PrimaryStat stat)
    {
        StringBuilder sb = new StringBuilder();

        string baseInfo = StatTooltipConfig.PrimaryStatDescription.TryGetValue(stat.StatType, out var info) ? info : "";

        sb.AppendLine(baseInfo);

        if (PrimaryStatBonusConfig.BonusMap.TryGetValue(stat.StatType, out var bonuses))
        {
            sb.AppendLine();

            foreach (var bonus in bonuses)
            {
                sb.AppendLine($"Each {stat.StatName} increases {bonus.TargetStat} by {bonus.BonusValue}.");
            }
        }
        return sb.ToString();
    }

    public static class StatTooltipConfig
    {
        public static readonly Dictionary<PrimaryStatType, string> PrimaryStatDescription = new()
        {
            {
                PrimaryStatType.Strength,
                "Strength increases your physical power."
            },
            {
                PrimaryStatType.Agility,
                "Agility improves your speed and reflexes."
            },
            {
                PrimaryStatType.Endurance,
                "Endurance improves your overall resilience."
            }
        };

        public static readonly Dictionary<LifeStatType, string> LifeStatDescriptions = new()
        {
            {
                LifeStatType.Health,
                "Health represents your life. Decreases when you take damage or starve."
            },
            {
                LifeStatType.Hunger,
                "Hunger shows your need for food. If it gets too low, it affects your health and energy."
            },
            {
                LifeStatType.Energy,
                "Energy is required for actions. Depletes when moving or working."
            }
        };
    }
}

