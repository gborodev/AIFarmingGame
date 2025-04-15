using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInfoSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<CharacterInfoSlot> OnSlotEnter;
    public event Action<CharacterInfoSlot> OnSlotExit;

    [SerializeField] private TextMeshProUGUI _infoText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSlotEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnSlotExit?.Invoke(this);
    }

    public void SetText(string text)
    {
        _infoText.text = text;
    }
}
