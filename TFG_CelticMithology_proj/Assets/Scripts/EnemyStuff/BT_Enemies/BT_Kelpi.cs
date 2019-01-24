using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Kelpi_Blackboard))]
public class BT_Kelpi : BT_Entity
{

    enum Kelpi_Phases
    {
        KELPI_PHASE_1,
        KELPI_PHASE_2
    }

    public Action_FollowPoint follow_point;
    public Action_Tail_Slash tail_slash;

    Kelpi_Phases kelpi_phase = Kelpi_Phases.KELPI_PHASE_1;

    [Header("How many lives need to lose to change phase")]
    public int lives_to_change_phase = 4;

    private bool can_make_slash = false;
    private bool make_displacement = true;

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
        else if ((int)myBB.GetParameter("live") < (int)myBB.GetParameter("total_live") - lives_to_change_phase)
        {
            kelpi_phase = Kelpi_Phases.KELPI_PHASE_2;
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
