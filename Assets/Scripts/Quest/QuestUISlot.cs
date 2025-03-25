using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestUISlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _questName;

    public event Action<QuestUISlot> OnClick;

    public void Set(string questName)
    {
        _questName.text = questName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}
