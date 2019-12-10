using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
	public GameObject target;

	public bool FollowX;
	public bool FollowY;
	public bool FollowZ;

	public float offsetX = 0;
	public float offsetY = 0;
	public float offsetZ = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		UpdatePosition();
    }

	void UpdatePosition()
	{
		Vector3 myPos = gameObject.transform.position;
		Vector3 pos = target.transform.position;
		float newX = FollowX ? (pos.x - offsetX) : myPos.x;
		float newY = FollowY ? (pos.y - offsetY) : myPos.y;
		float newZ = FollowZ ? (pos.z - offsetZ) : myPos.z;

		transform.position = new Vector3(newX, newY, newZ);
	}
}
