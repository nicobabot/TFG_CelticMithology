using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_SkeletonInvoke : ActionBase
{
    public CircleCollider2D colInvoke;
    public int numSkeletons;
    public GameObject skeletontObj;
    private GameObject player;
       
    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        for(int i=0; i< numSkeletons; i++)
        {
            GameObject go = Instantiate(skeletontObj);
            Vector2 circlePos = new Vector2(colInvoke.transform.position.x, colInvoke.transform.position.y);
            Vector2 newPos = Random.insideUnitCircle * colInvoke.radius + circlePos;
            go.transform.position = newPos;
        }

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
