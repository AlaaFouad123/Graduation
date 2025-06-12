using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
	public ProgressManager progressManager;
	private int openLevels;
	public List<GameObject> SceneLevels;

	void Start()
	{
		int stdId = int.Parse(SharedPrefManager.GetData<string>("studentId"));
		progressManager.getStudentCompletedLevels(stdId, 1, levels =>
		{
			openLevels = levels;

			for (int i = 1; i < SceneLevels.Count; i++)
			{
				var close1 = SceneLevels[i].transform.Find("Lock 1");
				var close2 = SceneLevels[i].transform.Find("Lock 2");
				var close3 = SceneLevels[i].transform.Find("Lock 3");

				var isColsed = i > openLevels;
				close1.gameObject.SetActive(isColsed);
				close2.gameObject.SetActive(isColsed);
				close3.gameObject.SetActive(isColsed);
			}

		});

	}



}
