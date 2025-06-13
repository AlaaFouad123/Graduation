using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SendARFrame : MonoBehaviour
{
	[SerializeField] private string apiUrl = "https://Esraa101-ModelML.hf.space/process_frame/";
	[SerializeField] public GameObject objectPrefab;
	[SerializeField] private ARRaycastManager raycastManager;

	private List<ARRaycastHit> hits = new();
	private List<GameObject> placedObjects = new();
	private Coroutine processingCoroutine;

	private bool isSending = false;
	private bool foundResponse = false;
	private int requestCount = 0;

	public Button Refresh;


	void OnEnable()
	{
		StartCoroutine(StartAPI());

	}

	private IEnumerator StartAPI()
	{
		yield return null;
		yield return null;

		XROrigin xrOrigin = null;

		for (int i = 0; i < 5; i++)
		{
			xrOrigin = FindFirstObjectByType<XROrigin>();
			if (xrOrigin != null) break;
			yield return null;
		}

		if (xrOrigin == null)
		{
			Debug.LogError("XROrigin not found after waiting. Cannot continue AR setup.");
			yield break;
		}

		raycastManager = xrOrigin.GetComponent<ARRaycastManager>();
		if (raycastManager == null)
		{
			Debug.LogError("ARRaycastManager not found on XROrigin.");
			yield break;
		}


		processingCoroutine = StartCoroutine(ProcessFramesUntilResponse());
		Debug.Log("AR Frame Processing Started");



		if (Refresh == null)
		{
			Refresh = GameObject.Find("Refresh")?.GetComponent<Button>();
		}
		else ////////edit here
		{
			Refresh.onClick.AddListener(OnRefreshButtonClicked);

			Refresh.gameObject.SetActive(false);
		}
	}

	IEnumerator ProcessFramesUntilResponse()
	{
		while (!foundResponse)
		{
			yield return StartCoroutine(CaptureAndSendFrame());
			yield return new WaitForSeconds(0.5f); // Small delay between frames
		}

		Debug.Log("Valid response received. Stopping frame processing.");
	}

	IEnumerator CaptureAndSendFrame()
	{
		if (foundResponse || isSending)
			yield break;

		yield return new WaitForEndOfFrame();

		Texture2D tex = new(Screen.width, Screen.height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		tex.Apply();

		byte[] imageBytes = tex.EncodeToPNG();
		Destroy(tex);

		yield return SendToAPI(imageBytes);
	}

	IEnumerator SendToAPI(byte[] imageBytes)
	{
		isSending = true;
		requestCount++;

		Debug.Log($"Sending Request #{requestCount}...");

		WWWForm form = new();
		form.AddBinaryData("file", imageBytes, "frame.png", "image/png");

		using UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Post(apiUrl, form);
		request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
		Debug.Log("Sending request to: " + apiUrl);
		yield return request.SendWebRequest();

		if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
		{
			Debug.Log("API Response: " + request.downloadHandler.text);
			foundResponse = true;
			ProcessResponse(request.downloadHandler.text);

			// Stop processing after a successful response
			if (processingCoroutine != null)
			{
				StopCoroutine(processingCoroutine);
				processingCoroutine = null;
			}
		}
		else
		{
			Debug.LogError("❌ API Error: " + request.error);
			Refresh.gameObject.SetActive(true);
		}

		isSending = false;
	}

	public void OnRefreshButtonClicked()
	{
		//Remove old objects
		foreach (var obj in placedObjects)
		{
			Destroy(obj);
		}
		placedObjects.Clear();

		// Allow new API processing
		foundResponse = false;

		if (processingCoroutine != null)
			StopCoroutine(processingCoroutine);

		processingCoroutine = StartCoroutine(ProcessFramesUntilResponse());

		Refresh.gameObject.SetActive(false);
	}

	void ProcessResponse(string responseString)
	{
		try
		{
			var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, int[][]>>(responseString);
			if (jsonResponse != null && jsonResponse.TryGetValue("empty_space", out int[][] maskArray))
			{
				if (maskArray.Length > 0 && maskArray[0].Length > 0)
				{
					PlaceCharacterBasedOnMask(maskArray);
				}
				else
				{
					Debug.LogWarning("⚠️ 'empty_space' is empty.");
					foundResponse = false;
				}
			}
			else
			{
				Debug.LogWarning("⚠️ 'empty_space' key not found in response.");
				foundResponse = false;
			}
		}
		catch (Exception e)
		{
			Debug.LogError("❌ Error processing response: " + e.Message);
			foundResponse = false;
		}
	}

	void PlaceCharacterBasedOnMask(int[][] maskArray)
	{
		Debug.Log("Placing character based on mask.");
		int height = maskArray.Length;
		int width = maskArray[0].Length;
		int step = 20;

		for (int y = 0; y < height; y += step)
		{
			for (int x = 0; x < width; x += step)
			{
				if (maskArray[y][x] == 1)
				{
					float screenX = x * (Screen.width / (float)width);
					float screenY = y * (Screen.height / (float)height);
					Vector2 screenPoint = new(screenX, screenY);

					RaycastAndPlace(screenPoint);
					return;
				}
			}
		}
	}

	void RaycastAndPlace(Vector2 screenPoint)
	{
		Debug.Log(raycastManager == null);
		if (Camera.main == null)
		{
			Debug.LogWarning("⚠️ Camera.main is not available. Waiting for camera initialization.");
			Refresh.gameObject.SetActive(true);
			return;
		}
		if (raycastManager.Raycast(screenPoint, hits, TrackableType.Planes))
		{
			Pose hitPose = hits[0].pose;

			if (objectPrefab != null)
			{
				Quaternion rotation = Quaternion.LookRotation(
					new Vector3(Camera.main.transform.position.x - hitPose.position.x, 0, Camera.main.transform.position.z - hitPose.position.z)
				);

				GameObject placedObj = Instantiate(objectPrefab, hitPose.position, rotation);

				placedObjects.Add(placedObj);

			}
		}
		else
		{
			Debug.LogWarning("⚠️ No plane detected. Try pointing the camera at the ground.");
			Refresh.gameObject.SetActive(true);

		}
	}
	public GameObject GetLastPlacedObject()
	{
		Debug.Log(placedObjects.Count);
		if (placedObjects.Count > 0)
			return placedObjects[placedObjects.Count - 1];
		else
			return null;
	}
}