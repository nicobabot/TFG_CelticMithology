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
    public Action_MeleSlashPlayer slash_melee;
    public Action_PushBack pushback;

    [Header("Phase 2")]
    public Action_FollowPoint follow_point;
    public Action_InvokeEnemies invoke_enemies;

    [Header("Phase 3")]
    public Action_ChargeToPlayer charge;

    [Header("Death State")]
    public Action_Dead dead;

    [Header("How many lives need to lose to change phase 1 -> phase 2")]
    public int lives_to_change_phase_2 = 4;


    [Header("How many lives need to lose to change phase 2 -> phase 3")]
    public int lives_to_change_phase_3 = 4;

    private bool is_dead = false;
    private bool can_make_slash = false;
    private bool can_make_displacement = true;
    private bool can_invoke_enemies = false;


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
        if ((int)myBB.GetParameter("live") < (int)myBB.GetParameter("total_live") - lives_to_change_phase_2)
        {
            maclir_phase = MacLir_Phases.MACLIR_PHASE_2;
        }
        if ((int)myBB.GetParameter("live") < ((int)myBB.GetParameter("total_live") - (lives_to_change_phase_3 + lives_to_change_phase_2)))
        {
            maclir_phase = MacLir_Phases.MACLIR_PHASE_3;
        }

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if (currentAction == null)
        {
            if (maclir_phase == MacLir_Phases.MACLIR_PHASE_1)
            {
                if (currentAction != follow_player && can_make_slash == false && (bool)myBB.GetParameter("is_enemy_hit") == false)
                {
                    slash_melee.Disable_Colliders_Attack();
                    currentAction = follow_player;
                    decide = true;
                }
                else if (currentAction != slash_melee && can_make_slash == true && (bool)myBB.GetParameter("is_enemy_hit") == false)
                {
                    currentAction = slash_melee;
                    decide = true;
                }
                else if (currentAction != pushback && (bool)myBB.GetParameter("is_enemy_hit") == true)
                {
                    currentAction = pushback;
                    decide = true;
                }
            }
            if (maclir_phase == MacLir_Phases.MACLIR_PHASE_2)
            {
                if (currentAction != follow_point && (can_make_displacement == true || (bool)myBB.GetParameter("is_enemy_hit") == true) && can_invoke_enemies == false)
                {
                    can_make_displacement = false;
                    currentAction = follow_point;
                    myBB.SetParameter("is_enemy_hit", false);
                    decide = true;
                }
                else if (currentAction != invoke_enemies && can_invoke_enemies == true)
                {
                    can_invoke_enemies = false;
                    currentAction = invoke_enemies;
                    decide = true;
                }

            }
            if (maclir_phase == MacLir_Phases.MACLIR_PHASE_3)
            {
                if (currentAction != charge)
                {
                    currentAction = charge;
                    decide = true;
                }
            }
        }

        return decide;
    }

    public void Set_Invoke_State(bool invoke_state)
    {
        can_invoke_enemies = invoke_state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_combat_collider"))
        {
            if (maclir_phase == MacLir_Phases.MACLIR_PHASE_3)
            {
                if (currentAction == charge)
                {
                    myBB.SetParameter("player_detected_charging", true);
                }
            }
            else {
                if (currentAction != null)
                {
                    currentAction.isFinish = true;
                }
                can_make_slash = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player_combat_collider"))
        {
            if (currentAction != null)
            {
                currentAction.isFinish = true;
            }
            can_make_slash = false;
        }
    }

}