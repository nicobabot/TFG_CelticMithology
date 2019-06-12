using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Caorthannach : BT_Entity
{
    //current_action
    public Action_FollowPlayer chase;
    public Action_ShootPlayer shoot;
    public Action_Dead dead;
    public Action_DamageStatic damage_static;
    private bool is_dead = false;

    public SpriteRenderer rendSprite;
    public Animator anim;

    override public void Update()
    {
        //Debug.Log(Vector3.Dot((player.transform.position - transform.position).normalized, Vector3.up));

        float value_y = Vector3.Dot((((GameObject)myBB.GetParameter("player")).transform.position - transform.position).normalized, Vector3.up);
        float value_x = Vector3.Dot((((GameObject)myBB.GetParameter("player")).transform.position - transform.position).normalized, Vector3.right);
        float direction = (value_y - (-1)) / (2);
        //anim.SetFloat("enemy_direction", direction);
        DetectDirection(transform.position,((GameObject)myBB.GetParameter("player")).transform.position);


        //Debug.Log(value_x);

        if (value_x < 0.0f)
             GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;

        if ((int)myBB.GetParameter("live") <= 0)
        {
            //Animation of enemy dying
            if (currentAction != null)
            {
                currentAction.isFinish = true;
            }
            is_dead = true;
        }

        base.Update();
    }

    override public bool MakeDecision()
    {
        bool decide = false;

        if ((bool)myBB.GetParameter("playerInsideRoom"))
        {
            if (currentAction != shoot && (bool)myBB.GetParameter("is_enemy_hit") == false && is_dead == false)
            {
                currentAction = shoot;
                decide = true;
            }
            else if ((bool)myBB.GetParameter("is_enemy_hit") == true && is_dead == false)
            {
                rendSprite.color = Color.white;
                currentAction = damage_static;
                decide = true;
            }
            else if (currentAction != dead && is_dead == true)
            {
                currentAction = dead;
                decide = true;
            }
        }

        return decide;
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
            anim.SetFloat("enemy_direction", 0.5f);
            return dir_x;
        }
        else
        {
            dir_x = Direction.NEUTRAL;
            //Debug.Log("Going " + dir_y.ToString());
            if(dir_y == Direction.UP)
                anim.SetFloat("enemy_direction", 1.0f);
            else if (dir_y == Direction.DOWN)
                anim.SetFloat("enemy_direction", 0.0f);

            return dir_y;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
