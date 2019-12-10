using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	Rigidbody2D rb;
	public GameObject[] randoms;
	float lastTime;
	float spawnTime = 1f;
	public float moveSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
		lastTime = Time.time;
		rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.time - lastTime > spawnTime)
		{
			for (int i = 0; i < 10; i++)
			{
				SpawnRandom();
			}
			

			lastTime = Time.time;
		}
		rb.velocity += Vector2.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		
    }

	void SpawnRandom()
	{
		float posX = Random.Range(transform.position.x - Camera.main.orthographicSize, transform.position.x + Camera.main.orthographicSize);
		float posY = Random.Range(transform.position.y - Camera.main.orthographicSize/2, transform.position.y - Camera.main.orthographicSize*2);

		int choice = Random.Range(0, randoms.Length);
		GameObject rando = Instantiate(randoms[choice], new Vector3(posX, posY, 0),Quaternion.identity);
		float scaleX = Random.Range(transform.localScale.x / 2, transform.localScale.x * 1.5f);
		float scaleY = Random.Range(transform.localScale.y / 2, transform.localScale.y * 1.5f);
		rando.transform.localScale = new Vector3(scaleX, scaleY, 1);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		float grow = 0.01f;
		if (collision.gameObject.tag == "Respawn")
		{
			Destroy(collision.gameObject);
			transform.localScale += new Vector3(grow,grow,0);
			Camera.main.orthographicSize += 0.2f;
		}
	}
}
