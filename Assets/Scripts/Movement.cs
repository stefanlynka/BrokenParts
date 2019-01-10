using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {

    Rigidbody2D rb2D;
    public Animator animator;
    float idle_time = 0F;

    float x_acceleration = 0.4F;
    float jump_speed = 5F;
    float max_x_speed = 5;
    float x_speed = 0;
    float y_speed = 0;
    float x_drag = 0.15F;
    float low_jump = 2f;
    bool grounded = true;
    float ability_timer = 0;
    public float cooldown = 0.2f;


    public bool extend_allowed = true;
    public bool retract_allowed = true;
    public bool flip_x_allowed = true;
    public bool flip_y_allowed = true;

    Vector3 speed_vector;
    public Vector2 origin = new Vector2(0,0);


    // Use this for initialization
    void Awake () {
        rb2D = GetComponent<Rigidbody2D>();

        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    // FixedUpdate is called once per frame, right before physics calculations
    void FixedUpdate () {
        Walking();
        Inputs();
        if (ability_timer > 0) ability_timer -= Time.deltaTime;

        AnimationUpdate();
    }

    // Changes animator's state (and therefore animation frame) based on idle-time and spin speed
    void AnimationUpdate() {
        if (Mathf.Abs(rb2D.angularVelocity) > 500) animator.SetBool("happy", true);
        else animator.SetBool("happy", false);
        idle_time += Time.deltaTime;
        if ((animator.GetInteger("idle_state") != 0)&&(idle_time>1)) animator.SetInteger("idle_state", 0);
        if (idle_time > 4) {
            animator.SetInteger("idle_state", Random.Range(1, 5));
            idle_time = 0F;
        }
    }

    // Use abilities based on Inputs and ability permissions
    void Inputs() {
        if (Input.GetKey(KeyCode.A)) {
            if (x_speed >= 0) x_speed -= x_acceleration;
            x_speed -= x_acceleration;
        }
        if (Input.GetKey(KeyCode.D)) {
            if (x_speed <= 0) x_speed += x_acceleration;
            x_speed += x_acceleration;
        }

        if (Input.GetKey(KeyCode.W) && (grounded)) {
            Jump();
            animator.SetInteger("idle_state", 1);
        }
        else {
            y_speed = 0;
        }
        // Makes you fall faster if you let go of jump (short hopping)
        if (!(Input.GetKey(KeyCode.W)) && (rb2D.velocity.y>0)) {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (low_jump - 1) * Time.deltaTime;
        }

        // Use abilities
        if (ability_timer <= 0) {
            if ((Input.GetKey(KeyCode.J)) && (extend_allowed)) {
                Extend();
            }
            if ((Input.GetKey(KeyCode.K)) && (retract_allowed)) {
                Retract();
            }
            if ((Input.GetKey(KeyCode.L)) && (flip_x_allowed)) {
                FlipHorizontal();
            }
            if ((Input.GetKey(KeyCode.Semicolon)) && (flip_y_allowed)) {
                FlipVertical();
            }
        }
    }

    // If the destination is safe, flip x coordinates and x speed
    void FlipHorizontal() {
        Vector2 target = new Vector2(-(rb2D.position.x - origin.x) + origin.x, rb2D.position.y);
        if (GetComponent<Player>().SafeDestination(target)) {
            rb2D.position = target;
            ability_timer = cooldown;
            GetComponent<Player>().ability_counter++;
            rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
            x_speed = -x_speed;
        }
    }

    // If the destination is safe, flip y coordinates
    void FlipVertical() {
        Vector2 target = new Vector2(rb2D.position.x, -(rb2D.position.y - origin.y) + origin.y);
        if (GetComponent<Player>().SafeDestination(target)) {
            rb2D.position = target;
            ability_timer = cooldown;
            GetComponent<Player>().ability_counter++;
        }
    }

    // If the destination is safe, double your x and y coordinates
    // If you would Extend past the level's limits, stop at the edge of the level, staying on Extend's path
    // The quadrant you are in determines which level edge you check and where you stop
    void Extend() {
        float x_pos = rb2D.position.x - origin.x;
        float y_pos = rb2D.position.y - origin.y;
        float slope = y_pos / x_pos;
        float new_x = x_pos * 2;
        float new_y = y_pos * 2;

        float map_height = GetComponent<Player>().current_level_height;
        float map_width = GetComponent<Player>().current_level_width;
        float quadrant_slope = map_height / map_width;

        // If you would extend past the end of the level, this stops you based on your quadrant
        // Top Quadrant
        if ((y_pos * 2 > map_height) && (Mathf.Abs(slope) > quadrant_slope)) {
            new_y = map_height;
            float angle = Mathf.Atan2(y_pos, x_pos);
            new_x = new_y / Mathf.Tan(angle);
        }

        // Bottom Quadrant
        else if ((y_pos * 2 < -map_height) && (Mathf.Abs(slope) > quadrant_slope)) {
            new_y = -map_height;
            float angle = Mathf.Atan2(y_pos, x_pos);
            new_x = new_y / Mathf.Tan(angle);
        }

        //Right Quadrant
        else if ((x_pos * 2 > map_width) && (Mathf.Abs(slope) < quadrant_slope)) {
            new_x = map_width;
            float angle = Mathf.Atan2(y_pos, x_pos);
            new_y = new_x * Mathf.Tan(angle);
        }

        //Left Quadrant
        else if ((x_pos * 2 < -map_width) && (Mathf.Abs(slope) < quadrant_slope)) {
            new_x = -map_width;
            float angle = Mathf.Atan2(y_pos, x_pos);
            new_y = new_x * Mathf.Tan(angle);
        }

        Vector2 target = new Vector2(new_x + origin.x, new_y + origin.y);

        if (GetComponent<Player>().SafeDestination(target)) {
            rb2D.position = target;

            ability_timer = cooldown;
            GetComponent<Player>().ability_counter++;
        }
    }

    // If the destination is safe, halve your x and y coordinates
    void Retract() {
        Vector2 origin = GetComponent<Player>().current_origin;
        Vector2 target = new Vector2((rb2D.position.x - origin.x) / 2 + origin.x, (rb2D.position.y - origin.y) / 2 + origin.y);
        if (GetComponent<Player>().SafeDestination(target)) {
            rb2D.position = target;
            ability_timer = cooldown;
            GetComponent<Player>().ability_counter++;
        }
    }

    // Set your velocity based on walking speed
    void Walking() {
        if (x_speed > max_x_speed)   x_speed = max_x_speed;
        if (x_speed < -max_x_speed) x_speed = -max_x_speed;

        rb2D.velocity = new Vector2(x_speed, rb2D.velocity.y);

        if (x_speed > 0) x_speed -= Mathf.Min(x_drag, x_speed);
        if (x_speed < 0) x_speed += Mathf.Max(x_drag, x_speed);
    }

    void Jump() {
        y_speed += jump_speed;
        y_speed = Mathf.Max(y_speed, jump_speed);
        grounded = false;
        rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Min(rb2D.velocity.y + y_speed, jump_speed));
    }

    // Set your position to spawn
    public void ResetPosition(Vector2 spawn) {
        print("spawn = " + spawn);
        rb2D.position = new Vector2(spawn.x, spawn.y);
        rb2D.velocity = new Vector2(0, 0);
        x_speed = 0;
    }

    public void SetAbilityPermissions(bool extend, bool retract, bool flip_x, bool flip_y) {
        extend_allowed = extend;
        retract_allowed = retract;
        flip_x_allowed = flip_x;
        flip_y_allowed = flip_y;
    }

    // When you touch the ground, set grounded to true so you can jump again
    private void OnTriggerStay2D(Collider2D collider) {
        if(collider.gameObject.tag == "Ground") {
            grounded = true;
        }
    }

    // If you touch a collectible, collect and destroy it
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Collectible") {
            Destroy(collider.gameObject);
            GetComponent<Player>().collected++;
        }
    }
}
