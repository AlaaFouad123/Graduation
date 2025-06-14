using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Profile : MonoBehaviour
{
	public string BaseAPIURL = "https://timonoapp.runasp.net/XR_Timono_App";

	private int studentId;
	private UserData userData;

	public TMP_Text UserName;
	public TMP_Text Email;
	public TMP_Text PhoneNumber;

	private void Start()
	{
		studentId = int.Parse(SharedPrefManager.GetData<string>("studentId").ToString());
		GetUserData(() =>
		{
			UserName.text = userData.name;
			Email.text = userData.email;
			PhoneNumber.text = userData.phoneNumber;
		});
	}


	public void GetUserData(Action Oncomplete)
	{
		StartCoroutine(GetUserData(studentId, Oncomplete));
	}
	public IEnumerator GetUserData(int studentId, Action Oncomplete)
	{
		string url = $"{BaseAPIURL}/Student/{studentId}";
		Debug.Log(url);
		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{

			yield return request.SendWebRequest();
			if (request.result == UnityWebRequest.Result.Success)
			{
				string json = request.downloadHandler.text;
				Debug.Log($"open levels: {json}");

				userData = JsonUtility.FromJson<UserData>(json);

				Debug.Log($"Name: {userData.name}");
				Debug.Log($"Email: {userData.email}");
				Debug.Log($"Phone: {userData.phoneNumber}");

				Oncomplete();
			}
			else
			{
				Debug.LogError($"Failed to get user data. Error: {request.error}");
			}
		}
	}
}

[System.Serializable]
public class UserData
{
	public string name;
	public string email;
	public string phoneNumber;

}
