using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Kelpi_Blackboard))]
public class BT_Kelpi : BT_Entity
{

    public enum Kelpi_Phases
    {
        KELPI_PHASE_1,
        KELPI_PHASE_2
    }

    public Kelpi_Phases kelpi_phase = Kelpi_Phases.KELPI_PHASE_1;

    [Header("Phase 1")]
    public Action_FollowPoint follow_point;
    public Action_Tail_Slash tail_slash;
    [Header("Phase 2")]
    public Action_SharkAttack shark_attack;

    [Header("Death State")]
    public Action_Dead dead;

    [Header("How many lives need to lose to change phase")]
    public int lives_to_change_phase = 4;

    private bool can_make_slash = false;
    private bool make_displacement = true;
    private bool make_shark_action = true;
    private bool is_dead = false;

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
        else if ((int)myBB.GetParameter("live") < (int)myBB.GetParameter("total_live") - lives_to_change_phase && kelpi_phase != Kelpi_Phases.KELPI_PHASE_2)
        {
            kelpi_phase = Kelpi_Phases.KELPI_PHASE_2;
            myBB.SetParameter("is_enemy_hit", false);
        }

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if (kelpi_phase == Kelpi_Phases.KELPI_PHASE_1) {
            if (currentAction != follow_point && make_displacement == true)
            {
                make_displacement = false;
                currentAction = follow_point;
                decide = true;
            }
            else if (currentAction != tail_slash && can_make_slash == true && (bool)myBB.GetParameter("is_enemy_hit") == false)
            {
                currentAction = tail_slash;
                decide = true;
            }
            else if (currentAction != follow_point && (bool)myBB.GetParameter("is_enemy_hit") == true)
            {
                tail_slash.Reset_Tail_Slash();
                currentAction = follow_point;
                myBB.SetParameter("is_enemy_hit", false);
                decide = true;
            }
        }
        else
        {
            if (currentAction != shark_attack && make_shark_action == true)
            {
                make_shark_action = false;
                currentAction = shark_attack;
                decide = true;
            }
            if (currentAction != dead && is_dead == true)
            {
                currentAction = dead;
                decide = true;
            }
        }

        return decide;
    }

    public void Set_Can_Make_Slash(bool new_state)
    {
        can_make_slash = new_state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_combat_collider"))
        {
            can_make_slash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player_combat_collider"))
        {
            can_make_slash = false;
        }
    }

}
