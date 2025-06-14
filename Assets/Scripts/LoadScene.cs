using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    
   

    // Example: Load the scene when this function is called
    public void LoadSceneSkip()
    {
        SceneManager.LoadScene(4);
    }
    public void LoadAppleGame()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadRegister()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadSignin()
    {
        SceneManager.LoadScene(1);
    }
}
