using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	// Start is called before the first frame update
	private int lives = 3;
	private float timer;
    private float size = 0;
	public Sprite[] livesStates;
	public GameObject healthbar;
    private float screenHeight;
    private float screenWidth;
    private PlayerController playerController;
	public Image bar; //keeps track of player size
	void Start()
	{
		timer = Time.time;
		screenHeight = Camera.main.orthographicSize * 2;
		screenWidth = screenHeight / Screen.height * Screen.width;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		lives = playerController.lives;
		size = Mathf.Clamp(playerController.size, 0, 100);
		if (size == 100)
		{
			PlayerPrefs.SetFloat("seconds", Mathf.Round((Time.time - timer) * 10) / 10f);
			SceneLoader.Victory();
		}
		UpdateUI();
	}

	public void UpdateUI()
	{
		if (lives > 0)
		{
			healthbar.GetComponent<Image>().sprite = livesStates[lives - 1];
		}
		else
		{
			Color trans = new Color(0, 0, 0, 0);
			healthbar.GetComponent<Image>().color = trans;
		}
		bar.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,Mathf.Lerp(bar.gameObject.GetComponent<RectTransform>().rect.width, size,Time.deltaTime*10));
	}
}
