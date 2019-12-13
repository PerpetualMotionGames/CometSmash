using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTemp : MonoBehaviour
{
	Rigidbody2D rb;
	public float moveSpeed = 10f;
	public int lives;
	public float size = 0f;
    // Start is called before the first frame update
    void Start()
    {
		lives = 3;
		rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		transform.position += new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
    }

	float roughArea(GameObject obj)
	{
		return obj.transform.localScale.x * obj.transform.localScale.y;
	}
	float areaDiff(GameObject a, GameObject b)
	{
		return Mathf.Abs(roughArea(a) - roughArea(b));
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Respawn")
		{
			var scale = collision.gameObject.transform.localScale;
			if (areaDiff(collision.gameObject, gameObject) < 0.5f)
			{
				//similar size
				Debug.Log("bounce");
			}
			else if (roughArea(gameObject) < roughArea(collision.gameObject))
			{
				//bigger
				lives -= 1;
			}
			else 
			{
				//player is bigger
				Destroy(collision.gameObject);
				transform.localScale += new Vector3(1, 1, 0)*0.25f;
				size += 10;
				GameObject.Find("Boundary").transform.localScale += new Vector3(5,4,0)*2;
				moveSpeed += 1;
				Camera.main.orthographicSize += 1;
				GameObject.Find("Boundary").GetComponent<ObjectGeneration>().SpawnItem();
			}

		}
	}
}
