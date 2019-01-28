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

        dir_collider = follow_player_scr.DetectDirection(transform.position, player.transform.position);
        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        timer_attack += Time.deltaTime;
        GameObject go = father_colliders.transform.GetChild((int)dir_collider).gameObject;

        get_damage_collider.enabled = false;
        go.SetActive(true);
        BoxCollider2D col = go.GetComponent<BoxCollider2D>();
        player_detection_slash = Physics2D.OverlapBox(go.transform.position, col.size, 0, player_mask);

        if (player_detection_slash != null)
        {
            //damage player
        }

        if (timer_attack > time_to_make_slash)
        {
            timer_attack = 0.0f;
            isFinish = true;
        }
        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
