using UnityEngine;

public interface IInteractable
{
    Transform GetTransform();

    bool CanInteract();
    void Interact();
}
