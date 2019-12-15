using UnityEngine;

public class CameraController : MonoBehaviour {

    public float offsetUp;
    public float offsetDown;
    public float offsetLeft;
    public float offsetRight;
    public bool lockX;
    public bool lockY;
    
    private GameObject cameraBounds;
    private GameObject player;

    private void Start() {
        cameraBounds = GameObject.Find("CameraBounds");
        player = GameObject.Find("Player");
    }

    void LateUpdate() {

        float height = GetComponent<Camera>().orthographicSize;
        // careful of the order below, Screen.width is int not float so will be rounded
        float width = height * Screen.width / Screen.height;

        float playerX = player.transform.position.x;
        float x = transform.position.x;
        float playerY = player.transform.position.y;
        float y = transform.position.y;
        float offsetUp = height * (this.offsetUp / 100);
        float offsetDown = height * (this.offsetDown / 100);
        float offsetLeft = width * (this.offsetLeft / 100);
        float offsetRight = width * (this.offsetRight / 100);

        /* the way the movement is handled, allows the camera to move only if you're passing the center of the screen, 
         / or are past the offset, this way the camera will not lock onto you and makes it smoother overall
         / 
         / also check that the camera is not wider / taller than the bounds, if it is lock it to the center
        */
        if (cameraBounds != null && width * 2 > cameraBounds.transform.localScale.x) {
            x = cameraBounds.transform.position.x;
        } else if (!lockX) {
            // move the camera if the player is passed the center, or left of the offset
            if (playerX > (x + offsetRight)) {
                x = playerX - offsetRight;
            } else if (playerX < (x - offsetLeft)) {
                x = playerX + offsetLeft;
            }
            if (cameraBounds != null) {
                // check the bounds are not ouside of the game area
                float leftBound = cameraBounds.transform.position.x - (cameraBounds.transform.localScale.x / 2);
                float rightBound = cameraBounds.transform.position.x + (cameraBounds.transform.localScale.x / 2);
                if (x - width < leftBound) {
                    x = leftBound + width;
                } else if (x + width > rightBound) {
                    x = rightBound - width;
                }
            }
        }
        if (cameraBounds != null && height * 2 > cameraBounds.transform.localScale.y) {
            y = cameraBounds.transform.position.y;
        } else if (!lockY) {
            // move the camera if the player is passed the center, or below the offset
            if (playerY > (y + offsetUp)) {
                y = playerY - offsetUp;
            } else if (playerY < (y - offsetDown)) {
                y = playerY + offsetDown;
            }
            if (cameraBounds != null) {
                // check the bounds are not ouside of the game area
                float lowerBound = cameraBounds.transform.position.y - (cameraBounds.transform.localScale.y / 2);
                float upperBound = cameraBounds.transform.position.y + (cameraBounds.transform.localScale.y / 2);
                if (y - height < lowerBound) {
                    y = lowerBound + height;
                } else if (y + height > upperBound) {
                    y = upperBound - height;
                }
            }
        }

        transform.position = new Vector3(x, y, transform.position.z);

    }
}
