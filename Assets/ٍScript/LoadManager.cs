using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadManager : MonoBehaviour
{
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}


	public void LoadStartAR(string levelName)
	{
		LevelManager.Level = levelName;
		SceneManager.LoadScene("StartAR");

	}

}

public static class LevelManager
{
	public static string Level { get; set; }
}
