using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueLevels : MonoBehaviour
{

	public void GoToNextLevel()
	{
		var dialogScene = SceneManager.GetSceneByName("dialog");
		var currentScene = SceneData.previousSceneName;
		Debug.Log(currentScene);
		Debug.Log(dialogScene.name);

		switch (currentScene)
		{
			case "A":
				SceneManager.LoadScene("B", LoadSceneMode.Additive);
				SceneManager.UnloadSceneAsync(dialogScene);

				break;
			case "B":

				SceneManager.LoadScene("C", LoadSceneMode.Additive);
				SceneManager.UnloadSceneAsync(dialogScene);

				break;
			case "C":
				//SceneManager.GetSceneByName("C")
				SceneManager.LoadScene("D", LoadSceneMode.Additive);
				SceneManager.UnloadSceneAsync(dialogScene);

				break;
				//case "D":
				//    SceneManager.LoadScene("LevelsScene");
				//    break;
		}
	}


	private void updateProgress()
	{

	}

}
