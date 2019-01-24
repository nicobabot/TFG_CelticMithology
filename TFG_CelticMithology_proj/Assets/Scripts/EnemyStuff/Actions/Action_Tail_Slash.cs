using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Tail_Slash : ActionBase {

    public float time_to_make_slash;
    public float time_waiting_to_get_damage;
    public GameObject father_colliders;
    public LayerMask player_mask;
    public Fader fader_scr;

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
            if (player_detection_slash != null)
            {
                Transform parent = player_detection_slash.transform.parent;

                if (parent != null)
                {
                    Live_Manager live_manager_scr = parent.GetComponent<Live_Manager>();
                    Player_Manager player_manager_scr = parent.GetComponent<Player_Manager>();

                    if (live_manager_scr != null && player_manager_scr != null)
                    {
                        go.SetActive(false);
                        player_manager_scr.Set_Enemy_Pushback(transform);
                        player_manager_scr.current_state = Player_Manager.Player_States.PUSHBACK_PLAYER;
                        fader_scr.Fade_image.enabled = true;
                        fader_scr.FadeOut(false, true);
                        live_manager_scr.DetectedDamage();
                        player_detection_slash = null;
                    }
                }
            }
            else
            {
                //Stop
            }
        }


        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
