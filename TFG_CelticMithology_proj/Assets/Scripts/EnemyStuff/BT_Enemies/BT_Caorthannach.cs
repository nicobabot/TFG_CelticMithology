using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Caorthannach : BT_Entity
{
    //current_action
    public Action_FollowPlayer chase;
    public Action_ShootPlayer shoot;

    override public void Update()
    {

        if ((int)myBB.GetParameter("live") <= 0)
        {
            //Animation of enemy dying
            if (currentAction != null)
            {
                currentAction.isFinish = true;
            }
            gameObject.SetActive(false);
        }

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if (currentAction != shoot)
        {
            currentAction = shoot;
            decide = true;
        }

        /*if (currentAction != chase && (bool)myBB.GetParameter("is_enemy_hit") == false && can_start_combat == false)
        {
            currentAction = chase;
            decide = true;
        }
        else if ((bool)myBB.GetParameter("is_enemy_hit") == true)
        {
            currentAction = pushback;
            decide = true;
        }*/


        return decide;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
