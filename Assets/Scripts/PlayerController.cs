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
    private float speed = 5f;


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
        speed += 0.01f; // constantly increase player speed
        Vector2 velocity = rb2d.velocity;

        // get the current bearing and requested heading based on player input
        bearing = GetBearing(velocity); 
        float heading = GetHeading(moveHorizontal);
        // to turn the player we will get the velocity of the direction of travel at a speed of 1 and add this to the current velocity
        Vector2 turnVelocity = GetVelocity(bearing + heading, 1);
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
    void OnTriggerEnter2D(Collider2D obj)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (obj.gameObject.CompareTag("Projectile")) {
            obj.gameObject.SetActive(false);
            //AudioController.PlaySound("PlayerHit");
        }
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
