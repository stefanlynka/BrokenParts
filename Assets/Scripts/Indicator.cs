using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {

    GameObject player;
    Vector2 origins;
    float player_x;
    float player_y;

    bool on = true;

    private void Awake() {
        player = GameObject.Find("/Player");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        origins = player.GetComponent<Player>().current_origin;
        if (on) {
            if (name == "Inner Line") ILineUpdate();
            if (name == "Inner Circle") ICircleUpdate();
            if (name == "Outer Line") OLineUpdate();
            if (name == "Outer Circle") OCircleUpdate();
            if (name == "X Circle") XCircleUpdate();
            if (name == "Y Circle") YCircleUpdate();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            on = !on;
            if (!on) {
                transform.position = new Vector3(0, 0, 50);
            }
        }
    }

    void XCircleUpdate() {
        transform.position = Tool.FlipXFromOrigin(player.transform.position, origins);
    }
    
    void YCircleUpdate() {
        transform.position = Tool.FlipYFromOrigin(player.transform.position, origins);
    }

    void ICircleUpdate() {
        transform.position = Tool.HalveFromOrigin(player.transform.position, origins);
    }



    void OCircleUpdate() {
        float height = player.GetComponent<Player>().current_level_height;
        float width = player.GetComponent<Player>().current_level_width;
        float q_slope = height / width;
        float x_pos = player.transform.position.x - origins.x;
        float y_pos = player.transform.position.y - origins.y;
        float slope = y_pos / x_pos;
        float new_x = x_pos * 2;
        float new_y = y_pos * 2;

        // Top Quadrant
        if ((y_pos * 2 > height) && (Mathf.Abs(slope) > q_slope)) {
            new_y = height;
            float angle = Mathf.Atan2(y_pos, x_pos);
            new_x = new_y / Mathf.Tan(angle);
        }

        // Bottom Quadrant
        if ((y_pos * 2 < -height) && (Mathf.Abs(slope) > q_slope)) {
            new_y = -height;
            float angle = Mathf.Atan2(y_pos, x_pos);
            new_x = new_y / Mathf.Tan(angle);
        }

        //Right Quadrant
        if ((x_pos * 2 > width) && (Mathf.Abs(slope) < q_slope)) {
            new_x = width;
            float angle = Mathf.Atan2(y_pos, x_pos);
            new_y = new_x * Mathf.Tan(angle);
        }

        //Left Quadrant
        if ((x_pos * 2 < -width) && (Mathf.Abs(slope) < q_slope)) {
            new_x = -width;
            float angle = Mathf.Atan2(y_pos, x_pos);
            new_y = new_x * Mathf.Tan(angle);
        }
        transform.position = new Vector2(new_x + origins.x, new_y + origins.y);

    }

    void ILineUpdate() {
        transform.position = Tool.HalveFromOrigin(player.transform.position, origins);
        float angle = Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.y - origins.y, player.transform.position.x - origins.x) + 90;
        transform.eulerAngles = new Vector3(0, 0, angle);
        float distance = Vector2.Distance(player.transform.position, origins);
        transform.localScale = new Vector3(0.25F, distance/4, 1);
    }

    void OLineUpdate() {
        float height = player.GetComponent<Player>().current_level_height;
        float width = player.GetComponent<Player>().current_level_width;
        float q_slope = height / width;
        float x_pos = player.transform.position.x - origins.x;
        float y_pos = player.transform.position.y - origins.y;
        float slope = y_pos / x_pos;
        float new_x = x_pos * 2;
        float new_y = y_pos * 2;

        // Top Quadrant
        if ((y_pos * 2 > height) && (Mathf.Abs(slope) > q_slope)) {
            new_y = height;
            float angleb = Mathf.Atan2(y_pos, x_pos);
            new_x = new_y / Mathf.Tan(angleb);
        }

        // Bottom Quadrant
        if ((y_pos * 2 < -height) && (Mathf.Abs(slope) > q_slope)) {
            new_y = -height;
            float angleb = Mathf.Atan2(y_pos, x_pos);
            new_x = new_y / Mathf.Tan(angleb);
        }

        //Right Quadrant
        if ((x_pos * 2 > width) && (Mathf.Abs(slope) < q_slope)) {
            new_x = width;
            float angleb = Mathf.Atan2(y_pos, x_pos);
            new_y = new_x * Mathf.Tan(angleb);
        }

        //Left Quadrant
        if ((x_pos * 2 < -width) && (Mathf.Abs(slope) < q_slope)) {
            new_x = -width;
            float angleb = Mathf.Atan2(y_pos, x_pos);
            new_y = new_x * Mathf.Tan(angleb);
        }
        transform.position = new Vector2(((new_x + origins.x) + player.transform.position.x)/ 2, ((new_y + origins.y) + player.transform.position.y)/ 2);

        float angle = Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.y - origins.y, player.transform.position.x - origins.x) + 90;
        transform.eulerAngles = new Vector3(0, 0, angle);
        float distance = Vector2.Distance(transform.position, player.transform.position);
        transform.localScale = new Vector3(0.25F, distance / 2, 1);
    }
}
