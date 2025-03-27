using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    public CharacterStats CharacterStats { get; private set; }

    private void Start()
    {
        CharacterStats = new CharacterStats();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CharacterStats.AddExpToStat(CharacterEnums.PrimaryStatType.Endurance, 1000);
    }
}
