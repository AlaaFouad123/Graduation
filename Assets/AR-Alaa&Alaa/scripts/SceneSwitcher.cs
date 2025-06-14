using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject FirstScene;
    public GameObject SecondScene;
    void Start()
    {
        FirstScene.SetActive(true);
        SecondScene.SetActive(false);
    }

    public void ChangeScene()
    {
        FirstScene.SetActive(false);
        SecondScene.SetActive(true);
    }
}
