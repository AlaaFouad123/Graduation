using UnityEngine;
using UnityEngine.InputSystem; // Import new Input System

public class ARCameraMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public float lookSpeed = 2.0f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    void Update()
    {
        // Get movement input
        if (Keyboard.current != null)
        {
            moveInput = new Vector2(
                (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
            );

            // Move Forward/Backward/Left/Right
            Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
            transform.position += moveDirection * speed * Time.deltaTime;
        }

        // Get mouse look input
        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            lookInput = Mouse.current.delta.ReadValue() * lookSpeed * Time.deltaTime;

            // Rotate Camera
            transform.Rotate(Vector3.up * lookInput.x);
            transform.Rotate(Vector3.right * -lookInput.y);
        }
    }
}
