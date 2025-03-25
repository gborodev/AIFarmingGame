using UnityEngine;
using UnityEngine.UI;

public class GamePanelController : MonoBehaviour
{
    [SerializeField] private Transform _buttonsContent;
    [SerializeField] private Transform _panelsContent;

    private Button[] _gameplayButtons;
    private GamePanel[] _gameplayPanels;

    private void Awake()
    {
        _gameplayButtons = _buttonsContent.GetComponentsInChildren<Button>(true);
        _gameplayPanels = _panelsContent.GetComponentsInChildren<GamePanel>(true);

        if (_gameplayButtons.Length > _gameplayPanels.Length)
        {
            for (int i = _gameplayPanels.Length; i < _gameplayButtons.Length; i++)
            {
                _gameplayButtons[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < _gameplayButtons.Length; i++)
        {
            int index = i;

            if (index > _gameplayPanels.Length - 1) break;

            Button button = _gameplayButtons[index];
            button.onClick.AddListener(() => SetPanel(index));
        }
    }

    private void Start()
    {
        SetPanel(0);
    }

    private void SetPanel(int index)
    {
        foreach (GamePanel panel in _gameplayPanels)
        {
            panel.Close();
        }

        _gameplayPanels[index].Open();
    }
}
