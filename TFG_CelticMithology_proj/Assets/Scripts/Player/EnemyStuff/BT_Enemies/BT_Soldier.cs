using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Soldier : BT_Entity {

    //current_action
    //[SerializeField]private ActionBase patroll;



    override public void Update()
    {


        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        /*if (currentAction != chase_player && this.myBB.GetParameter("player_transform") != null && (bool)this.myBB.GetParameter("ready_to_attack")==false)
        {
            currentAction = chase_player;
            decide = true;
        }*/


        return decide;
    }

}
