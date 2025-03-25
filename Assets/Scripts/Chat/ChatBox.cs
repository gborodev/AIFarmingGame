using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
    [SerializeField] private RectTransform _chatContext;
    [SerializeField] private TextMeshProUGUI _textPrefab;

    private const int MAX_TEXT_SIZE = 15;

    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();

    private void Start()
    {
        AddText("Game started.");
    }

    public void AddText(string text)
    {
        TextMeshProUGUI newText = Instantiate(_textPrefab, _chatContext);

        newText.text = text;

        if (textList.Count >= MAX_TEXT_SIZE)
        {
            TextMeshProUGUI outText = textList[^1];
            textList.Remove(outText);
            Destroy(outText.gameObject);
        }

        _chatContext.anchoredPosition = Vector3.zero;

        textList.Add(newText);
    }
}
