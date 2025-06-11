using System.Collections.Generic;
using UnityEngine;

public class apple2Falling : MonoBehaviour
{
	private Transform treeWithApples;
	public float groundY = 0f;
	public float fallSpeed = 0.7f;

	private bool shouldFall = false;
	private List<Transform> apples = new List<Transform>();

	private void Update()
	{
		if (shouldFall)
		{
			foreach (var apple in apples)
			{
				float newY = Mathf.Lerp(apple.position.y, groundY, Time.deltaTime * fallSpeed);
				apple.position = new Vector3(apple.position.x, newY, apple.position.z);
			}
		}
	}

	public void SetTreeAndInitialize(Transform tree)
	{
		treeWithApples = tree;
		apples.Clear();
		foreach (Transform item in treeWithApples)
		{
			Debug.LogWarning(item.name);
		}
		//Debug.LogWarning(treeWithApples.childCount);

		if (treeWithApples.childCount > 1)
		{
			Transform applesParent = treeWithApples.GetChild(1);
			Debug.LogWarning(applesParent.childCount);
			foreach (Transform apple in applesParent)
			{
				apples.Add(apple);
			}
		}
	}

	public void StartAppleFall()
	{
		shouldFall = true;
	}
}