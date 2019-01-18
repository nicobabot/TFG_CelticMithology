﻿using UnityEngine;

[RequireComponent(typeof(Collision_Movement))]
[RequireComponent(typeof(Slash_Attack))]
[RequireComponent(typeof(Player_PushBack))]
public class Player_Manager : MonoBehaviour
{
    public float time_dashing = 1.0f;
    public float movement_speed = 5.0f;
    public float dash_speed = 10.0f;

    public enum Player_States
    {
        IDLE_PLAYER,
        MOVING_PLAYER,
        DASHING_PLAYER,
        SLASHING_PLAYER,
        FALLING_PLAYER,
        PUSHBACK_PLAYER
    }

    public Player_States current_state;

    public enum Player_Direction
    {
        UP_PLAYER = 0,
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

    private Collision_Movement movement_script;
    private Slash_Attack slash_attack_script;
    private Player_PushBack pushback_script;
    private float timer_dash = 0.0f;
    private bool want_to_dash = false;

    // Use this for initialization
    private void Start()
    {
        movement_script = GetComponent<Collision_Movement>();
        slash_attack_script = GetComponent<Slash_Attack>();
        pushback_script = GetComponent<Player_PushBack>();

        timer_dash = 0.0f;

        current_state = Player_States.IDLE_PLAYER;

        //testing porpouse-------------
        player_weapon = Player_Weapon.Sword;
        //----------------------------
    }

    // Update is called once per frame
    private void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Abutton")) && current_state != Player_States.PUSHBACK_PLAYER)
        {
            slash_attack_script.Update_Attack_Colliders_To_None_Active();
            current_state = Player_States.DASHING_PLAYER;
        }
        else if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Xbutton")) && current_state != Player_States.PUSHBACK_PLAYER)
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
        else if (current_state == Player_States.PUSHBACK_PLAYER)
        {
            slash_attack_script.Update_Attack_Colliders_To_None_Active();
            pushback_script.PushBack_Update();
        }
        else
        {
            timer_dash = 0.0f;
            movement_script.Movement_Update(movement_speed);
        }
    }

    public void Set_Enemy_Pushback(Transform enemy_trans)
    {
        pushback_script.enemy_pos = enemy_trans;
    }

}