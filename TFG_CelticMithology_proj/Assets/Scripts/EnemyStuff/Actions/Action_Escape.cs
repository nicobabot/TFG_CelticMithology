using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Escape : ActionBase
{
    public float speed;
    public Transform[] PointsToEscape;
    private GameObject player;
    SpriteRenderer mySprite;
    int counterPoint = 0;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        mySprite = (SpriteRenderer)myBT.myBB.GetParameter("mySpriteRend");

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        if (counterPoint < PointsToEscape.Length && counterPoint >= 0)
        {
            Transform temp = PointsToEscape[counterPoint];

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, temp.localPosition, speed * Time.deltaTime);

            if ((transform.localPosition - temp.localPosition).magnitude < 0.5f || (bool)myBT.myBB.GetParameter("changepath"))
            {

                int dirPoint = (int)myBT.myBB.GetParameter("pointdir");

                if (dirPoint % 2 == 0)
                {
                    counterPoint++;
                    if (counterPoint >= PointsToEscape.Length)
                    {
                        counterPoint = 0;
                    }
                }
                else
                {
                    counterPoint--;
                    if (counterPoint < 0)
                    {
                        counterPoint = PointsToEscape.Length - 1;
                    }
                }

                myBT.myBB.SetParameter("changepath", false);

            }
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }

}
