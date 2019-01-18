using UnityEngine;

[RequireComponent(typeof(Action_FollowPlayer))]
public class Action_MeleeAttack : ActionBase
{
    public float time_to_attack = 0.5f;
    public LayerMask player_layer;
    public Fader fader_scr;
    private float timer_to_attack = 0.5f;
    private GameObject player;
    private BoxCollider2D collider;

    private Live_Manager live_manager_scr;
    private Action_FollowPlayer follow_player_scr;

    override public BT_Status StartAction()
    {
        timer_to_attack = time_to_attack;
        player = (GameObject)myBT.myBB.GetParameter("player");
        follow_player_scr = GetComponent<Action_FollowPlayer>();

        Direction dir = follow_player_scr.DetectDirection(transform.position, player.transform.position);

        if (transform.childCount > 0)
        {
            if (transform.GetChild(0).childCount > 0)
            {
                collider = transform.GetChild(0).GetChild((int)dir).GetComponent<BoxCollider2D>();
            }
        }

        timer_to_attack = 0;

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        if (collider == null)
        {
            Debug.Log("No collider there! _Action_MeleeAttack");
            return BT_Status.RUNNING;
        }

        //need to know the animation timing
        timer_to_attack += Time.deltaTime;

        if (timer_to_attack > time_to_attack)
        {
            Collider2D col_temp = Physics2D.OverlapBox(collider.transform.position, collider.size, 0.0f, player_layer);

            if (col_temp != null)
            {
                Debug.Log("Player damaged!");
                fader_scr.Fade_image.enabled = true;
                fader_scr.FadeOut(false, true);

                Transform parent = col_temp.transform.parent;
                if (parent != null)
                {
                    Player_Manager player_manager = parent.GetComponent<Player_Manager>();
                    player_manager.current_state = Player_Manager.Player_States.PUSHBACK_PLAYER;
                    player_manager.Set_Enemy_Pushback(transform);
                    live_manager_scr = parent.GetComponent<Live_Manager>();
                    if (live_manager_scr != null)
                    {
                        live_manager_scr.DetectedDamage();
                    }
                    else
                    {
                        Debug.Log("Live Manager not found _Action_MeleeAttack");
                    }
                }
                else
                {
                    Debug.Log("Parent not found _Action_MeleeAttack");
                }
            }
            col_temp = null;
            timer_to_attack = 0;
        }

        return BT_Status.RUNNING;
    }

    private void OnDrawGizmos()
    {
        if (collider != null)
            Gizmos.DrawCube(collider.transform.position, collider.size);
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}