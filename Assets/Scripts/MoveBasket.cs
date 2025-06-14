using UnityEngine;

public class MoveBasket : MonoBehaviour
{
    public float speed = 6f;
    private Camera mainCamera;
    private float screenHalfWidthInWorldUnits;

    private void Start()
    {
        mainCamera = Camera.main;
        float halfBasketWidth = transform.localScale.x / 2f;
        screenHalfWidthInWorldUnits = mainCamera.orthographicSize * mainCamera.aspect - halfBasketWidth;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // First touch (index 0)
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
            Vector3 targetPosition = new Vector3(touchPosition.x, transform.position.y, transform.position.z);

            // Clamp to screen bounds
            targetPosition.x = Mathf.Clamp(targetPosition.x, -screenHalfWidthInWorldUnits, screenHalfWidthInWorldUnits);

            // Smoothly move the basket
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
