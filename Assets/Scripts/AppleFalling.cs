using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AppleFallingUI : MonoBehaviour
{ 
    public GameObject applePrefab; // مرجع لـ UI Image التفاحة
    public RectTransform canvasTransform; // مرجع للـ Canvas الذي يحتوي على التفاح
    public float spawnRate = 1.5f; // معدل توليد التفاح بالثواني
    public float spawnRangeX = 400f; // نطاق التوليد العشوائي للتفاح في الـ UI
    void Start()
    {
        InvokeRepeating("SpawnApple", 1f, spawnRate); // توليد التفاح بشكل متكرر
    }

    void SpawnApple()
    {
        float posX = Random.Range(-spawnRangeX, spawnRangeX); // توليد X بشكل عشوائي
        Vector3 spawnPosition = new Vector3(posX, canvasTransform.rect.height / 2, 0); // التفاح يسقط من الأعلى
        GameObject newApple = Instantiate(applePrefab, canvasTransform);
        RectTransform appleRect = newApple.GetComponent<RectTransform>();
        appleRect.anchoredPosition = spawnPosition;
        newApple.AddComponent<AppleUIDropper>(); // إضافة سكريبت لحذف التفاح عند خروجه من الشاشة
    }
}

// سكريبت لحذف التفاحة عند خروجها من الشاشة في UI
public class AppleUIDropper : MonoBehaviour
{
    public float fallSpeed = 770f; // سرعة سقوط التفاح في الـ UI

    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition -= new Vector2(0, fallSpeed * Time.deltaTime);
        
        if (rectTransform.anchoredPosition.y < -Screen.height / 3) // إذا خرجت من الشاشة
        {
            Destroy(gameObject);
        }
    }
}
