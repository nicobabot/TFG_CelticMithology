﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_MeleeAttack : ActionBase {

    public float time_to_attack = 0.5f;
    public LayerMask player_layer;
    float timer_to_attack = 0.5f;
    GameObject player;
    BoxCollider2D collider;

    override public BT_Status StartAction()
    {
        timer_to_attack = time_to_attack;
        player = (GameObject)myBT.myBB.GetParameter("player");
        Direction dir = (Direction)myBT.myBB.GetParameter("direction");

        if (transform.childCount > 0)
        {
            if (transform.GetChild(0).childCount > 0) {

                collider = transform.GetChild(0).GetChild((int)dir).GetComponent<BoxCollider2D>();

            }
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        if(collider == null)
        {
            Debug.Log("No collider there! _Action_MeleeAttack");
            return BT_Status.RUNNING;
        }

     

        //need to know the animation timing
        timer_to_attack += Time.deltaTime;

        if (timer_to_attack > time_to_attack)
        {
            Collider2D col_temp = Physics2D.OverlapBox(collider.transform.position, collider.size, 0.0f, player_layer);

            if (col_temp != null)
            {
                Debug.Log("Player damaged!");
                //Damage player
            }
            timer_to_attack = 0;
        }
        
        

        return BT_Status.RUNNING;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(collider.transform.position, collider.size);
    }

    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }
}
