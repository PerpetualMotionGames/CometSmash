using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
	private static float musicvol;
	private static float soundvol;

	// Start is called before the first frame update
	private void Awake()
	{
		GameObject[] multiAudio = GameObject.FindGameObjectsWithTag("audio");
		if (multiAudio.Length > 1)
		{
			Destroy(gameObject);
		}
	}

	void Start()
    {
		musicvol = PlayerPrefs.GetFloat("MusicVolume", 1);
		soundvol = PlayerPrefs.GetFloat("SoundVolume", 1);
        ChangeVolume("Soundtrack", musicvol);
		DontDestroyOnLoad(gameObject);
    }

	public void SettingsVolume()
	{
		musicvol = PlayerPrefs.GetFloat("MusicVolume", 1);
		soundvol = PlayerPrefs.GetFloat("SoundVolume", 1);
        ChangeVolume("Soundtrack", musicvol);
    }

	public static void PlaySound(string clip) {
        AudioSource audioSource = GameObject.Find(clip).GetComponent<AudioSource>();
		audioSource.volume = soundvol;
        if (audioSource == null) {
            Debug.Log("AudioController.PlaySound could not find a clip named: " + clip);
            return;
        }
        audioSource.Play();
    }

    public static void ChangeVolume(string clip, float volume) {
        AudioSource audioSource = GameObject.Find(clip).GetComponent<AudioSource>();
        if (audioSource == null) {
            Debug.Log("AudioController.ChangeVolume could not find a clip named: " + clip);
            return;
        }
        audioSource.volume = volume;
        if (volume > 0 && !audioSource.isPlaying) {
            audioSource.Play();
        } else if (volume == 0 && audioSource.isPlaying) {
            audioSource.Stop();
        }
    }

    public static void IncrementPitch(string clip) {
        AudioSource audioSource = GameObject.Find(clip).GetComponent<AudioSource>();
        if (audioSource == null) {
            Debug.Log("AudioController.IncrementPitch could not find a clip named: " + clip);
            return;
        }
        audioSource.pitch += 0.1f;
    }

}
