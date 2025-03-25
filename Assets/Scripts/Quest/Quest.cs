using System;
using UnityEngine;

public enum QuestState { Available, Progress, Completed, Failed }

public abstract class Quest : MonoBehaviour
{
    private string _questID;
    private string _questName;
    private string _questDialogue;

    [Header("Quest Rewards")]
    [SerializeField] private QuestReward _questReward;

    public string QuestID => _questID;
    public string QuestName => _questName;
    public string QuestDialogue => _questDialogue;
    public QuestReward QuestReward => _questReward;

    public event Action<Quest> OnQuestAvailable;
    public event Action<Quest> OnQuestCompleted;

    private QuestState _state;
    public QuestState QuestState
    {
        get { return _state; }
        set
        {
            _state = value;

            switch (_state)
            {
                case QuestState.Available:
                    OnQuestAvailable?.Invoke(this);
                    break;
                case QuestState.Progress:
                    break;
                case QuestState.Completed:
                    OnQuestCompleted?.Invoke(this);
                    break;
                case QuestState.Failed:
                    break;
                default:
                    break;
            }
        }
    }

    public void QuestInitialize()
    {
        _questID = Guid.NewGuid().ToString();
        _questName = GenerateQuestName();
        _questDialogue = GenerateQuestDialogue();

        QuestState = QuestState.Available;
    }

    public virtual bool QuestAccept()
    {
        switch (QuestState)
        {
            case QuestState.Available:
                QuestState = QuestState.Progress;
                return true;
            case QuestState.Progress:
                return false;
            case QuestState.Completed:
                return false;
            case QuestState.Failed:
                return false;
            default:
                return true;
        }
    }

    protected abstract string GenerateQuestName();
    protected abstract string GenerateQuestDialogue();
}

[Serializable]
public class QuestReward
{
    [SerializeField] private ItemReward[] _rewardItems;
    [SerializeField] private int _rewardGold;
}

[Serializable]
public class ItemReward
{
    [SerializeField] private Item _rewardItem;
    [SerializeField] private int _rewardAmount = 1;
}
