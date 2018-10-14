using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Soldier : BT_Entity {

    //current_action
    public Action_FollowPlayer chase;
    public GameObject player;

    override public void Update()
    {


        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if (currentAction != chase)
        {
            currentAction = chase;
            decide = true;
        }


        return decide;
    }

}
