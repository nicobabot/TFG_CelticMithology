﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Action_FollowPlayer))]
public class Action_FollowPoint : ActionBase {

    public GameObject points_father;
    public float velocity = 1.0f;
    public GameObject inmortalGO;
    public BoxCollider2D col_detect_player;
    public BoxCollider2D collider_enemy;
    public bool moreThanOne = true;

    private Action_FollowPlayer follow_player_scr;
    private GameObject player;
    private Vector3 point_to_follow;

    private Vector3 last_point_followed;

    bool flip = false;

    private Animator myAnimator;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        follow_player_scr = GetComponent<Action_FollowPlayer>();

        if (moreThanOne)
        {
            do
            {
                int point = Random.Range(0, points_father.transform.childCount);
                point_to_follow = points_father.transform.GetChild(point).position;
            } while (point_to_follow == last_point_followed);

            last_point_followed = point_to_follow;
        }
        else point_to_follow = points_father.transform.position;

        //Make to water animation

            //Change sprite to behind water

        collider_enemy.enabled = false;
        col_detect_player.enabled = false;
        
        //mclir vicente
        if(myBT.enemy_type == Enemy_type.MACLIR_ENEMY)
        {
            myAnimator = (Animator)myBT.myBB.GetParameter("myAnimator");
            myAnimator.SetFloat("direction", 0.0f);
            myAnimator.SetBool("goin_to_invoke", true);
            myAnimator.SetBool("invokinasion", true);
        }

            return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        Vector3 dist_to_arrive = point_to_follow - transform.position;

        float step = Time.deltaTime * velocity;

        if(dist_to_arrive.magnitude < 0.1f)
        {
            //To change sprite
            Direction dir = follow_player_scr.DetectDirection(transform.position, player.transform.position);
            collider_enemy.enabled = true;
            col_detect_player.enabled = true;
            inmortalGO.SetActive(false);

            if (myBT.enemy_type == Enemy_type.MACLIR_ENEMY)
            {
                BT_MacLir mac_lir_bt = GetComponent<BT_MacLir>();
                mac_lir_bt.Set_Invoke_State(true);
                myAnimator.SetBool("goin_to_invoke", false);
            }
            else if(myBT.enemy_type == Enemy_type.MORRIGAN_ENEMY)
            {
                myBT.myBB.SetParameter("invokedCrows", false);
            }

            isFinish = true;
        }
        else
        {
            

            if(myBT.enemy_type == Enemy_type.MACLIR_ENEMY)
            {
                bool fliped = GetComponent<SpriteRenderer>().flipX;

                if (point_to_follow.x > transform.position.x && fliped == true)
                {
                   
                    GetComponent<SpriteRenderer>().flipX = false;
                }

                if (point_to_follow.x < transform.position.x && fliped == false)
                {

                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            else
            {
                if (point_to_follow.x < transform.position.x && flip == false)
                {
                    Debug.Log("Flipp");
                    GetComponent<SpriteRenderer>().flipX = true;
                    flip = true;
                }
                else
                {
                    if (point_to_follow.x > transform.position.x && flip == true)
                    {
                        Debug.Log(" no Flipp");
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, point_to_follow, step);
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
