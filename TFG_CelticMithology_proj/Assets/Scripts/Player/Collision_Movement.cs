using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Movement : MonoBehaviour {

    Rigidbody2D rb;
    Player_Manager play_manager_scr;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        play_manager_scr = GetComponent<Player_Manager>();
    }
	
	// Update is called once per frame
	public void Movement_Update (float new_speed, bool is_dashing= false) {

        float input_movement_horizontal = Input.GetAxisRaw("Horizontal");
        float input_movement_vertical = Input.GetAxisRaw("Vertical");

        if (input_movement_horizontal > 0.5f || input_movement_horizontal < -0.5f)
        {
            rb.velocity = new Vector3(input_movement_horizontal * new_speed, rb.velocity.y);
            if (!is_dashing)  play_manager_scr.current_state = Player_Manager.Player_States.MOVING_PLAYER;
        }
        else if (input_movement_horizontal < 0.5f || input_movement_horizontal > -0.5f)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y);
        }

        if (input_movement_vertical > 0.5f || input_movement_vertical < -0.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, input_movement_vertical * new_speed);
            if (!is_dashing) play_manager_scr.current_state = Player_Manager.Player_States.MOVING_PLAYER;
        }
        else if (input_movement_vertical < 0.5f || input_movement_vertical > -0.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f);
        }

        if ((input_movement_horizontal < 0.5f && input_movement_horizontal > -0.5f) && 
            (input_movement_vertical < 0.5f && input_movement_vertical > -0.5f)) {
            if (!is_dashing) play_manager_scr.current_state = Player_Manager.Player_States.IDLE_PLAYER;
        }

    }


}
