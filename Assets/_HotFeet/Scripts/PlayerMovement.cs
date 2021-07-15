using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputAction inputAction;
    private CharacterController controller;
    [SerializeField] private float movementSpeed;
    
    private void OnEnable() => inputAction.Enable();
    private void OnDisable() => inputAction.Disable();
    void Start() => controller = GetComponent<CharacterController>();
    
    void Update() => Movement();
    
    private void Movement()
    {
        var input = inputAction.ReadValue<Vector2>();
        var output = new Vector3(input.x, 0, input.y);

        if (!controller.isGrounded)
            output += Physics.gravity;

        controller.Move(output * Time.deltaTime * movementSpeed);
    }
}