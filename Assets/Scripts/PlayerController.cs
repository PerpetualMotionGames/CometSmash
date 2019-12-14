using UnityEngine;

public class PlayerController : MonoBehaviour {

    // gravity increases velocity of an object at 9.8 meters per second
    // example, after 3 seconds of free fall, an object will be travelling at 9.8 * 3 meters per second
    // private static float GRAVITY = 9.8f;

    private Rigidbody2D rb2d;
    // private Animator animator;
    private float moveHorizontal = 0f;
    private bool dying = false;
    private float bearing = 0f; // radians
    private float speed = 10f;
    public int lives = 3;
    public float size = 0f;


    // Start is called before the first frame update
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        // calculate animation effects
        moveHorizontal = Input.GetAxisRaw("Horizontal");
    }

    // Fixed update is called just before calculating any physics
    private void FixedUpdate() {
        Vector2 velocity = rb2d.velocity;

        // get the current bearing and requested heading based on player input
        bearing = GetBearing(velocity); 
        float heading = GetHeading(moveHorizontal);
        // to turn the player we will get the velocity of the direction of travel at a speed of 1 and add this to the current velocity
        Vector2 turnVelocity = GetVelocity(bearing + heading, speed/10);
        velocity += turnVelocity;
        // we then need to recalculate the new bearing after applying turn velocity
        bearing = GetBearing(velocity);

        // set the new velocity and rotation based on the new bearing and speed
        rb2d.velocity = GetVelocity(bearing, speed);
        rb2d.SetRotation(Mathf.Rad2Deg * bearing);
    }

    // returns bearing in radians based on velocity
    private static float GetBearing(Vector2 velocity) {
        return Mathf.Atan2(velocity.y, velocity.x);
    }

    // returns heading based on the players horizontal axis
    private static float GetHeading(float move) {
        // convert to radians
        return move *= (Mathf.PI / 2) * -1;
    }

    // returns the velocity based on the bearing (radians) and speed
    private Vector2 GetVelocity(float bearing, float speed) {
        float x = speed * Mathf.Cos(bearing);
        float y = speed * Mathf.Sin(bearing);
        return new Vector2(x,y);
    }


    //OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Respawn") {
            var scale = collision.gameObject.transform.localScale;
            if (areaDiff(collision.gameObject, gameObject) < 0.5f) {
                //similar size
                // Debug.Log("bounce");
            } else if (roughArea(gameObject) < roughArea(collision.gameObject)) {
                //bigger
                lives -= 1;
            } else {
                //player is bigger
                Destroy(collision.gameObject);
                transform.localScale += new Vector3(1, 1, 0) * 0.1f;
                size += 1;
                GameObject.Find("Boundary").transform.localScale += new Vector3(5, 4, 0) * 2;
                speed += 2.5f;
                Camera.main.orthographicSize += 1;
                GameObject.Find("Boundary").GetComponent<ObjectGeneration>().SpawnItem();
            }

        }
    }
    private float roughArea(GameObject obj) {
        return obj.transform.localScale.x * obj.transform.localScale.y;
    }
    private float areaDiff(GameObject a, GameObject b) {
        return Mathf.Abs(roughArea(a) - roughArea(b));
    }
    public void Die() {
        if (!dying) {
            dying = true;
            //AudioController.PlaySound("GameOver");
            //KillObject kill = gameObject.AddComponent<KillObject>();
            //kill.PlayerKill();
        }
    }

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "something")
		{
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "something")
		{
		}
	}
}
