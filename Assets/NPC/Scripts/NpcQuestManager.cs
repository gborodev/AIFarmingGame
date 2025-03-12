using System.Collections.Generic;
using UnityEngine;

public class NpcQuestManager : MonoBehaviour
{
    private Quest[] _quests;

    private List<Quest> _availableQuestList = new List<Quest>();

    private void Awake()
    {
        _quests = GetComponentsInChildren<Quest>();

        foreach (Quest quest in _quests)
        {
            quest.OnQuestAvailable += QuestAvailable;
            quest.OnQuestCompleted += QuestCompleted;

            quest.QuestInitialize();
        }
    }

    public List<Quest> GetAllAvailableQuests()
    {
        return _availableQuestList;
    }

    private void QuestAvailable(Quest quest)
    {
        _availableQuestList.Add(quest);
    }

    private void QuestCompleted(Quest quest)
    {
        quest.OnQuestAvailable -= QuestAvailable;
        quest.OnQuestCompleted -= QuestCompleted;

        _availableQuestList.Remove(quest);
    }
}
