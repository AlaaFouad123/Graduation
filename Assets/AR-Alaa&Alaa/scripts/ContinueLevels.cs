using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueLevels : MonoBehaviour
{
	public ProgressManager progressManager;
	private int levelId;
	public void GoToNextLevel()
	{
		var dialogScene = SceneManager.GetSceneByName("dialog");
		var currentScene = SceneData.previousSceneName;
		Debug.Log(currentScene);
		Debug.Log(dialogScene.name);

		switch (currentScene)
		{
			case "A":
				levelId = 1;
				UpdateProgress();
				SceneManager.LoadScene("B", LoadSceneMode.Additive);
				SceneManager.UnloadSceneAsync(dialogScene);

				break;
			case "B":
				levelId = 2;
				UpdateProgress();
				SceneManager.LoadScene("C", LoadSceneMode.Additive);
				SceneManager.UnloadSceneAsync(dialogScene);

				break;
			case "C":
				levelId = 3;
				UpdateProgress();
				//SceneManager.GetSceneByName("C")
				SceneManager.LoadScene("D", LoadSceneMode.Additive);
				SceneManager.UnloadSceneAsync(dialogScene);

				break;
			case "D":
				levelId = 4;
				UpdateProgress();
				SceneManager.LoadScene("LevelsEnglish");
				break;
		}
	}


	public void UpdateProgress()
	{
		progressManager.UpdateStudentProgress(
			int.Parse(SharedPrefManager.GetData<string>("studentId")),
			levelId
			);
	}

}
