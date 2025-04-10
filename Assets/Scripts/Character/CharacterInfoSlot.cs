using TMPro;
using UnityEngine;

public class CharacterInfoSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoText;

    public void SetText(string text)
    {
        _infoText.text = text;
    }
}
