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
    public Action_SharkAttack shark_attack;
    [Header("Phase 2")]
    public Action_ThrowBouncingBalls throwBall;

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

        if ((bool)myBB.GetParameter("playerInsideRoom"))
        {

                if (currentAction != shark_attack && !(bool)myBB.GetParameter("shootBall"))
                {
                    currentAction = shark_attack;
                    decide = true;
                }
                if (currentAction != throwBall && (bool)myBB.GetParameter("shootBall"))
                {
                    myBB.SetParameter("shootBall", false);
                    currentAction = throwBall;
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
