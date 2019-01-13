using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_PushBack : ActionBase {

    public float push_distance = 1.0f;
    public float time_doing_pushback=0.5f;

    float timer_pushback;
    GameObject player;
    Vector3 pushback_dir = Vector3.zero;
    bool is_pushback_done = false;
    bool can_make_pushback = false;
    Rigidbody2D rb;

    override public BT_Status StartAction()
    {

        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_PushBack");
        }

        rb = gameObject.GetComponent<Rigidbody2D>();

        //Reverse direction to make the pushback
        pushback_dir = transform.position - player.transform.position;
        pushback_dir = pushback_dir.normalized * push_distance;

        Vector3 new_pos = transform.position + pushback_dir;
        Pathfinder path_scr = myBT.pathfinder_scr;

        Vector3Int new_pos_cell = path_scr.walkability.LocalToCell(new_pos);
        can_make_pushback = path_scr.IsWalkableTile(new_pos_cell.x, new_pos_cell.y);

        is_pushback_done = false;
        timer_pushback = 0.0f;

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        if (!can_make_pushback)
        {
            myBT.myBB.SetParameter("is_enemy_hit", false);
            isFinish = true;
            return BT_Status.RUNNING;
        }

        rb.velocity = pushback_dir;

        timer_pushback += Time.deltaTime;

        if(timer_pushback > time_doing_pushback)
        {
            rb.velocity = Vector2.zero;
            myBT.myBB.SetParameter("is_enemy_hit", false);
            isFinish = true;
        }

        return BT_Status.RUNNING;
    }


    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }

}
