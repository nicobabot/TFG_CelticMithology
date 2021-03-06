﻿using System.Collections.Generic;
using UnityEngine;

public class Action_FollowPlayerRanged : ActionBase
{
    public float time_to_change_cell = 1.0f;
    public float speed = 5.0f;
    public float range = 5.0f;
    private float timer_changing = 0.0f;
    private int cells_changed = 0;
    private List<PathNode> tiles_list;

    //Values calculate path
    private Vector3 destiny_pos = Vector3.zero;

    private Vector3Int cell_destiny_pos = Vector3Int.zero;
    private Vector3Int cell_pos = Vector3Int.zero;
    private PathNode actual_node;
    private GameObject player;

    private SpriteRenderer mySpriteRend;
    private Animator myAnimator;

    private float timeWaiting = 0.65f;
    private float timerWait = 0.0f;
    private bool waitOnce = false;
    private bool canMove = true;


    bool can_reach = true;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        mySpriteRend = (SpriteRenderer)myBT.myBB.GetParameter("mySpriteRend");

        myAnimator = (Animator)myBT.myBB.GetParameter("myAnimator");

        timerWait = 0.0f;

        if (myAnimator != null && myBT.enemy_type != Enemy_type.KELPIE_ENEMY)
        {
            myAnimator.SetBool("enemy_startwalking", true);
            myAnimator.SetFloat("offsetAnimation", Random.Range(0.0f, 1.0f));
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        if ((player.transform.position - transform.position).magnitude > range)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            Direction mydir = DetectDirection(transform.position, player.transform.position);

            if (mydir == Direction.RIGHT && mySpriteRend != null)
            {
                mySpriteRend.flipX = false;
            }
            else if (mydir == Direction.LEFT && mySpriteRend != null)
            {
                mySpriteRend.flipX = true;
            }
        }

        return BT_Status.RUNNING;
    }

    bool WaitFirstAnimationFinished()
    {
        bool ret = false;

        timerWait += Time.deltaTime;

        if (timerWait>= timeWaiting)
        {
            ret = true;
        }

        return ret;
    }

    public Direction DetectDirection(Vector3 my_position, Vector3 new_position)
    {
        Direction dir_x = Direction.NEUTRAL;
        Direction dir_y = Direction.NEUTRAL;

        float x = my_position.x;
        float y = my_position.y;

        if (x < new_position.x) dir_x = Direction.RIGHT;
        else dir_x = Direction.LEFT;

        if (y < new_position.y) dir_y = Direction.UP;
        else dir_y = Direction.DOWN;

        float dif_x = Mathf.Abs(x - new_position.x);
        float dif_y = Mathf.Abs(y - new_position.y);

        if (dif_x > dif_y)
        {
            dir_y = Direction.NEUTRAL;
            //Debug.Log("Going " + dir_x.ToString());
            myBT.myBB.SetParameter("direction", dir_x);
            return dir_x;
        }
        else
        {
            dir_x = Direction.NEUTRAL;
            //Debug.Log("Going " + dir_y.ToString());
            myBT.myBB.SetParameter("direction", dir_y);
            return dir_y;
        }
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}