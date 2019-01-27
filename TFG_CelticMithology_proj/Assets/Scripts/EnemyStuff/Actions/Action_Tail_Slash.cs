using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Tail_Slash : ActionBase {

    public float time_to_make_slash;
    public float time_waiting_to_get_damage;
    public GameObject father_colliders;
    public LayerMask player_mask;
    public Fader fader_scr;

    [Header("The collider that player detects to make damage the enemy")]
    public BoxCollider2D get_damage_collider;

    private Action_FollowPlayer follow_player_scr;
    private GameObject player;
    private Vector3 point_to_follow;
    private float timer_attack = 0.0f;
    private bool slash_done = false;
    private BoxCollider2D collider_enemy;
    private Collider2D player_detection_slash;
    private Direction dir_collider;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        follow_player_scr = GetComponent<Action_FollowPlayer>();
        collider_enemy = GetComponent<BoxCollider2D>();

        dir_collider = follow_player_scr.DetectDirection(transform.position, player.transform.position);
        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        timer_attack += Time.deltaTime;
        GameObject go = father_colliders.transform.GetChild((int)dir_collider).gameObject;
        if (!slash_done)
        {
            get_damage_collider.enabled = false;
            go.SetActive(true);
            BoxCollider2D col = go.GetComponent<BoxCollider2D>();
            player_detection_slash = Physics2D.OverlapBox(go.transform.position, col.size, 0, player_mask);

            if (timer_attack > time_to_make_slash)
            {
                timer_attack = 0.0f;
                slash_done = true;
            }
        }
        else
        {
            go.SetActive(false);
            if (player_detection_slash != null)
            {
                Transform parent = player_detection_slash.transform.parent;

                if (parent != null)
                {
                    Player_Manager player_manager_scr = parent.GetComponent<Player_Manager>();

                    if (player_manager_scr != null)
                    {
                        player_manager_scr.GetDamage(transform);
                        slash_done = false;
                        player_detection_slash = null;
                        BT_Kelpi kelpi_bt = ((BT_Kelpi)myBT);
                        if (kelpi_bt != null) kelpi_bt.Set_Can_Make_Slash(false);
                    }
                }
            }
            else
            {
                //Stop
                get_damage_collider.enabled = true;
            }
            isFinish = true;

        }


        return BT_Status.RUNNING;
    }

    public void Reset_Tail_Slash()
    {
        slash_done = false;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
