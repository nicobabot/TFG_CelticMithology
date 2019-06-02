using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dagda_Blackboard))]
public class BT_Dagda : BT_Entity
{
    public enum Dagda_Phases
    {
        DAGDA_PHASE_1,
        DAGDA_PHASE_2
    }

    public Dagda_Phases dagda_phase = Dagda_Phases.DAGDA_PHASE_1;

    [Header("Phase 1")]
    public Action_FollowPlayer follow_player;
    public Action_MeleSlashPlayer slash_melee;
    public Action_PushBack pushback;

    [Header("Phase 2")]
    public Action_Escape escape;

    [Header("Death State")]
    public Action_DeadEnemySpawn dead;

    [Header("How many lives need to lose to change phase 1 -> phase 2")]
    public int lives_to_change_phase_2 = 4;

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
            dagda_phase = Dagda_Phases.DAGDA_PHASE_2;
        }

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;
        if ((bool)myBB.GetParameter("playerInsideRoom"))
        {
            if (currentAction == null)
            {
                if (dagda_phase == Dagda_Phases.DAGDA_PHASE_1)
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
                if (dagda_phase == Dagda_Phases.DAGDA_PHASE_2)
                {
                    if (currentAction != escape && !(bool)myBB.GetParameter("is_enemy_hit") == true)
                    {
                        currentAction = escape;
                        decide = true;
                    }
                    else if (currentAction != pushback && (bool)myBB.GetParameter("is_enemy_hit"))
                    {
                        int count = (int)myBB.GetParameter("pointdir");
                        count+=1;
                        myBB.SetParameter("pointdir", count);
                        myBB.SetParameter("changepath", true);
                        currentAction = pushback;
                        decide = true;
                    }
                    if (is_dead)
                    {
                        currentAction = dead;
                        decide = true;
                    }
                }
            }
        }

        return decide;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_combat_collider"))
        {
            if (currentAction != null)
            {
                currentAction.isFinish = true;
            }
            can_make_slash = true;
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