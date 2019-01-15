using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Movement : MonoBehaviour {

    public Collider2D combat_player_collider;
    public Collider2D falling_player_collider;

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

        //If the player is dashing we want to deactivate the combat collider to
        //be inmortal
        if(is_dashing == true)
        {
            if (combat_player_collider.enabled)
            {
                combat_player_collider.enabled = false;
                falling_player_collider.enabled = false;
            }
        }
        else
        {
            //If the player finished dashing we want to activate the combat collider
            if (!combat_player_collider.enabled)
            {
                combat_player_collider.enabled = true;
                falling_player_collider.enabled = true;
            }
        }

        //Just to keep tracking of the direction of the player
        if (input_movement_horizontal > 0.5f)
        {
            play_manager_scr.player_direction = Player_Manager.Player_Direction.RIGHT_PLAYER;
        }
        else if (input_movement_horizontal < -0.5f)
        {
            play_manager_scr.player_direction = Player_Manager.Player_Direction.LEFT_PLAYER;
        }

        if (input_movement_vertical > 0.5f)
        {
            play_manager_scr.player_direction = Player_Manager.Player_Direction.UP_PLAYER;
        }
        else if(input_movement_vertical < -0.5f)
        {
            play_manager_scr.player_direction = Player_Manager.Player_Direction.DOWN_PLAYER;
        }

        //Movement calculation
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

        //Just to change the state of the player
        if ((input_movement_horizontal < 0.5f && input_movement_horizontal > -0.5f) && 
            (input_movement_vertical < 0.5f && input_movement_vertical > -0.5f)) {
            if (!is_dashing) play_manager_scr.current_state = Player_Manager.Player_States.IDLE_PLAYER;
        }

    }


}
