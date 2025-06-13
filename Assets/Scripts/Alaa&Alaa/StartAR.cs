using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAR : MonoBehaviour
{
	public TMP_Text levelName;
	public ScoreManager scoreManager;
	public ARSetupInOtherLevels aRSetupInOtherLevels;


	void Start()
	{
		Debug.Log(LevelManager.Level);
		levelName.text = LevelManager.Level;

		var StudentId = SharedPrefManager.GetData<string>("studentId");
		Debug.Log($"id from shared pred {StudentId}");
		scoreManager.GetStudentScore(int.Parse(StudentId));


	}

	//public void StartLevelWithAR()
	//{
	//	if (!ARInstance.arAlreadyInstantiated && ARInstance.arSetupPrefab == null)
	//	{
	//		Debug.Log("setup from start ar");
	//		aRSetupInOtherLevels.SetupAR();
	//	}
	//	var level = LevelManager.Level;

	//	switch (level)
	//	{
	//		case "Level 1":
	//			SceneManager.LoadScene("A");
	//			break;
	//		case "Level 2":
	//			SceneManager.LoadScene("B");
	//			break;
	//		case "Level 3":
	//			SceneManager.LoadScene("C");
	//			break;
	//		case "Level 4":
	//			SceneManager.LoadScene("D");
	//			break;
	//	}
	//}

	public void StartLevelWithAR()
	{
		var oldCamera = GameObject.Find("2DCamera");
		if (oldCamera != null)
		{
			Debug.Log("Disabling 2D camera: " + oldCamera.name);
			oldCamera.SetActive(false);
		}

		var ev = GameObject.Find("2DEventSystem");
		if (ev != null)
		{
			Debug.Log("Disabling 2D 2DEventSystem: " + ev.name);
			oldCamera.SetActive(false);
		}

		//if (!ARInstance.arAlreadyInstantiated && ARInstance.arSetupPrefab == null)
		//{
		//	//Debug.Log("AR not ready — setting up and waiting...");
		//	//aRSetupInOtherLevels.SetupAR(() =>
		//	//{
		//		//Debug.Log("AR setup complete. Loading scene...");
		//		LoadLevelScene();
		//	//});
		//}
		//else
		//{
		Debug.Log("AR already ready. Loading scene immediately.");
		LoadLevelScene();
		//}
	}

	private void LoadLevelScene()
	{
		switch (LevelManager.Level)
		{
			case "Level 1": SceneManager.LoadScene("A", LoadSceneMode.Additive); break;
			case "Level 2": SceneManager.LoadScene("B"); break;
			case "Level 3": SceneManager.LoadScene("C"); break;
			case "Level 4": SceneManager.LoadScene("D"); break;
			default: Debug.LogWarning("Unknown level: " + LevelManager.Level); break;
		}
	}

}
