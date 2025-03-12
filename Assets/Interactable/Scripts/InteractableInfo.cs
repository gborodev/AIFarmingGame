using TMPro;
using UnityEngine;

public class InteractableInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;

    private void Start()
    {
        InteractManager.Instance.OnInteractable += UpdateInfo;

        UpdateInfo(null);
    }

    private void UpdateInfo(IInteractable interactable)
    {
        if (interactable != null)
        {
            if (interactable is NpcObject npc)
            {
                infoText.text = $"[F] {npc.NPC_Name}";
            }

            infoText.enabled = true;
        }
        else
        {
            infoText.enabled = false;
        }
        Debug.Log("Trigger");
    }
}
