using System;
using UnityEngine;

public class InteractManager : Singleton<InteractManager>
{
    public event Action<IInteractable> OnInteractable;

    private IInteractable _interactable;
    private float _interactableDistance = 2f;

    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }


    private void FixedUpdate()
    {
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, _interactableDistance))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
            {
                if (_interactable == null)
                {
                    _interactable = interactable;

                    OnInteractable?.Invoke(_interactable);
                }
            }
        }
        else
        {
            if (_interactable != null)
            {
                _interactable = null;

                OnInteractable?.Invoke(null);
            }
        }
    }
}
