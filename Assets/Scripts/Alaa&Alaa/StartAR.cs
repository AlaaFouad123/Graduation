using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAR : MonoBehaviour
{
	public TMP_Text levelName;

	void Start()
	{
		Debug.Log(LevelManager.Level);
		levelName.text = LevelManager.Level;
	}

	public void StartLevelWithAR()
	{
		var level = LevelManager.Level;
		switch (level)
		{
			case "Level 1":
				SceneManager.LoadScene("A");
				break;
			case "Level 2":
				SceneManager.LoadScene("B");
				break;
			case "Level 3":
				SceneManager.LoadScene("C");
				break;
			case "Level 4":
				SceneManager.LoadScene("D");
				break;
		}
	}


}
