using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private InputActionReference mouseInput;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private Transform cameraObj;

    private Vector3 mouseDelta;
    private Vector3 movement;
    private Vector3 velocity;

    private float mouseX;
    private float mouseY;
    private float mouseSensitivity = 25f;
    private float movementSpeed = 350f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        mouseInput.action.Enable();
        moveInput.action.Enable();

        mouseInput.action.performed += CalculateMouse;
        mouseInput.action.canceled += CalculateMouse;

        moveInput.action.performed += CalculateMove;
        moveInput.action.canceled += CalculateMove;
    }
    private void OnDisable()
    {
        mouseInput.action.Disable();
        moveInput.action.Disable();
    }

    private void Update()
    {
        mouseX += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        mouseY += -mouseDelta.y * mouseSensitivity * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -80, 80);

        velocity = (movement.x * transform.right) + (movement.y * transform.forward);
        velocity *= movementSpeed;

        cameraObj.localRotation = Quaternion.Euler(new Vector3(mouseY, 0));
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = velocity * Time.fixedDeltaTime;
        rb.angularVelocity = new Vector3(0, mouseDelta.x) * mouseSensitivity * Time.fixedDeltaTime;
    }

    private void CalculateMouse(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void CalculateMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
}
