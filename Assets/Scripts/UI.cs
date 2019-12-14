using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	// Start is called before the first frame update
	int lives = 3;
	float timer;
	float size = 0;
	public Sprite[] livesStates;
	public GameObject healthbar;
	float screenHeight;
	float screenWidth;
	playerTemp pt;
	public Image bar; //keeps track of player size
	void Start()
	{
		timer = Time.time;
		screenHeight = Camera.main.orthographicSize * 2;
		screenWidth = screenHeight / Screen.height * Screen.width;
		pt = GameObject.Find("Player").GetComponent<playerTemp>();
	}

	// Update is called once per frame
	void Update()
	{
		lives = pt.lives;
		size = Mathf.Clamp(pt.size, 0, 500);
		if (size == 500)
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
