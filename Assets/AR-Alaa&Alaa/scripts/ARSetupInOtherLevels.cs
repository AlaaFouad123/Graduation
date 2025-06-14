using System;
using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSetupInOtherLevels : MonoBehaviour
{
	public GameObject arSetupPrefab;

	public void SetupAR(Action onComplete = null)
	{
		if (ARInstance.arAlreadyInstantiated)
		{
			Debug.Log("AR setup already exists. Skipping instantiation.");
			onComplete?.Invoke();
			return;
		}

		if (arSetupPrefab == null)
		{
			Debug.LogError("AR Setup Prefab is not assigned in the Inspector.");
			onComplete?.Invoke();
			return;
		}

		try
		{
			GameObject arInstance = Instantiate(arSetupPrefab);
			//EnableARComponentsTemporarily(arSetupPrefab);
			ARInstance.arSetupPrefab = arInstance;
			DontDestroyOnLoad(arInstance);
			ARInstance.arAlreadyInstantiated = true;

			Debug.Log("AR setup instantiated and marked as DontDestroyOnLoad.");

			// Start coroutine to wait for camera and call the callback
			StartCoroutine(WaitForCameraThenComplete(onComplete));
		}
		catch (Exception e)
		{
			Debug.LogError("Failed to instantiate AR setup prefab: " + e.Message);
			onComplete?.Invoke();
		}
	}

	private IEnumerator WaitForCameraThenComplete(Action onComplete)
	{
		float timeout = 5f, timer = 0f;
		while (Camera.main == null && timer < timeout)
		{
			Debug.Log("Waiting for Camera.main...");
			yield return new WaitForSeconds(0.25f);
			timer += 0.25f;
		}

		if (Camera.main == null)
			Debug.LogError("Camera.main not found after timeout.");
		else
			Debug.Log("Camera.main is ready.");

		onComplete?.Invoke();
	}

	private void EnableARComponentsTemporarily(GameObject instance)
	{
		var session = instance.GetComponentInChildren<ARSession>();
		if (session) session.enabled = true;

		var origin = instance.GetComponentInChildren<XROrigin>();
		if (origin) origin.enabled = true;

		Debug.Log("enabled ARSession and XROrigin.");
	}
}

public static class ARInstance
{
	public static bool arAlreadyInstantiated = false;
	public static GameObject? arSetupPrefab = null;
}

