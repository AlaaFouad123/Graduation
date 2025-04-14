using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using Unity.Tutorials.Core.Editor;
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
        resetErrors();

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

    public void resetErrors()
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
            EmailError.text = "email is required";
        }

        if (!Validation.CheckUserName(user.username))
        {
            UserNameError.text = "username is required,at least 4 charcters";
        }

        if (!Validation.CheckPassword(user.password))
        {
            PasswordError.text = "password is required";
        }

        if (!Validation.CheckPhone(user.phoneNumber))
        {
            PhoneError.text = "phone is required";
        }
    }

    public async Task SendRegisterRequest(UserRegister user)
    {
        string jsonData = JsonUtility.ToJson(user);

        // Create web request
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
                SceneManager.LoadScene("Sign in");
            }
        }
    }

    public void HandleErrors(UnityWebRequest request)
    {
        Debug.LogError("Registeration failed: " + request.downloadHandler.text);

        if (request.downloadHandler.text.Contains("email", StringComparison.InvariantCultureIgnoreCase))
        {
            EmailError.text = "";
            EmailError.text = request.downloadHandler.text
                .TrimStart('"', '[')
                .TrimEnd('"', ']');
        }
        if (request.downloadHandler.text.Contains("password", StringComparison.InvariantCultureIgnoreCase))
        {
            var error = request.downloadHandler.text;
            PasswordError.text = error
                .TrimStart(" '\"' [ register failed : ".ToCharArray())
                .TrimEnd('"', ']')
                .Split(',')[0];
        }
        if (request.downloadHandler.text.Contains("phone", StringComparison.InvariantCultureIgnoreCase))
        {
            PhoneError.text = "Invalid phone number.";
        }
        if (request.downloadHandler.text.Contains("username", StringComparison.InvariantCultureIgnoreCase))
        {
            UserNameError.text = "Username Already Taken,try again";
        }

        else
        {
            EmailError.text = "Registeration failed. Please try again.";
            PasswordError.text = "";
        }

    }
}



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

public class Validation
{
    public static bool IsValid(UserRegister user)
    {
        return CheckUserName(user.username) &&
               CheckEmail(user.email) &&
               CheckPhone(user.phoneNumber) &&
               CheckPassword(user.password);
    }


    public static bool CheckEmail(string email) => email.IsNotNullOrEmpty();
    public static bool CheckPassword(string password) => password.IsNotNullOrEmpty();
    public static bool CheckPhone(string phone) => phone.IsNotNullOrEmpty();
    public static bool CheckUserName(string username) =>
        username.IsNotNullOrEmpty() && Regex.IsMatch(username, @"[a-zA-Z]{4}");




}


