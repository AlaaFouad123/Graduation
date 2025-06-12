
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
	public string BaseAPIURL = "https://timonoapp.runasp.net/XR_Timono_App";
	[SerializeField] TMP_Text CurrentScore;


	public void UpdateStudentScore(int stdId, int score)
	{
		StartCoroutine(UpdateStudentScoreCoroutine(stdId, score));
	}

	private IEnumerator UpdateStudentScoreCoroutine(int stdId, int score)
	{
		var newScore = score + 10;
		string url = $"{BaseAPIURL}/Student/updateScore/{stdId}/{newScore}";
		Debug.Log(score + "  ttttt" + " " + url);

		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
			{
				Debug.Log("Student score: " + request.downloadHandler.text);
				SharedPrefManager.SaveData("score", newScore);
				CurrentScore.text = newScore.ToString();
			}
			else
			{
				Debug.LogError($"Failed to update score. Error: {request.error}");
			}
		}
	}

	public void GetStudentScore(int stdId)
	{
		StartCoroutine(GetStudentScoreCoroutine(stdId));
	}

	private IEnumerator GetStudentScoreCoroutine(int stdId)
	{
		string url = BaseAPIURL + $"/Student/score/{stdId}";
		Debug.Log("Fetching score from URL: " + url);

		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
			{
				Debug.Log("Student score: " + request.downloadHandler.text);
				SharedPrefManager.SaveData("score", int.Parse(request.downloadHandler.text));
			}
			else
			{
				Debug.LogError($"Failed to get score. Error: {request.error}");
			}
		}
	}

}
