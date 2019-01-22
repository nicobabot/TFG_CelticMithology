using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Kelpi_Blackboard))]
public class BT_Kelpi : BT_Entity
{

    public Action_FollowPoint follow_point;
    public Action_Tail_Slash tail_slash;
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

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if (currentAction != follow_point && make_displacement == true)
        {
            make_displacement = false;
            currentAction = follow_point;
            decide = true;
        }
        else if (currentAction != tail_slash && can_make_slash == true)
        {
            currentAction = tail_slash;
            decide = true;
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
