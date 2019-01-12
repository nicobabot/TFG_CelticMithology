using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Soldier : BT_Entity {

    //current_action
    public Action_FollowPlayer chase;
    public Action_PushBack pushback;
    public Action_MeleeAttack melee_attack;

    bool can_start_combat = false;

    override public void Update()
    {


        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if (currentAction != chase && can_start_combat == false)
        {
            currentAction = chase;
            decide = true;
        }
        else if ((bool)myBB.GetParameter("is_enemy_hit") == true)
        {
            currentAction = pushback;
            decide = true;
        }
       /* else if (can_start_combat == true)
        {
            //currentAction = chase;
            decide = true;
        }*/


        return decide;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Can start combat
        if(collision.tag == "player_combat_collider")
        {
            if(currentAction != null)
            currentAction.isFinish=true;

            can_start_combat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Start chasing
        if (collision.tag == "player_combat_collider")
        {
            if (currentAction != null)
            {
                if (currentAction == melee_attack)
                {
                    currentAction.isFinish = true;
                }
            }

            can_start_combat = false;
        }
    }

}
