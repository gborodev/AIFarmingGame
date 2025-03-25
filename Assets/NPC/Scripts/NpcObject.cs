using UnityEngine;

public class NpcObject : MonoBehaviour
{
    private NpcQuestManager _questManager;

    [SerializeField] private string _npcName = "New NPC";

    public string NPC_Name => _npcName;

    private void Start()
    {
        _questManager = GetComponentInChildren<NpcQuestManager>();
    }

    private void OnValidate()
    {
        if (gameObject.name != _npcName)
        {
            gameObject.name = _npcName;
        }
    }

    public bool CanInteract()
    {
        return _questManager.GetAllAvailableQuests().Count > 0;
    }

    public void Interact()
    {

    }
    public void EndInteract()
    {

    }

    public Transform GetTransform()
    {
        return transform;
    }
}
