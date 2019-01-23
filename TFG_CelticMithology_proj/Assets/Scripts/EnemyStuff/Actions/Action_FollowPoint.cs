﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Action_FollowPlayer))]
public class Action_FollowPoint : ActionBase {

    public GameObject points_father;
    public float velocity = 1.0f;
    public BoxCollider2D col_detect_player;

    private Action_FollowPlayer follow_player_scr;
    private GameObject player;
    private BoxCollider2D collider_enemy;
    private Vector3 point_to_follow;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        follow_player_scr = GetComponent<Action_FollowPlayer>();
        collider_enemy = GetComponent<BoxCollider2D>();

        int point = Random.Range(0, points_father.transform.childCount);

        point_to_follow = points_father.transform.GetChild(point).position;

        //Make to water animation

        //Change sprite to behind water

        collider_enemy.enabled = false;
        col_detect_player.enabled = false;

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
            isFinish = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, point_to_follow, step);
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}