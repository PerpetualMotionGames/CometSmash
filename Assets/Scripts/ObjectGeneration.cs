using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneration : MonoBehaviour
{
	public GameObject player;
	float screenWidth;
	float screenHeight;
	public Sprite[] spaceSprites;
	public GameObject spaceItem;
	public GameObject spawnBoundary;

	//variables for determining size of spawning items and their locations.
	float minX;
	float maxX;
	float minY;
	float maxY;
	float minScaleX;
	float minScaleY;
	float maxScaleX;
	float maxScaleY;

	// Start is called before the first frame update
	void Start()
    {
		if (player == null)
		{
			player = GameObject.Find("Player");
		}
		screenHeight = Camera.main.orthographicSize*2;
		screenWidth = screenHeight / Screen.height * Screen.width;
		MultiSpawn(50, 10);
    }

	Vector3 rPosOffScreen()
	{
		Vector3 spawnPos = transform.position;
		float sWidth = Camera.main.orthographicSize / Screen.height * Screen.width;
		while(Mathf.Abs(spawnPos.x - transform.position.x) < sWidth/2 && Mathf.Abs(spawnPos.y - transform.position.y)< Camera.main.orthographicSize / 2)
		{
			spawnPos = RandomPos();
		}
		return spawnPos;
	}
	Vector3 RandomPos()
	{
		Vector3 spawnPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
		return spawnPos;
	}
	Vector2 RandomScale()
	{
		float scaleX = Random.Range(minScaleX, maxScaleX);
		float scaleY = Mathf.Clamp(Random.Range(minScaleY, maxScaleY),scaleX*0.9f,scaleX*1.1f);
		Vector3 size = new Vector3(scaleX,scaleY, 1);
		return size;
	}
	void UpdateBounds()
	{
		if (spawnBoundary == null)
		{
			minX = player.transform.position.x - screenWidth;
			maxX = player.transform.position.x + screenWidth;
			minY = player.transform.position.y - screenHeight;
			maxY = player.transform.position.y + screenHeight;
		}
		else
		{
			Vector3 len = spawnBoundary.transform.localScale;
			Vector3 pos = spawnBoundary.transform.position;
			minX = pos.x - len.x / 2;
			maxX = minX + len.x;
			minY = pos.y - len.y / 2;
			maxY = minY + len.y;
		}
		minScaleX = player.transform.localScale.x / 2;
		minScaleY = player.transform.localScale.y / 2;
		maxScaleX = player.transform.localScale.x * 2;
		maxScaleY = player.transform.localScale.y * 2;
	}
	public void SpawnItem()
	{
		UpdateBounds();
		//randomly spawn in an item
		int choice = Random.Range(0, spaceSprites.Length);
		GameObject newSpaceObject = Instantiate(spaceItem, rPosOffScreen(), Quaternion.identity);
		newSpaceObject.GetComponent<SpriteRenderer>().sprite = spaceSprites[choice];
		newSpaceObject.transform.localScale = RandomScale();
		if (choice > 32)
		{
			//its a planet rather than an asteroid
			newSpaceObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 200);
		}
	}

	public void MultiSpawn(int numItems, int intervals)
	{
		UpdateBounds();
		//use this for the initial item spawn so that it places them a little better
		float lenX = ((float)(maxX - minX)) / intervals;
		float lenY = ((float)(maxY - minY)) / intervals;
		int spawnsPerInterval = (int)Mathf.Ceil((float) numItems / (intervals * intervals));

		Debug.Log(new Vector4(lenX, lenY, minX,maxX));
		for(int i=0; i < intervals; i++)
		{
			for (int j = 0; j < intervals; j++)
			{
				for(int k = 0; k < spawnsPerInterval; k++)
				{
					if (i != 0 || j != 0)
					{
						int choice = Random.Range(0, spaceSprites.Length);
						float x = Random.Range(minX + i * lenX, minX + (i + 1) * lenX);
						float y = Random.Range(minY + j * lenY, minY + (j + 1) * lenY);
						GameObject newSpaceObject = Instantiate(spaceItem, new Vector3(x, y, 0), Quaternion.identity);
						newSpaceObject.GetComponent<SpriteRenderer>().sprite = spaceSprites[choice];
						newSpaceObject.transform.localScale = RandomScale();
						if (choice > 32)
						{
							//its a planet rather than an asteroid
							newSpaceObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 200);
						}
					}
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		//this is the point at which the player boundary moves away from one of the obstacles, what we want to do here is move it to the opposite bound.
		//determine whether it needs to move from top->bottom bottom->top, left>right...etc 
		UpdateBounds();
		Vector3 collisionPos = collision.gameObject.transform.position;

		bool outOfBoundsX = (collisionPos.x < minX || collisionPos.x > maxX);
		bool outOfBoundsY = (collisionPos.y < minY || collisionPos.y > maxY);

		float xPercent = (collisionPos.x - minX) / (maxX - minX);
		float yPercent = (collisionPos.y - minY) / (maxY - minY);
		float newX = minX + (maxX - minX) * xPercent;
		float newY = minY + (maxY - minY) * yPercent;
		//float newX = collisionPos.x;
		//float newY = collisionPos.y;
		var newScale = RandomScale();

		if (outOfBoundsX)
		{
			//out of left/right bounds
			newX = collisionPos.x < minX ? (maxX - newScale.x / 2) : (minX + newScale.x / 2);
		}
		if (outOfBoundsY)
		{
			//out of vertical bounds
			newY = collisionPos.y < minY ? (maxY - newScale.y / 2) : (minY + newScale.y / 2);
		}

		collision.gameObject.transform.position = rPosOffScreen();//new Vector3(newX, newY, collisionPos.z);
		collision.gameObject.transform.localScale = newScale;
	}
}
