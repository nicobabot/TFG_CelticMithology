using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Action_SharkAttack : ActionBase
{
    public Sprite shadow_sprite;
    public float follow_speed = 1.0f;
    public float time_following_player = 1.5f;
    public BoxCollider2D shark_collider;
    public LayerMask player_layer;

    SpriteRenderer sprite_rend;
    GameObject player;
    Sprite normal_sprite;
    Collider2D player_found;
    float timer_follow = 0.0f;
    bool shark_attack_done = false;

    override public BT_Status StartAction()
    {
        sprite_rend = gameObject.GetComponent<SpriteRenderer>();
        player = (GameObject)myBT.myBB.GetParameter("player");
        normal_sprite = sprite_rend.sprite;

        //Animation of kelpie going below water

        //Changing sprite to shadow below water
        sprite_rend.sprite = shadow_sprite;

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        if (shark_attack_done == false)
        {
            Vector3 player_pos = player.transform.position;
            Vector3 my_pos = transform.position;

            Vector3 diff = player_pos - my_pos;

            float step = Time.deltaTime * follow_speed;

            transform.position = Vector3.MoveTowards(my_pos, player_pos, follow_speed);

            if (diff.magnitude < 0.1f)
            {
                timer_follow += Time.deltaTime;

                if (timer_follow > time_following_player)
                {
                    sprite_rend.sprite = normal_sprite;
                    //Activate colliders
                    shark_collider.enabled = true;
                    player_found = Physics2D.OverlapBox(shark_collider.transform.position, shark_collider.size, 0.0f, player_layer);

                    shark_attack_done = true;
                    timer_follow = 0;
                }

            }
        }
        else
        {
            if (player_found != null)
            {
                shark_collider.enabled = false;
                sprite_rend.sprite = shadow_sprite;
                Transform parent = player_found.transform.parent;
                Player_Manager player_manager = parent.GetComponent<Player_Manager>();
                player_manager.GetDamage(transform);
                player_found = null;
                shark_attack_done = false;
            }
            else
            {
                //make kelpi fail attack and then wait seconds
            }
        }

        return BT_Status.RUNNING;
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawCube(shark_collider.transform.position, shark_collider.size);
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
