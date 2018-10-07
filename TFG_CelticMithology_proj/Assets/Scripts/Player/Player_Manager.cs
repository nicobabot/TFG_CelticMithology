using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour {
    public float time_dashing = 1.0f;
    public float movement_speed = 5.0f;
    public float dash_speed = 10.0f;

    public enum Player_States
    {
        IDLE_PLAYER,
        MOVING_PLAYER,
        DASHING_PLAYER,
        SLASHING_PLAYER,
        FALLING_PLAYER
    }
    public  Player_States current_state;

    public enum Player_Direction
    {
        UP_PLAYER=0,
        DOWN_PLAYER,
        RIGHT_PLAYER,
        LEFT_PLAYER
    }
    public Player_Direction player_direction;

    public enum Player_Weapon
    {
        Sword,
        Shield
    }
    public Player_Weapon player_weapon;

    Collision_Movement movement_script;
    Slash_Attack slash_attack_script;
    float timer_dash = 0.0f;
    bool want_to_dash = false;



    // Use this for initialization
    void Start () {

        movement_script = GetComponent<Collision_Movement>();
        if (movement_script == null)
        {
            Debug.Log("Movement script not found _ Player_Manager");
        }

        slash_attack_script = GetComponent<Slash_Attack>();
        if (slash_attack_script == null)
        {
            Debug.Log("Slash script not found _ Player_Manager");
        }

        timer_dash = 0.0f;

        current_state = Player_States.IDLE_PLAYER;

        //testing porpouse-------------
        player_weapon = Player_Weapon.Sword;
        //----------------------------

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Abutton"))
        {
            current_state = Player_States.DASHING_PLAYER;
        }
        else if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Xbutton"))
        {
            current_state = Player_States.SLASHING_PLAYER;
        }


        if (current_state == Player_States.DASHING_PLAYER && timer_dash <= time_dashing)
        {
            timer_dash += Time.deltaTime;
            movement_script.Movement_Update(dash_speed, true);
        }
        else if (current_state == Player_States.SLASHING_PLAYER)
        {
            slash_attack_script.Attack_Slash_Update();
        }
        else if (current_state == Player_States.FALLING_PLAYER)
        {
            //todo
            Vector3 temp_vect = Vector3.zero;
            transform.position = temp_vect;
            current_state = Player_States.IDLE_PLAYER;
        }
        else {
            timer_dash = 0.0f;
            movement_script.Movement_Update(movement_speed);
        }

    }
}
