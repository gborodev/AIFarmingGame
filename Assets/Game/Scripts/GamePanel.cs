using UnityEngine;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Transform _panelContent;

    public virtual void Open()
    {
        _panelContent.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        _panelContent.gameObject.SetActive(false);
    }
}
