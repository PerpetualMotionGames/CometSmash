using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicMenu : MonoBehaviour
{
	public GameObject comet;
	public float interval = 1f;
	float lastTime;
	float screenHeight;
	float screenWidth;
	public float cometSpeed = 100f;
	public Text optionalGameOverText;
	public Text optionalVictoryText;
	playerTemp pt;
	void Start()
	{
		screenHeight = Camera.main.orthographicSize * 2;
		screenWidth = screenHeight / Screen.height * Screen.width;
		lastTime = Time.time;

		if (optionalGameOverText != null)
		{
			optionalGameOverText.text = "You managed to reach a total size of " + PlayerPrefs.GetFloat("size", 0) +"% of the observable universe" ;
		}

		if( optionalVictoryText != null)
		{
			optionalVictoryText.text = "You destroyed the entire observable universe in just " + PlayerPrefs.GetFloat("seconds", 0) +  " seconds!";
		}
    }

    // Update is called once per frame
    void Update()
    {
		if(Time.time-lastTime > interval)
		{
			spawnItem();
			lastTime = Time.time;
		}
		
    }

	void spawnItem()
	{
		var pos = new Vector2(Random.Range(-screenWidth, -screenWidth / 2), Random.Range(screenHeight / 2, screenHeight));
		GameObject com = Instantiate(comet, pos, Quaternion.identity);
		Rigidbody2D rb = com.GetComponent<Rigidbody2D>();
		rb.AddForce(new Vector2(Random.Range(5, 15), Random.Range(-5, -15))*cometSpeed);
	}
}
