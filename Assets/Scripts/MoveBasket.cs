using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBasket : MonoBehaviour
{
    public float speed = 6f;
    private float moveInput;

    private void Update()
    {
        // تحريك السلة بناءً على الإدخال
        transform.Translate(new Vector3(moveInput, 0, 0) * speed * Time.deltaTime);
    }

    // دالة لالتقاط الإدخال من Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>(); // سيعطي -1 عند الضغط على اليسار، 1 عند الضغط على اليمين، و 0 عند عدم الضغط
    }
}
