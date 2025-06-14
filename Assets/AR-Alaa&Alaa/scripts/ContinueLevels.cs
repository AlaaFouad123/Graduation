using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueLevels : MonoBehaviour
{

	public ProgressManager progressManager;
	private int levelId;

	private void Start()
	{
		var currentScene = SceneData.previousSceneName;



		switch (currentScene)
		{
			case "A": levelId = 1; break;
			case "B": levelId = 2; break;
			case "C": levelId = 3; break;
			case "D": levelId = 4; break;
		}
	}
	//	private ARSetupInOtherLevels arSretupInotherLevels;
	//	public void GoToNextLevel()
	//	{
	//		var dialogScene = SceneManager.GetSceneByName("dialog");
	//		var currentScene = SceneData.previousSceneName;



	//		switch (currentScene)
	//		{
	//			case "A":
	//				levelId = 1;
	//				UpdateProgress();
	//				SceneManager.LoadScene("B", LoadSceneMode.Additive);
	//				SceneManager.UnloadSceneAsync(dialogScene);

	//				break;
	//			case "B":
	//				levelId = 2;
	//				UpdateProgress();
	//				SceneManager.LoadScene("C", LoadSceneMode.Additive);
	//				SceneManager.UnloadSceneAsync(dialogScene);

	//				break;
	//			case "C":
	//				levelId = 3;
	//				UpdateProgress();
	//				SceneManager.LoadScene("D", LoadSceneMode.Additive);
	//				SceneManager.UnloadSceneAsync(dialogScene);

	//				break;
	//			case "D":
	//				levelId = 4;
	//				UpdateProgress();
	//				SceneManager.LoadScene("LevelsEnglish");
	//				break;
	//		}
	//	}

	//	//public void BackToLevels()
	//	//{
	//	//	var arSretupInotherLevels = FindObjectOfType<ARSetupInOtherLevels>();
	//	//	Debug.Log(arSretupInotherLevels.arSetupPrefab == null);
	//	//	if (arSretupInotherLevels != null)
	//	//	{
	//	//		DestroyPrefab();
	//	//		SceneManager.LoadScene("LevelsEnglish");
	//	//	}
	//	//}
	//	//public void DestroyPrefab()
	//	//{
	//	//	DestroyPrefabCourotine();
	//	//}
	//	//public void DestroyPrefabCourotine()
	//	//{
	//	//	var setupPrefab = GameObject.Find("ARSetup(Clone)");
	//	//	Debug.Log("start Destroying");

	//	//	if (ARInstance.arAlreadyInstantiated && setupPrefab != null)
	//	//	{
	//	//		Debug.Log("Destroying AR Setup ");
	//	//		StopAllCoroutines(); // stops all on this MonoBehaviour
	//	//		Destroy(setupPrefab);
	//	//		Debug.Log(setupPrefab.transform.Find("Main Camera") != null ? "AR Camera Found" : "AR Camera Not Found");
	//	//		//yield return new WaitForEndOfFrame();

	//	//		ARInstance.arSetupPrefab = null;
	//	//		ARInstance.arAlreadyInstantiated = false;
	//	//	}
	//	//}
	//	public void BackToLevels()
	//	{
	//		var arSetupInOtherLevels = FindObjectOfType<ARSetupInOtherLevels>();

	//		if (arSetupInOtherLevels != null && ARInstance.arSetupPrefab != null)
	//		{
	//			StartCoroutine(DestroyPrefabCoroutine(() =>
	//			{
	//				SceneManager.LoadScene("LevelsEnglish");
	//			}));
	//		}
	//		else
	//		{
	//			SceneManager.LoadScene("LevelsEnglish");
	//		}
	//	}


	//	private IEnumerator DestroyPrefabCoroutine(System.Action onComplete)
	//	{
	//		GameObject setupPrefab = ARInstance.arSetupPrefab;

	//		if (setupPrefab != null)
	//		{
	//			Debug.Log("Destroying AR setup prefab...");

	//			// 1. Stop custom coroutines inside AR prefab
	//			foreach (var comp in setupPrefab.GetComponentsInChildren<MonoBehaviour>())
	//			{
	//				comp.StopAllCoroutines();
	//			}

	//			DisableARComponentsTemporarily(setupPrefab);
	//			// 2. Deactivate XR Origin (to avoid Camera.main issues)
	//			//var xrOrigin = setupPrefab.GetComponentInChildren<Unity.XR.CoreUtils.XROrigin>();
	//			//if (xrOrigin != null)
	//			//{
	//			//	xrOrigin.gameObject.SetActive(false);
	//			//}

	//			// 3. Wait one frame to ensure everything stops cleanly
	//			yield return new WaitForEndOfFrame();

	//			// 4. Destroy prefab
	//			//Destroy(setupPrefab);
	//			Debug.Log("AR Setup destroyed.");
	//		}

	//		//ARInstance.arSetupPrefab = null;
	//		//ARInstance.arAlreadyInstantiated = false;

	//		// 5. Load next scene
	//		onComplete?.Invoke();
	//	}

	//	private void DisableARComponentsTemporarily(GameObject instance)
	//	{
	//		var session = instance.transform.Find("AR Session")?.GetComponent<ARSession>();
	//		if (session != null)
	//		{
	//			session.enabled = false;
	//			Debug.Log("ARSession component found and disabled.");
	//		}
	//		else
	//		{
	//			Debug.LogWarning("AR Session not found in the instance.");
	//		}

	//		var origin = instance.transform.Find("XR Origin")?.GetComponent<XROrigin>();
	//		if (origin != null)
	//		{
	//			origin.enabled = false;
	//			Debug.Log("XROrigin component found and disabled.");
	//		}
	//		else
	//		{
	//			Debug.LogWarning("XR Origin not found in the instance.");
	//		}

	//		Debug.Log("Temporarily disabled AR components if found.");
	//	}

	public void UpdateProgress()
	{
		progressManager.UpdateStudentProgress(
			int.Parse(SharedPrefManager.GetData<string>("studentId")),
			levelId
			);
	}


	public void BackToLevels()
	{
		UpdateProgress();
		SceneManager.LoadScene("LevelsEnglish");
	}


}
