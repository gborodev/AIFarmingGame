using UnityEngine;

public class StatManagement : Singleton<StatManagement>
{
    [Header("Primary Stat Options")]
    public const int MAX_PRIMARY_STAT_LEVEL = 50;
    public const int MAX_PRIMARY_STAT_EXPERIENCE = 1881;
    [SerializeField] private AnimationCurve _baseStatCurve;

    [Header("Primary Stat Bonuses")]
    [SerializeField] private float _healthPerStrength;
    [SerializeField] private float _healthPerEndurance;
    [SerializeField] private float _satietyPerEndurance;

    public float HealthPerStrength => _healthPerStrength;
    public float HealthPerEndurance => _healthPerEndurance;
    public float SatietyPerEndurance => _satietyPerEndurance;

    public int GetCalculatedRequirementXP(int level)
    {
        float curvePoint = (float)level / MAX_PRIMARY_STAT_LEVEL;
        float experiencePoint = _baseStatCurve.Evaluate(curvePoint);
        float experience = experiencePoint * MAX_PRIMARY_STAT_EXPERIENCE;

        experience = Mathf.Clamp(experience, 1, MAX_PRIMARY_STAT_EXPERIENCE);


        return Mathf.RoundToInt(experience);
    }
}
