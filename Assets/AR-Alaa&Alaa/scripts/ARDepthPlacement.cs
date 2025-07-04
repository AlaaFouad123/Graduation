﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SendARFrame : MonoBehaviour
{
	[SerializeField] private string apiUrl = "https://Esraa101-ModelML.hf.space/process_frame/";
	[SerializeField] public GameObject objectPrefab;
	[SerializeField] private ARRaycastManager raycastManager;
	public AudioClip? lastAudio;
	public AudioSource audioSource;




	private List<ARRaycastHit> hits = new();
	private List<GameObject> placedObjects = new();
	private Coroutine processingCoroutine;

	private bool isSending = false;
	private bool foundResponse = false;
	private int requestCount = 0;

	public Button Refresh;


	void OnEnable()
	{
		var test = FindFirstObjectByType<XROrigin>();
		if (test == null)
		{
			Debug.LogError("XROrigin not found in the scene. Please ensure it is present.");
			return;
		}
		//if (raycastManager == null)
		raycastManager = test.GetComponent<ARRaycastManager>();

		processingCoroutine = StartCoroutine(ProcessFramesUntilResponse());
		Debug.Log("AR Frame Processing Started");



		//if (Refresh == null)
		//{
		//	Refresh = GameObject.Find("Refresh")?.GetComponent<Button>();
		//}
		if (Refresh != null)
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

					StartCoroutine(RaycastAndPlace(screenPoint));
					return;
				}
			}
		}
	}
	//c 
	IEnumerator RaycastAndPlace(Vector2 screenPoint)
	{
		Debug.Log(raycastManager == null);
		if (raycastManager.Raycast(screenPoint, hits, TrackableType.Planes))
		{
			Pose hitPose = hits[0].pose;

			if (objectPrefab != null)
			{
				Quaternion rotation = Quaternion.LookRotation(
					new Vector3(Camera.main.transform.position.x - hitPose.position.x, 0, Camera.main.transform.position.z - hitPose.position.z)
				);

				GameObject placedObj = Instantiate(objectPrefab, hitPose.position, rotation);
				if (audioSource != null)
				{
					Debug.Log("play");
					PlaylastAudio();
				}
				placedObjects.Add(placedObj);

				if (CheckLevels() && !placedObj.gameObject.name.Contains("Robot"))//a //robot
				{
					yield return new WaitForSeconds(15f);
					placedObj.SetActive(false);
					LoadDialog();

				}



			}
		}
		else
		{
			Debug.LogWarning("⚠️ No plane detected. Try pointing the camera at the ground.");
			Refresh.gameObject.SetActive(true);

		}
	}

	private bool CheckLevels()
	{
		var controllerObject = GameObject.Find("ModelsController");
		if (controllerObject != null)
		{
			InputHandler controller = controllerObject.GetComponent<InputHandler>();
			if (controller != null)
			{
				List<GameObject> models = controller.models;
				if (models[0].name.ToLower().StartsWith("c") || models[0].name.ToLower().StartsWith("d"))
				{
					return true;
				}
				return false;

			}
			else
			{
				Debug.LogError("ModelsController script not found on GameObject.");
				return false;
			}
		}
		return false;
	}

	private void LoadDialog()
	{
		SceneData.previousSceneName = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene("dialog");

	}
	void PlaylastAudio()
	{
		if (lastAudio != null)
		{
			//yield return new WaitForSeconds(2);
			audioSource.PlayOneShot(lastAudio);
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