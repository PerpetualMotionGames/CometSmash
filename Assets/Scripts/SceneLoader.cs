using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class SceneLoader : MonoBehaviour
{
	int thisIndex = 0; //index of current scene
	public void QuitGame()
	{
		Application.Quit();
	}
	public void LoadLevel(int index)
	{
		if (index == 1)
		{
			AudioController.ChangeVolume("Movement", PlayerPrefs.GetFloat("volume")*0.4f);
		}
		SceneManager.LoadScene(index);
	}

	public static void Victory()
	{
		SceneManager.LoadScene("VictoryScene");
	}
	public static void GameOver()
	{
		SceneManager.LoadScene("GameOver");
	}
	public static void Menu()
	{
		SceneManager.LoadScene("Menu");
	}

	public static void ReloadCurrentScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadNextLevel()
	{
		// Debug.Log(thisIndex);
		AudioController.ChangeVolume("Movement", PlayerPrefs.GetFloat("volume")*0.4f);
		if(thisIndex < SceneManager.sceneCountInBuildSettings - 1)
		{
			LoadLevel(thisIndex + 1);
		}
		else
		{
			// Debug.Log(SceneManager.sceneCountInBuildSettings);
			LoadLevel(0);
		}
	}

    public static int CurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
