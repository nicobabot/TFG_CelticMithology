using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_InvokeEnemies : ActionBase
{
    public GameObject melee_enemy;
    public uint num_enemy_spawn = 5;
    public float time_to_spawn_next_enemy = 0.0f;
    public float radius = 2.0f;

    private GameObject player;

    private float total_time_spawning = 0.0f;

    private float timer_spawning = 0.0f;
    private float timer_spawning_other_enemy = 0.0f;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        total_time_spawning = num_enemy_spawn * time_to_spawn_next_enemy;
        timer_spawning = 0.0f;
        timer_spawning_other_enemy = 0.0f;

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        timer_spawning += Time.deltaTime;

        timer_spawning_other_enemy += Time.deltaTime;

        if (timer_spawning < total_time_spawning)
        {

            if (timer_spawning_other_enemy > time_to_spawn_next_enemy)
            {

                timer_spawning_other_enemy = 0;
                Vector2 point_to_spawn = transform.position + Random.insideUnitSphere * radius;
                GameObject temp_go = Instantiate(melee_enemy);
                temp_go.SetActive(true);
                temp_go.transform.position = point_to_spawn;

            }
        }
        else
        {
            isFinish = true;
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
