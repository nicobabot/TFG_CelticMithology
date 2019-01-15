using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_PushBack : ActionBase {

    public float push_distance = 1.0f;
    public float time_doing_pushback=0.5f;
    public float min_distance = 0.1f;

    float timer_pushback;
    GameObject player;
    Vector3 pushback_dir = Vector3.zero;
    bool is_pushback_done = false;
    bool can_make_pushback = false;
    bool is_there_wall = false;
    Vector3 hitpoint_wall;
    Rigidbody2D rb;

    Vector3 temp_position;
    Vector3 temp_position_player;

    override public BT_Status StartAction()
    {

        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_PushBack");
        }

        rb = gameObject.GetComponent<Rigidbody2D>();

        //Reverse direction to make the pushback
        SpriteRenderer sprite_rend = GetComponent<SpriteRenderer>();
        if (sprite_rend == null)
        {
            Debug.Log("Sprite renderer null _Action_PushBack");
        }

        float size_addition = (sprite_rend.size.y * 0.5f) * 0.5f;
        temp_position = transform.position;
        temp_position.y += size_addition;


        SpriteRenderer sprite_rend_player = player.GetComponent<SpriteRenderer>();
        if (sprite_rend_player == null)
        {
            Debug.Log("Sprite renderer null _Action_PushBack");
        }

        float size_addition_player = (sprite_rend_player.size.y * 0.5f) * 0.5f;
        temp_position_player = player.transform.position;
        temp_position_player.y += size_addition_player;

        pushback_dir = temp_position - temp_position_player;
        pushback_dir = pushback_dir.normalized * push_distance;



        RaycastHit2D[] hit = Physics2D.RaycastAll(temp_position, pushback_dir.normalized, push_distance);
        if (hit != null)
        {
            foreach(RaycastHit2D hit_t in hit)
            if (hit_t.transform.CompareTag("wall"))
            {
                is_there_wall = true;
                hitpoint_wall = hit_t.point;
                break;
            }
        }


        is_pushback_done = false;
        timer_pushback = 0.0f;

        return BT_Status.RUNNING;
    }

    private void OnDrawGizmos()
    {
        /*player = (GameObject)myBT.myBB.GetParameter("player");
        pushback_dir = transform.position - player.transform.position;
        pushback_dir = pushback_dir.normalized * push_distance;
        Gizmos.DrawLine(transform.position, transform.position + pushback_dir);*/
        Gizmos.DrawLine(temp_position, temp_position + pushback_dir);
        Gizmos.DrawWireSphere(hitpoint_wall, min_distance);
    }

    override public BT_Status UpdateAction()
    {

        Vector3 direction = temp_position - hitpoint_wall;

        if (direction.magnitude < min_distance)
        {
            rb.velocity = Vector2.zero;
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
