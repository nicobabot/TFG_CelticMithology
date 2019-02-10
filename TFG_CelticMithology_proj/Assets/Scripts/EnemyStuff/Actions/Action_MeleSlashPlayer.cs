using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent(typeof(Action_FollowPlayer))]
public class Action_MeleSlashPlayer : ActionBase
{

    public float time_to_make_slash;
    public GameObject father_colliders;
    public LayerMask player_mask;

    [Header("The collider that player detects to make damage the enemy")]
    public BoxCollider2D get_damage_collider;

    private Action_FollowPlayer follow_player_scr;
    private GameObject player;
    private float timer_attack = 0.0f;
    private bool slash_done = false;
    private bool is_player_detected = false;
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
        player_detection_slash = null;
        is_player_detected = false;
        timer_attack = 0.0f;
        dir_collider = follow_player_scr.DetectDirection(transform.position, player.transform.position);
        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        timer_attack += Time.deltaTime;
        GameObject go = father_colliders.transform.GetChild((int)dir_collider).gameObject;

        //get_damage_collider.enabled = false;
        go.SetActive(true);
        BoxCollider2D col = go.GetComponent<BoxCollider2D>();
        player_detection_slash = Physics2D.OverlapBox(go.transform.position, col.size, 0, player_mask);

        if (player_detection_slash != null)
        {
            Transform parent = player_detection_slash.transform.parent;
            if (parent != null)
            {
                is_player_detected = true;
                Player_Manager player_manager = parent.GetComponent<Player_Manager>();
                player_manager.GetDamage(transform);
            }

        }

        if (timer_attack > time_to_make_slash || is_player_detected == true)
        {
            is_player_detected = false;
            Disable_Colliders_Attack();
            go.SetActive(false);
            player_detection_slash = null;
            timer_attack = 0.0f;
            //get_damage_collider.enabled = true;
            return BT_Status.SUCCESS;
        }
        return BT_Status.RUNNING;
    }

    public void Disable_Colliders_Attack()
    {
        int num_col = father_colliders.transform.childCount;

        for (int i=0; i < num_col; i++)
        {
            father_colliders.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
