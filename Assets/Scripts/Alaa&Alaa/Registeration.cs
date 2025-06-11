using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Registeration : MonoBehaviour
{
    public string BaseAPIURL = "https://timonoapp.runasp.net";
    public TMP_InputField EmailInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PhoneInputField;
    public TMP_InputField UserNameInputField;
    public TMP_Text EmailError;
    public TMP_Text PasswordError;
    public TMP_Text PhoneError;
    public TMP_Text UserNameError;
    public GameObject loadingIndicator;
    public GameObject RegisterButton;

    private void Start()
    {
        loadingIndicator.SetActive(false);
    }

    public async void OnRegisterButtonClicked()
    {
        loadingIndicator.SetActive(true);
        RegisterButton.SetActive(false);

        await Register();

        loadingIndicator.SetActive(false);
        RegisterButton.SetActive(true);
    }

    public async Task Register()
    {
        ResetErrors();

        var user = new UserRegister
        {
            email = EmailInputField.text,
            password = PasswordInputField.text,
            username = UserNameInputField.text,
            phoneNumber = PhoneInputField.text
        };

        if (Validation.IsValid(user))
        {
            await SendRegisterRequest(user);
        }
        else
        {
            ValidateInput(user);
        }
    }

    public void ResetErrors()
    {
        UserNameError.text = "";
        EmailError.text = "";
        PhoneError.text = "";
        PasswordError.text = "";
    }

    public void ValidateInput(UserRegister user)
    {
        if (!Validation.CheckEmail(user.email))
        {
            EmailError.text = "Email is required.";
        }

        if (!Validation.CheckUserName(user.username))
        {
            UserNameError.text = "Username is required (at least 4 characters).";
        }

        if (!Validation.CheckPassword(user.password))
        {
            PasswordError.text = "Password is required.";
        }

        if (!Validation.CheckPhone(user.phoneNumber))
        {
            PhoneError.text = "Phone number is required.";
        }
    }

    public async Task SendRegisterRequest(UserRegister user)
    {
        string jsonData = JsonUtility.ToJson(user);

        using (UnityWebRequest request = new UnityWebRequest($"{BaseAPIURL}/Register", "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleErrors(request);
            }
            else
            {
                SceneManager.LoadScene("Sign in"); // Make sure the scene is added to Build Settings
            }
        }
    }

    public void HandleErrors(UnityWebRequest request)
    {
        Debug.LogError("Registration failed: " + request.downloadHandler.text);
        string response = request.downloadHandler.text;

        if (response.Contains("email", StringComparison.InvariantCultureIgnoreCase))
        {
            EmailError.text = response.Trim('"', '[', ']');
        }

        if (response.Contains("password", StringComparison.InvariantCultureIgnoreCase))
        {
            PasswordError.text = response
                .Trim('"', '[', ']')
                .Split(',')[0];
        }

        if (response.Contains("phone", StringComparison.InvariantCultureIgnoreCase))
        {
            PhoneError.text = "Invalid phone number.";
        }

        if (response.Contains("username", StringComparison.InvariantCultureIgnoreCase))
        {
            UserNameError.text = "Username already taken. Try again.";
        }

        if (string.IsNullOrEmpty(EmailError.text) && string.IsNullOrEmpty(UserNameError.text))
        {
            EmailError.text = "Registration failed. Please try again.";
        }
    }
}

[Serializable]
public class UserRegister
{
    public string email;
    public string password;
    public string phoneNumber;
    public string username;

    public override string ToString()
    {
        return $"{email}\n{username}\n{phoneNumber}\n{password}";
    }
}

public static class Validation
{
    public static bool IsValid(UserRegister user)
    {
        return CheckUserName(user.username) &&
               CheckEmail(user.email) &&
               CheckPhone(user.phoneNumber) &&
               CheckPassword(user.password);
    }

    public static bool CheckEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public static bool CheckPassword(string password) =>
        !string.IsNullOrWhiteSpace(password) && password.Length >= 6;

    public static bool CheckPhone(string phone) =>
        !string.IsNullOrWhiteSpace(phone) && Regex.IsMatch(phone, @"^[0-9]{10,15}$");

    public static bool CheckUserName(string username) =>
        !string.IsNullOrWhiteSpace(username) && username.Length >= 4;
}
