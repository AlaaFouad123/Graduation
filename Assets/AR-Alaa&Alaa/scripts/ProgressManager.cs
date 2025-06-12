using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ProgressManager : MonoBehaviour
{
	public string BaseAPIURL = "https://timonoapp.runasp.net/XR_Timono_App";


	public void UpdateStudentProgress(int stdId, int levelId)
	{
		StartCoroutine(UpdateStudentProgressCoroutine(stdId, levelId));
	}

	private IEnumerator UpdateStudentProgressCoroutine(int stdId, int levelId)
	{

		string url = $"{BaseAPIURL}/Progress/AddProgress?studentId={stdId}&lessonId={levelId}&progress={100}";

		using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, ""))
		{
			request.downloadHandler = new DownloadHandlerBuffer();

			yield return request.SendWebRequest();
			if (request.result == UnityWebRequest.Result.Success)
			{
				Debug.Log("progress updated successfully");
			}
			else
			{
				Debug.LogError($"Failed to update progress. Error: {request.error}");
			}
		}
	}

	public void getStudentCompletedLevels(int stdId, int subjectId, Action<int> onComplete)
	{
		StartCoroutine(getProgressCoroutine(stdId, subjectId, onComplete));
	}

	private IEnumerator getProgressCoroutine(int stdId, int subjectId, Action<int> onComplete)
	{
		string url = $"{BaseAPIURL}/Progress/SpecificProgress/{stdId}/{subjectId}";
		Debug.Log(url);
		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{

			yield return request.SendWebRequest();
			if (request.result == UnityWebRequest.Result.Success)
			{
				Debug.Log($"open levels: {request.downloadHandler.text}");
				onComplete(int.Parse(request.downloadHandler.text));
			}
			else
			{
				Debug.LogError($"Failed to get progress. Error: {request.error}");
			}
		}
	}
}
