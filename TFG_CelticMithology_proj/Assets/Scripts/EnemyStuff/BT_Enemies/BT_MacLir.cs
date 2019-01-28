using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MacLir_Blackboard))]
public class BT_MacLir : BT_Entity
{

    public enum MacLir_Phases
    {
        MACLIR_PHASE_1,
        MACLIR_PHASE_2,
        MACLIR_PHASE_3
    }

    public MacLir_Phases maclir_phase = MacLir_Phases.MACLIR_PHASE_1;

    [Header("Phase 1")]
    public Action_FollowPlayer follow_player;

    [Header("Phase 2")]
    public Action_FollowPoint follow_point;


    [Header("Death State")]
    public Action_Dead dead;

    [Header("How many lives need to lose to change phase 1 -> phase 2")]
    public int lives_to_change_phase_2 = 4;


    [Header("How many lives need to lose to change phase 2 -> phase 3")]
    public int lives_to_change_phase_3 = 4;

    private bool is_dead = false;
    private bool can_make_slash = false;

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
        else if ((int)myBB.GetParameter("live") < (int)myBB.GetParameter("total_live") - lives_to_change_phase_2 && maclir_phase != MacLir_Phases.MACLIR_PHASE_1)
        {
            maclir_phase = MacLir_Phases.MACLIR_PHASE_2;
        }
        else if ((int)myBB.GetParameter("live") < (int)myBB.GetParameter("total_live") - lives_to_change_phase_2 && maclir_phase != MacLir_Phases.MACLIR_PHASE_2)
        {
            maclir_phase = MacLir_Phases.MACLIR_PHASE_3;
        }

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if (maclir_phase == MacLir_Phases.MACLIR_PHASE_1)
        {
            if (currentAction != follow_player)
            {
                //make_displacement = false;
                currentAction = follow_player;
                decide = true;
            }
        
        }

        return decide;
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