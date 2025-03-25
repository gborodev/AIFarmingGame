public static class QuestEvents
{
    public delegate void EnemyKillHandler(Enemy enemy);


    public static EnemyKillHandler OnEnemyKilled;
}
