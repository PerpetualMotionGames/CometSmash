using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneration : MonoBehaviour
{
	public GameObject player;
	private float screenWidth;
	private float screenHeight;
	public Sprite[] spaceSprites;
	public GameObject spaceItem;
	public GameObject spawnBoundary;
    public int maxObjects = 100;

	// Start is called before the first frame update
	void Start()
    {
		if (player == null)
		{
			player = GameObject.Find("Player");
		}
		screenHeight = Camera.main.orthographicSize*2;
		screenWidth = screenHeight / Screen.height * Screen.width;
		// MultiSpawn(50, 10);
    }

    private void FixedUpdate() {
        GameObject[] spaceObjects = GameObject.FindGameObjectsWithTag("SpaceObject");
        // only want to spawn more if the max isn't already breached
        if (spaceObjects.Length < maxObjects) {
            Bounds spawnBoundary = GetSpawnBounds();
            int itemsInSpawn = CountItemsInBounds(spaceObjects, spawnBoundary);
            int maxItemsInSpawn = (int)(maxObjects / 6); // around 16 % should leave room to always spawn and leave less gaps
            while (itemsInSpawn < maxItemsInSpawn) {
                SpawnItem();
                itemsInSpawn++;
            }
        }
    }

    private int CountItemsInBounds(GameObject[] gameObjects, Bounds bounds) {
        int count = 0;
        foreach (GameObject g in gameObjects) {
            Vector3 pos = g.transform.position;
            // if in bounds then increment count
            if (pos.x >= bounds.minX && 
                pos.x <= bounds.maxX && 
                pos.y >= bounds.minY && 
                pos.y <= bounds.maxY) {
                count++;
            }
        }
        return count;
    }

    private Vector3 GetRandomPos(Bounds boundary)
	{
		return new Vector3(Random.Range(boundary.minX, boundary.maxX), Random.Range(boundary.minY, boundary.maxY), 0);
	}
	private Vector3 GetRandomScale(Bounds scaleBounds)
	{
		float scaleX = Random.Range(scaleBounds.minX, scaleBounds.maxX);
		float scaleY = Mathf.Clamp(Random.Range(scaleBounds.minY, scaleBounds.maxY),scaleX*0.9f,scaleX*1.1f);
		return new Vector3(scaleX, scaleY, 0);
	}
	private Bounds GetSpawnBounds()
	{
        Vector3 len = spawnBoundary.transform.localScale;
        Vector3 pos = spawnBoundary.transform.position;
        Bounds spawnBounds = new Bounds();
        // only want to spawn items right of the screen in the final 80%
        spawnBounds.minX = pos.x + len.x * 0.3f;
        spawnBounds.maxX = pos.x + len.x / 2;
        spawnBounds.minY = pos.y - len.y / 2;
        spawnBounds.maxY = pos.y + len.y / 2;
        return spawnBounds;
	}
    private Bounds GetGameBounds() {
        Bounds gameBounds = new Bounds();
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight / Screen.height * Screen.width;
        if (spawnBoundary == null) {
            gameBounds.minX = Camera.main.transform.position.x - screenWidth;
            gameBounds.maxX = Camera.main.transform.position.x + screenWidth;
            gameBounds.minY = Camera.main.transform.position.y - screenHeight;
            gameBounds.maxY = Camera.main.transform.position.y + screenHeight;
        } else {
            Vector3 len = spawnBoundary.transform.localScale;
            Vector3 pos = spawnBoundary.transform.position;
            gameBounds.minX = pos.x - len.x / 2;
            gameBounds.maxX = pos.x + len.x / 2;
            gameBounds.minY = pos.y - len.y / 2;
            gameBounds.maxY = pos.y + len.y / 2;
        }
        return gameBounds;
    }
    private Bounds GetItemScaleBounds() {
        Bounds scaleBounds = new Bounds();
        scaleBounds.minX = player.transform.localScale.x / 2;
        scaleBounds.maxX = player.transform.localScale.x * 3;
        scaleBounds.minY = player.transform.localScale.y / 2;
        scaleBounds.maxY = player.transform.localScale.y * 3;
        return scaleBounds;
    }

    public void SpawnItem()
	{
        Vector3 position = GetRandomPos(GetSpawnBounds());
        Vector3 scale = GetRandomScale(GetItemScaleBounds());

        //randomly spawn in an item
        int choice = Random.Range(0, spaceSprites.Length);
		GameObject newSpaceObject = Instantiate(spaceItem, position, Quaternion.identity);
		newSpaceObject.GetComponent<SpriteRenderer>().sprite = spaceSprites[choice];
		newSpaceObject.transform.localScale = scale;
		if (choice > 31)
		{
			//its a planet rather than an asteroid
			newSpaceObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 200);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		//this is the point at which the player boundary moves away from one of the obstacles, what we want to do here is move it to the opposite bound.
		//determine whether it needs to move from top->bottom bottom->top, left>right...etc 
		Vector3 objectPos = collision.gameObject.transform.position;
        Bounds gameBounds = GetGameBounds();
        float gameHeight = gameBounds.maxY - gameBounds.minY;
        // if out of bounds in the X axis, kill the object
        if (objectPos.x < gameBounds.minX || objectPos.x > gameBounds.maxX) {
            Destroy(collision.gameObject);
            return;
        }

        // if out of bounds on the Y axis, move it to the other side
        while (objectPos.y < gameBounds.minY) {
            objectPos.y += gameHeight;
        }
        while (objectPos.y > gameBounds.maxY) {
            objectPos.y -= gameHeight;
        }
        collision.gameObject.transform.position = objectPos;
	}
}

class Bounds {
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public Bounds() : this(0, 0, 0, 0) { }
    public Bounds(float minX, float maxX, float minY, float maxY) {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }
}