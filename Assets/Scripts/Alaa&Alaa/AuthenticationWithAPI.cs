using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthenticationWithAPI : MonoBehaviour
{
	public string BaseAPIURL = "https://timonoapp.runasp.net";
	public TMP_InputField EmailInputField;
	public TMP_InputField PasswordInputField;
	public TMP_Text EmailError;
	public TMP_Text PasswordError;

	public void Login()
	{
		var user = new UserLogin
		{
			email = EmailInputField.text,
			password = PasswordInputField.text
		};

		StartCoroutine(SendLoginRequest(user));
	}

	public IEnumerator SendLoginRequest(UserLogin user)
	{
		// Convert to JSON
		string jsonData = JsonUtility.ToJson(user);

		// Create web request
		using (UnityWebRequest request = new UnityWebRequest($"{BaseAPIURL}/Login", "POST"))
		{
			byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
			request.uploadHandler = new UploadHandlerRaw(jsonToSend);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{

				Debug.LogError("Login failed: " + request.downloadHandler.text);

				if (request.downloadHandler.text.Contains("email"))
				{
					EmailError.text = "";
					EmailError.text = "Invalid email address.";
				}
				else if (request.downloadHandler.text.Contains("password"))
				{
					PasswordError.text = "Invalid password.";
				}
				else
				{
					EmailError.text = "Login failed. Please try again.";
					PasswordError.text = "";
				}
			}
			else
			{
				Debug.Log("Login response: " + request.downloadHandler.text);
				EmailError.text = "";
				PasswordError.text = "";
				SceneManager.LoadScene("SkipSub");
			}
		}
	}

}

public class UserLogin
{
	public string email;
	public string password;
}
