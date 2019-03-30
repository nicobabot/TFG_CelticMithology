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

    public Animator anim;
    public GameObject player;

    override public void Update()
    {
        //Debug.Log(Vector3.Dot((player.transform.position - transform.position).normalized, Vector3.up));

        float value_y = Vector3.Dot((player.transform.position - transform.position).normalized, Vector3.up);
        float value_x = Vector3.Dot((player.transform.position - transform.position).normalized, Vector3.right);
        float direction = (value_y - (-1)) / (2);
        anim.SetFloat("enemy_direction", direction);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
