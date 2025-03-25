using UnityEngine;

public class InputManager : Singleton<InputManager>, InputActions.IPlayerActions
{
    private InputActions input;

    public delegate void MoveInputHandler(Vector2 movementValue);
    public delegate void SprintInputHandler(bool isSprinting);

    public MoveInputHandler OnMoveInput { get; set; }
    public SprintInputHandler OnSprintInput { get; set; }


    private void Start()
    {
        input = new InputActions();

        input.Enable();

        input.Player.AddCallbacks(this);
    }
    private void OnDestroy()
    {
        input.Player.RemoveCallbacks(this);

        input.Dispose();
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    public void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    public void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    public void OnCrouch(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    public void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    public void OnPrevious(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    public void OnNext(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    public void OnSprint(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnSprintInput?.Invoke(context.performed);
    }
}
