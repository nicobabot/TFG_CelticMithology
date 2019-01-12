using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_MeleeAttack : ActionBase {

    override public BT_Status StartAction()
    {


        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {


        return BT_Status.RUNNING;
    }


    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }
}
