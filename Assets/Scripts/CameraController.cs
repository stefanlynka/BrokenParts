using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool at_location = true;
    public static float scale = 6.75F;
    public static float offset_x = -1.75f;
    public static float offset_y = -0.75f;
    public static float offset_z = -10f;
    private Vector3 target_location;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!at_location) {
            transform.position = Vector3.Lerp(transform.position, target_location, 0.01f);
            if (Vector3.Distance(transform.position, target_location) < 0.1f) at_location = true;
        }
	}
    public void MoveCamera(Vector3 new_location) {
        transform.position = new_location;
    }

    public void SlowMoveCamera(Vector3 new_location) {
        target_location = new_location;
        at_location = false;
    }
}
