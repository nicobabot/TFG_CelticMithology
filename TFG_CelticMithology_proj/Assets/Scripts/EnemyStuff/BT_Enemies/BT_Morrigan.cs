using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Morrigan_Blackboard))]
public class BT_Morrigan : BT_Entity
{
    public enum PashesMorrigan
    {
        MORRIGAN_PHASE1,
        MORRIGAN_PHASE2,
        MORRIGAN_PHASE3
    }
    public PashesMorrigan phaseMorr;

    [Header("Phase 1")]
    public Action_CrowInvocation invokeCrows;
    public Action_FollowPlayer follow_player;
    public Action_FollowPoint followPoint;

    public Action_PushBack pushback;

    [Header("Death State")]
    public Action_Dead dead;

    [Header("How many lives need to lose to change phase 1 -> phase 2")]
    public int lives_to_change_phase_2 = 4;

    [Header("How many lives need to lose to change phase 2 -> phase 3")]
    public int lives_to_change_phase_3 = 6;

    private bool is_dead = false;
    private bool can_make_slash = false;
    private bool phaseTwoDone = false;
    private bool phaseThreeDone = false;

    private int crowMaxLive = 2;
    private int crowLive = 2;

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

        if ((int)myBB.GetParameter("live") < (int)myBB.GetParameter("total_live") - lives_to_change_phase_2 && !phaseTwoDone)
        {
            phaseMorr = PashesMorrigan.MORRIGAN_PHASE2;
            phaseTwoDone = true;
        }
        else if ((int)myBB.GetParameter("live") < (int)myBB.GetParameter("total_live") - lives_to_change_phase_2 - lives_to_change_phase_3 && !phaseThreeDone)
        {
            phaseMorr = PashesMorrigan.MORRIGAN_PHASE3;
            phaseThreeDone = true;
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
                if (phaseMorr == PashesMorrigan.MORRIGAN_PHASE1)
                {
                    if (currentAction != invokeCrows && (!(bool)myBB.GetParameter("invokedCrows")))
                    {
                        crowLive = crowMaxLive;
                        currentAction = invokeCrows;
                        decide = true;
                    }
                    if (currentAction != followPoint && (bool)myBB.GetParameter("invokedCrows") && crowLive <= 0)
                    {
                        currentAction = followPoint;
                        decide = true;
                    }
                    else if(currentAction != pushback && (bool)myBB.GetParameter("is_enemy_hit") == true)
                    {
                        crowLive--;
                        currentAction = pushback;
                        decide = true;
                    }
                    else if (currentAction != invokeCrows && (bool)myBB.GetParameter("invokedCrows") && crowLive > 0)
                    {
                        currentAction = follow_player;
                        decide = true;
                    }
                }
                else if (phaseMorr == PashesMorrigan.MORRIGAN_PHASE2)
                {
                    if (currentAction != pushback && (bool)myBB.GetParameter("is_enemy_hit") == true)
                    {
                        currentAction = pushback;
                        decide = true;
                    }
                }
                else if (phaseMorr == PashesMorrigan.MORRIGAN_PHASE3)
                {
                    if (currentAction != dead && is_dead == true)
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
              //  currentAction.isFinish = true;
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
               // currentAction.isFinish = true;
            }
            can_make_slash = false;
        }
    }

}
