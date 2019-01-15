using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_MeleeAttack : ActionBase {

    public float time_to_attack = 0.5f;
    float timer_to_attack = 0.5f;
    GameObject player;

    override public BT_Status StartAction()
    {
        timer_to_attack = time_to_attack;
        player = (GameObject)myBT.myBB.GetParameter("player");

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        //need to know the animation timing
        timer_to_attack += Time.deltaTime;

        if (timer_to_attack > time_to_attack)
        {
            //Damage player

            timer_to_attack = 0;
        }
        
        

        return BT_Status.RUNNING;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }

    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }
}
