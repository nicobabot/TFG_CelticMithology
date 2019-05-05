using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Banshee_Blackboard))]
public class BT_Banshee : BT_Entity
{
    //current_action
    public Action_FollowPlayer chase;
    public Action_PushBack pushback;
    public Action_DissapearAppear dissappear;
    public Action_Stunned stunned;
    public Action_Dead dead;

    public enum BansheeState
    {
        OTHER_BANSHEE,
        STUNNED_BANSHEE
    }
    public BansheeState myState;

    // public Action_MeleeAttack melee_attack;
    private bool can_start_combat = false;
    private bool is_dead = false;
    private bool want_stun = false;

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


        if ((bool)myBB.GetParameter("playerInsideRoom"))
        {
            if (currentAction != chase && (bool)myBB.GetParameter("is_enemy_hit") == false && !(bool)myBB.GetParameter("want_to_hit")
               && !(bool)myBB.GetParameter("is_enemy_stunned") && can_start_combat == false && is_dead == false)
            {
                myState = BansheeState.OTHER_BANSHEE;
                currentAction = chase;
                decide = true;
            }
            else if (currentAction != dissappear && (bool)myBB.GetParameter("want_to_hit") && !(bool)myBB.GetParameter("is_enemy_stunned"))
            {
                myState = BansheeState.OTHER_BANSHEE;
                //Dissappear and appear close
                myBB.SetParameter("want_to_hit", false);
                currentAction = dissappear;
                decide = true;
            }
            else if (currentAction != stunned /*&& (bool)myBB.GetParameter("want_to_hit")*/ && (bool)myBB.GetParameter("is_enemy_stunned"))
            {
                myState = BansheeState.STUNNED_BANSHEE;
                currentAction = stunned;
                decide = true;
            }
            else if ((bool)myBB.GetParameter("is_enemy_hit") == true && is_dead == false)
            {
                myState = BansheeState.OTHER_BANSHEE;
                currentAction = pushback;
                decide = true;
            }
            else if (currentAction != dead && is_dead == true)
            {
                myState = BansheeState.OTHER_BANSHEE;
                currentAction = dead;
                decide = true;
            }
            /*else if (can_start_combat == true)
            {
                currentAction = melee_attack;
                decide = true;
            }*/
        }

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

        if (collision.tag == "player_change_state_collider")
        {
            Player_Manager pm = collision.GetComponentInParent<Player_Manager>();
            if (pm != null && pm.current_state == Player_Manager.Player_States.DASHING_PLAYER 
                && currentAction != stunned && !(bool)myBB.GetParameter("is_enemy_stunned"))
            {
                if (currentAction != null)
                    currentAction.isFinish = true;

                myBB.SetParameter("is_enemy_stunned", true);
            }
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