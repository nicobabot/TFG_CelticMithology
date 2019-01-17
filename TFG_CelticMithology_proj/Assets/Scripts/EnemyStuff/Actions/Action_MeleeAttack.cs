using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_MeleeAttack : ActionBase {

    public float time_to_attack = 0.5f;
    public LayerMask player_layer;
    public Fader fader_scr;
    float timer_to_attack = 0.5f;
    GameObject player;
    BoxCollider2D collider;
    Live_Manager live_manager_scr;

    override public BT_Status StartAction()
    {
        timer_to_attack = time_to_attack;
        player = (GameObject)myBT.myBB.GetParameter("player");

        bool use_horizontal = false;

        Direction dir = (Direction)myBT.myBB.GetParameter("direction");

        Direction dir_x = Direction.NEUTRAL;
        Direction dir_y = Direction.NEUTRAL;

        float x = transform.position.x;
        float y = transform.position.y;

        if (x < player.transform.position.x) dir_x = Direction.RIGHT;
        else dir_x = Direction.LEFT;
        

        if(y < player.transform.position.y) dir_y = Direction.UP;
        else dir_y = Direction.DOWN;


        float dif_x = Mathf.Abs(x - player.transform.position.x);
        float dif_y = Mathf.Abs(y - player.transform.position.y);

        if (dif_x > dif_y)
        {
            dir_y = Direction.NEUTRAL;
            use_horizontal = true;
        }
        else
        {
            dir_x = Direction.NEUTRAL;
        }

        if (transform.childCount > 0)
        {
            if (transform.GetChild(0).childCount > 0) {
                if (use_horizontal) {
                    collider = transform.GetChild(0).GetChild((int)dir_x).GetComponent<BoxCollider2D>();
                }
                else
                {
                    collider = transform.GetChild(0).GetChild((int)dir_y).GetComponent<BoxCollider2D>();
                }

            }
        }

        timer_to_attack = 0;

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        if(collider == null)
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
                fader_scr.FadeOut(false,true);

                Transform parent = col_temp.transform.parent;
                if (parent != null) {

                    Player_Manager player_manager = parent.GetComponent<Player_Manager>();
                    player_manager.current_state = Player_Manager.Player_States.PUSHBACK_PLAYER;
                    Player_PushBack pushback_player = parent.GetComponent<Player_PushBack>();
                    if (pushback_player != null)
                    {
                        pushback_player.enemy_pos = transform;
                    }
                    else
                    {
                        Debug.Log("pushback not detected _Action_MeleeAttack");
                    }

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
