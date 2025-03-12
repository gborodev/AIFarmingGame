using System;
using UnityEngine;

public class KillQuest : Quest
{
    [Header("Quest Kill Objectives")]
    [SerializeField] private KillObjective[] _killObjectives;

    public override bool QuestAccept()
    {
        if (base.QuestAccept() is false) return false;

        QuestEvents.OnEnemyKilled += (enemy) =>
        {
            foreach (KillObjective objective in _killObjectives)
            {
                if (objective.IsCompleted) return;

                if (enemy.EnemyID == objective.Target.EnemyID)
                {
                    objective.Progress++;

                    if (objective.IsCompleted)
                    {
                        bool isDone = AllObjectivesIsDone();

                        if (isDone) QuestState = QuestState.Completed;
                    }
                }
            }

        };
        return true;
    }

    private bool AllObjectivesIsDone()
    {
        int completedCount = 0;

        foreach (KillObjective objective in _killObjectives)
        {
            if (objective.IsCompleted)
            {
                completedCount++;
            }
        }

        if (completedCount >= _killObjectives.Length)
        {
            return true;
        }
        return false;
    }

    protected override string GenerateQuestDialogue()
    {
        return "";
    }

    protected override string GenerateQuestName()
    {
        return "";
    }
}

[Serializable]
public class KillObjective
{
    [SerializeField] private Enemy _targetEnemy;
    [SerializeField] private int _requirement;

    private int _progress = 0;
    private bool _isCompleted = false;

    public Enemy Target => _targetEnemy;
    public int Requirement => _requirement;
    public bool IsCompleted => _isCompleted;

    public int Progress
    {
        get { return _progress; }
        set
        {
            _progress = value;

            if (_progress >= _requirement)
            {
                _isCompleted = true;
            }
        }
    }

}
