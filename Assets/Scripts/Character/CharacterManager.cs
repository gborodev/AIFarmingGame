using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    public CharacterStats CharacterStats { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        CharacterStats = new CharacterStats();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterStats.AddExpToStat(CharacterEnums.PrimaryStatType.Strength, 1000f);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CharacterStats.UpdateLifeStatValue(CharacterEnums.LifeStatType.Energy, -20);
        }
    }
}
