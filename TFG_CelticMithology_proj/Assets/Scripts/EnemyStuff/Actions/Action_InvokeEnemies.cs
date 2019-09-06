using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_InvokeEnemies : ActionBase
{
    public GameObject melee_enemy;
    public GameObject ImortalGO;
    public BoxCollider2D collider_enemy;
    public uint num_enemy_spawn = 5;
    public float time_to_spawn_next_enemy = 0.0f;
    public float radius = 2.0f;

    private GameObject player;

    private float total_time_spawning = 0.0f;

    private float timer_spawning = 0.0f;
    private float timer_spawning_other_enemy = 0.0f;

    private Animator myAnimator;

    override public BT_Status StartAction()
    {
        myAnimator = (Animator)myBT.myBB.GetParameter("myAnimator");

        if (myAnimator)
        {
            if(myBT.enemy_type == Enemy_type.MORRIGAN_ENEMY)
                myAnimator.SetInteger("Phase", 2);
            else myAnimator.SetInteger("Phase", 1);

            myAnimator.SetBool("invokinasion", true);
        }

        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        total_time_spawning = num_enemy_spawn * time_to_spawn_next_enemy;
        timer_spawning = 0.0f;
        timer_spawning_other_enemy = 0.0f;

        collider_enemy.enabled = false;
        //ImortalGO.SetActive(true);
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

                if (myBT.enemy_type == Enemy_type.MACLIR_ENEMY)
                {
                    Banshee_Blackboard soldierBt = temp_go.GetComponent<Banshee_Blackboard>();
                    soldierBt.playerIsInsideRoom.SetValue(true);
                }
                else
                {
                    Soldier_Blackboard soldierBt = temp_go.GetComponent<Soldier_Blackboard>();
                    soldierBt.playerIsInsideRoom.SetValue(true);
                }

                
                temp_go.SetActive(true);
                temp_go.transform.position = point_to_spawn;

            }
        }
        else
        {
            isFinish = true;
            collider_enemy.enabled = true;
           
           
            ImortalGO.SetActive(false);
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        myAnimator.SetBool("invokinasion", false);

        return BT_Status.SUCCESS;
    }
}
