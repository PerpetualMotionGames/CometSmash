using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inGameUI : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		disableElements();

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Pause();
		}
	}

	public void Pause()
	{
		Time.timeScale = 0;
		enableElements();
	}

	public void Resume()
	{
		Time.timeScale = 1;
		disableElements();
	}

	public void menu()
	{
		Resume();
		SceneLoader.Menu();
	}

	private void enableElements()
	{
		foreach(Transform child in transform)
		{
			child.gameObject.SetActive(true);
		}
	}

	private void disableElements()
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(false);
		}
	}
}
