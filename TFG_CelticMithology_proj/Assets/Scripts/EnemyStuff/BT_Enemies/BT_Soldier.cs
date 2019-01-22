using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Soldier_Blackboard))]
public class BT_Soldier : BT_Entity
{
    //current_action
    public Action_FollowPlayer chase;
    public Action_PushBack pushback;
    public Action_Dead dead;
   // public Action_MeleeAttack melee_attack;
    private bool can_start_combat = false;
    private bool is_dead = false;

    override public void Start()
    {
    }

    override public void Update()
    {

        if ((int)myBB.GetParameter("live") <= 0)
        {
            //Animation of enemy dying
            if (currentAction != null)
            {
                currentAction.isFinish = true;
            }
            is_dead = true;
        }

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if (currentAction != chase && (bool)myBB.GetParameter("is_enemy_hit") == false && can_start_combat == false && is_dead == false)
        {
            currentAction = chase;
            decide = true;
        }
        else if ((bool)myBB.GetParameter("is_enemy_hit") == true && is_dead == false)
        {
            currentAction = pushback;
            decide = true;
        }
        else if (currentAction != dead && is_dead == true)
        {
            currentAction = dead;
            decide = true;
        }
        /*else if (can_start_combat == true)
        {
            currentAction = melee_attack;
            decide = true;
        }*/

        return decide;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Can start combat
        if (collision.tag == "player_combat_collider")
        {
            if (currentAction != null)
                currentAction.isFinish = true;

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
               /* if (currentAction == melee_attack)
                {
                    currentAction.isFinish = true;
                }*/
            }

            can_start_combat = false;
        }
    }
}