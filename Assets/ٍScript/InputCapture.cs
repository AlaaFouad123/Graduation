using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputCapture : MonoBehaviour
{
    public TMP_InputField inputEmail;
    public TMP_InputField inputPass;
    public Image BorderEmail;
    public Image BorderPass;
    private string userEmail;
    private string userPass;
    bool IsEmailEnter = false;
    public GameObject WrongEmail;
    public GameObject EnterMail;
    public GameObject WrongPass;
    public GameObject mess;
    public TextMeshProUGUI textMess;
    string UserName = "Alaa";
    void Start()
    {
        WrongEmail.SetActive(false);
        EnterMail.SetActive(true);
        WrongPass.SetActive(false);
        mess.SetActive(false);
        if (inputEmail != null)
        {
            inputEmail.onEndEdit.AddListener(OnEndEditEmail);
            if (inputEmail.text.Length > 0)
            {
                EnterMail.SetActive(false);
            }
        }

        if (inputPass != null)
        {
            inputPass.onEndEdit.AddListener(OnEndEditPass);
        }

    }

    void OnEndEditEmail(string input)
    {
        if (inputEmail.text.Length > 0)
        {
            userEmail = input;
            Debug.Log("User Email: " + userEmail);
        }
    }

    void OnEndEditPass(string input)
    {
        if (inputPass.text.Length > 0)
        {
            userPass = input;
            Debug.Log("User Password: " + userPass);
        }
    }


    public string GetUserEmail()
    {
        return userEmail;
    }


    public string GetUserPass()
    {
        return userPass;
    }

    public void Check()
    {
        // email
        if (userEmail == "fouad123@gmail.com")
        {
            IsEmailEnter = true;
            WrongEmail.SetActive(false);
            EnterMail.SetActive(false);
            BorderEmail.color = new Color(0.565f, 0.933f, 0.565f);  // Light Green color using normalized values

        }
        else
        {
            IsEmailEnter = false;
            WrongEmail.SetActive(true);
            EnterMail.SetActive(false);
            BorderEmail.color = new Color(0.5f, 0f, 0.5f);  // Purple color using normalized values


        }


        //Pass
        if (userPass == "000" && IsEmailEnter)
        {
            WrongPass.SetActive(false);
            BorderPass.color = new Color(0.565f, 0.933f, 0.565f);  // Light Green color using normalized values
            mess.SetActive(true);
            textMess.text = $"Hi {UserName}";

        }
        else
        {
            if (IsEmailEnter)
            {
                WrongPass.SetActive(true);
                BorderPass.color = new Color(0.5f, 0f, 0.5f);  // Purple color using normalized values
            }

        }


    }
}
