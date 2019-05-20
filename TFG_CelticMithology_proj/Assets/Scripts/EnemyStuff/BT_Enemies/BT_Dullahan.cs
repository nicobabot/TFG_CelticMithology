using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dullahan_Blackboard))]
public class BT_Dullahan : BT_Entity
{

    public enum PashesDullahan
    {
        DULLAHAN_PHASE1,
        DULLAHAN_PHASE2
    }
    public PashesDullahan phaseDull;

    [Header("Phase 1")]
    public Action_FollowPlayer follow_player;
    public Action_MeleSlashPlayer slash_melee;
    public Action_PushBack pushback;
    public Action_WanderAttack wanderAttack;

    [Header("Death State")]
    public Action_DeadBoss dead;

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

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;
        if ((bool)myBB.GetParameter("playerInsideRoom"))
        {
            if (currentAction == null)
            {
                if (phaseDull == PashesDullahan.DULLAHAN_PHASE1)
                {
                    if (currentAction != wanderAttack && (bool)myBB.GetParameter("is_enemy_hit") == false)
                    {
                        currentAction = wanderAttack;
                        decide = true;
                    }
                    else if (currentAction != pushback && (bool)myBB.GetParameter("is_enemy_hit") == true)
                    {
                        currentAction = pushback;
                        decide = true;
                    }
                }
                else if (phaseDull == PashesDullahan.DULLAHAN_PHASE2)
                {
                    if (currentAction != follow_player && (bool)myBB.GetParameter("is_enemy_hit") == false)
                    {
                        currentAction = follow_player;
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
