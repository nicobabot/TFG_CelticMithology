﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_Attack : MonoBehaviour {

    public GameObject father_collider_slash_attack;
    public float time_slashing = 1.0f;
    public CameraManager cam_manager;

    Player_Manager player_manager_sct;
    float timer_slash = 0.0f;
    Collider2D[] enemies_found = null;

    bool is_slash_done = false;

    // Use this for initialization
    void Start () {
        player_manager_sct = GetComponent<Player_Manager>();
    }
	
	// Update is called once per frame
	public void Attack_Slash_Update () {

        GameObject collider_to_activate = father_collider_slash_attack.transform.GetChild((int)player_manager_sct.player_direction).gameObject;

        if (!collider_to_activate.active)
        {
            collider_to_activate.SetActive(true);
        }

        Detect_Collision_Slash collision_slash_scr = collider_to_activate.GetComponent<Detect_Collision_Slash>();

        if(collision_slash_scr!= null && is_slash_done == false)
        {
            enemies_found = collision_slash_scr.Is_Enemy_Collided();
            if (enemies_found.Length > 0)
            {
                React_To_Slash();
            }
            is_slash_done = true;
        }

        timer_slash += Time.deltaTime;

        if(timer_slash> time_slashing)
        {
            is_slash_done = false;
            timer_slash = 0;
            collider_to_activate.SetActive(false);
            player_manager_sct.current_state = Player_Manager.Player_States.IDLE_PLAYER;
        }

    }

    void React_To_Slash()
    {
        Debug.Log("Enemies detected: " + enemies_found.Length);
        cam_manager.Cam_Shake();

        foreach (Collider2D col in enemies_found)
        {

            //Detect enemy type
            BT_Soldier soldier = col.GetComponent<BT_Soldier>();
            if (soldier != null)
            {
                Soldier_Blackboard bb_soldier = col.GetComponent<Soldier_Blackboard>();
                bb_soldier.is_enemy_hit.SetValue(true);
            }

        }

        System.Array.Clear(enemies_found, 0, enemies_found.Length);
            //Call enemy damage function
    }

}