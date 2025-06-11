using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
	public GameObject? lastModel;
	public List<GameObject> models;
	public List<Vector3> stopPositions;
	public List<AudioClip> audioClips;
	public AudioClip? lastAudio;
	public TMP_Text displayText;
	public GameObject WriteWords;
	public apple2Falling tree;
	public AudioClip tryAgainAudioClip;
	public AudioClip cheerAudioClip;

	public Vector3 rotationVector;
	public float moveSpeed = 2.0f;

	[SerializeField] TMP_InputField inputField;
	[SerializeField] TMP_Text resultText;

	private int currentModelIndex = 0;
	private bool isMoving = false;

	public Button submitButton;

	public RobotAudio robotAudio;

	public AudioSource audioSource;

	public Image BorderEmail;

	[SerializeField] private XROrigin xrOrigin;





	void Start()
	{
		if (lastModel != null)
		{
			lastModel.SetActive(false);
		}

		HideInputField();
		if (models.Count > 0)
		{
			models[0].SetActive(true);
			StartAudio();
		}

		for (int i = 1; i < models.Count; i++)
		{
			models[i].SetActive(false);
		}
		submitButton.onClick.AddListener(ValidateInput);

	}

	public void ValidateInput()
	{
		string input = inputField.text.Trim().ToLower();

		if (currentModelIndex < models.Count && input == models[currentModelIndex].name.ToLower())
		{

			PlayCheerSound();

			HideInputField();
			isMoving = true;
		}
		else
		{
			BorderEmail.color = new Color(1f, 0f, 0f);
			PlayTryAgainSound();
		}
	}

	void Update()
	{
		if (isMoving && currentModelIndex < models.Count)
		{
			GameObject currentModel = models[currentModelIndex];

			Vector3 stopPosition = stopPositions[currentModelIndex];

			currentModel.transform.Rotate(rotationVector * Time.deltaTime);

			if (Vector3.Distance(currentModel.transform.position, stopPosition) > 0.01f)
			{
				currentModel.transform.position = Vector3.MoveTowards(currentModel.transform.position,
					stopPosition, moveSpeed * Time.deltaTime);
			}
			else
			{
				currentModel.transform.position = stopPosition;
				isMoving = false;
				StartCoroutine(ShowNextModel());
			}

		}
	}

	IEnumerator ShowNextModel()
	{
		StartCoroutine(HideAllModel());
		currentModelIndex++;
		if (currentModelIndex < models.Count)
		{
			models[currentModelIndex].SetActive(true);

			resultText.text = models[currentModelIndex].name;
			resultText.color = Color.white;

			StartAudio();
		}
		else
		{
			inputField.transform.parent.gameObject.SetActive(false);
			resultText.text = "";
			//if (lastModel != null)
			//{
			yield return new WaitForSeconds(2);
			//	lastModel.SetActive(true);
			//	PlaylastAudio();
			//}
		}
	}

	void PlaylastAudio()
	{
		if (lastAudio != null)
		{
			//yield return new WaitForSeconds(2);
			audioSource.PlayOneShot(lastAudio);
		}

	}

	void PlayCheerSound()
	{
		if (cheerAudioClip != null)
		{
			audioSource.PlayOneShot(cheerAudioClip);
		}
	}

	void PlayTryAgainSound()
	{
		if (tryAgainAudioClip != null)
		{
			audioSource.PlayOneShot(tryAgainAudioClip);
		}
	}


	public void StartAudio()
	{
		StartCoroutine(PlayAudioWithDelay());
	}

	private IEnumerator PlayAudioWithDelay()
	{
		yield return new WaitForSeconds(2);

		if (currentModelIndex < audioClips.Count)
		{
			robotAudio.startAudio(
				audioClips[currentModelIndex],
				() =>
				{
					ClearText();
					ShowInputField();
					BorderEmail.color = Color.white;
				}
			);
		}
	}


	void ClearText()
	{
		if (displayText != null)
		{
			displayText.text = "";
		}
	}

	void ShowInputField()
	{
		if (WriteWords != null)
			WriteWords.gameObject.SetActive(true);
	}

	void HideInputField()
	{
		if (WriteWords != null)
		{
			WriteWords.gameObject.SetActive(false);
			inputField.text = string.Empty;
		}
	}
	IEnumerator HideAllModel()
	{
		if (currentModelIndex == (models.Count - 1))
		{
			yield return new WaitForSeconds(2);
			for (int i = 0; i < models.Count; i++)
			{
				models[i].SetActive(false);
			}
			if (lastModel != null)
			{
				Debug.Log("last model" + lastModel.name);
				lastModel.SetActive(true);
			}
			if (models[0].name.ToLower().StartsWith("a"))
			{
				Debug.Log("tree logic running...");

				var sendAR = lastModel.GetComponent<SendARFrame>();
				Transform treeWithApples = null;

				if (sendAR != null)
				{
					float timer = 0f;
					float timeout = 120f;
					float checkInterval = 1f; // seconds


					while (treeWithApples == null && timer < timeout)
					{
						var lastPlaced = sendAR.GetLastPlacedObject();
						if (lastPlaced != null)
						{
							treeWithApples = lastPlaced.transform;
							break;
						}

						yield return new WaitForSeconds(checkInterval);
						timer += checkInterval;
					}

					if (treeWithApples != null)
					{
						tree.SetTreeAndInitialize(treeWithApples);
						tree.StartAppleFall();
						yield return new WaitForSeconds(10);
						treeWithApples.gameObject.SetActive(false);
					}
					else
					{
						Debug.LogWarning("Timed out waiting for last placed object.");
					}
				}
				else
				{
					Debug.LogError("SendARFrame not found on lastModel.");
				}


			}
			StartCoroutine(DialogScene());

		}

	}
	IEnumerator DialogScene()
	{

		if (models[0].name.ToLower().StartsWith("b"))
		{
			PlaylastAudio();
			yield return new WaitForSeconds(10);
			lastModel.SetActive(false);
		}
		else
		{

			if (lastModel != null && !models[0].name.ToLower().StartsWith("a"))
			{
				SendARFrame sender = lastModel.GetComponent<SendARFrame>();

				if (sender != null)
				{
					GameObject lastObject = null;

					float timeout = 120f;
					float timer = 0f;
					float checkInterval = 1f; // seconds

					while (lastObject == null && timer < timeout)
					{
						lastObject = sender.GetLastPlacedObject();

						if (lastObject == null)
						{
							yield return new WaitForSeconds(checkInterval);
							timer += checkInterval;
						}
					}

					if (lastObject != null)
					{
						Debug.Log("✅ Object placed — ready to proceed");
						PlaylastAudio();
						yield return new WaitForSeconds(10);
						lastObject.SetActive(false);
					}
					else
					{
						Debug.LogWarning("❌ Timed out waiting for object placement");
					}
				}
				else
				{
					Debug.LogError("SendARFrame component missing on lastModel!");
				}
			}
		}


		setSceneData();
		//Debug.Log((xrOrigin == null) + "models");
		//XROriginHolder.xrOrigin = xrOrigin;

		SceneManager.LoadScene("dialog", LoadSceneMode.Additive);
	}

	private void setSceneData()
	{
		if (models[0].name.ToLower().StartsWith("a"))
		{
			Debug.Log("a here");
			SceneData.previousSceneName = "A";
		}
		else if (models[0].name.ToLower().StartsWith("b"))
		{
			SceneData.previousSceneName = "B";
		}
		else if (models[0].name.ToLower().StartsWith("c"))
		{
			SceneData.previousSceneName = "C";
		}
		else if (models[0].name.ToLower().StartsWith("d"))
		{
			SceneData.previousSceneName = "D";
		}
		else
			SceneData.previousSceneName = SceneManager.GetActiveScene().name;
	}




}



public static class SceneData
{
	public static string previousSceneName;

}

//public static class XROriginHolder
//{
//	public static XROrigin xrOrigin;
//}
