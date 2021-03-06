﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Action_SharkAttack : ActionBase
{

    enum Kelpi_State
    {
        FOLLOWING_PLAYER,
        STUNNED,
    }

    Kelpi_State state;

    public Sprite shadow_sprite;
    public float follow_speed = 1.0f;
    public float time_following_player = 1.5f;
    public float distance_to_arrive_player = 0.5f;
    public BoxCollider2D shark_collider;

    [Tooltip("By code will deactivate this collider to led kelpi follow the player")]
    public BoxCollider2D get_damage_collider;
    public BoxCollider2D player_damage_collider;
    public LayerMask player_layer;

    [Header("Stun kelpi fail shark bite")]
    public float time_stunned = 0.75f;
    public Image stun_filler;
    private float timer_stunned_count = 0.0f;

    SpriteRenderer sprite_rend;
    SpriteRenderer sprite_rend_player;
    GameObject player;
    Sprite normal_sprite;
    Collider2D player_found;
    float timer_follow = 0.0f;

    bool animationAttackDone = false;
    float timerAnimationAttack = 0.0f;

    bool shark_attack_done = false;

    private Animator myAnimator;

    float standard_y_sprite_player;
    float standard_y_sprite_enemy;
    float sprite_enemy_x = 0;

    override public BT_Status StartAction()
    {
        state = Kelpi_State.FOLLOWING_PLAYER;

        sprite_rend = gameObject.GetComponent<SpriteRenderer>();
        player = (GameObject)myBT.myBB.GetParameter("player");
        sprite_rend_player = player.GetComponent<SpriteRenderer>();
        normal_sprite = sprite_rend.sprite;


        myAnimator = (Animator)myBT.myBB.GetParameter("myAnimator");

        standard_y_sprite_player = sprite_rend_player.bounds.size.y;
        standard_y_sprite_enemy = sprite_rend.bounds.size.y;

        //Animation of kelpie going below water

        //Changing sprite to shadow below water
        sprite_rend.sprite = shadow_sprite;
        shark_attack_done = false;
        timer_stunned_count = 0;
        player_damage_collider.enabled = false;
        get_damage_collider.enabled = false;
        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        if (shark_attack_done == false)
        {

            if (!animationAttackDone)
            {
                Vector3 player_pos = player.transform.position;
                player_pos.y += standard_y_sprite_player * 0.5f;
                player_pos.y -= standard_y_sprite_enemy * 0.5f;
                Vector3 my_pos = transform.position;

                Vector3 diff = player_pos - my_pos;

                float step = Time.deltaTime * follow_speed;

                transform.position = Vector3.MoveTowards(my_pos, player_pos, follow_speed);

                // if (diff.magnitude < distance_to_arrive_player)
                //{
                timer_follow += Time.deltaTime;

                sprite_rend.size += new Vector2(50, 50);


                if (timer_follow > time_following_player)
                {
                    //sprite_rend.sprite = normal_sprite;
                    //Activate colliders
                    GoUnderground(false);
                    timerAnimationAttack += Time.deltaTime;

                    if (timerAnimationAttack >= 0.20f)
                    {
                        timerAnimationAttack = 0;
                        animationAttackDone = true;
                    }
                }
            }
            else
            {
                timerAnimationAttack += Time.deltaTime;
                if (timerAnimationAttack >= 0.40f || player_found != null)
                {
                    shark_collider.enabled = true;
                    player_found = Physics2D.OverlapBox(shark_collider.transform.position, shark_collider.size, 0.0f, player_layer);
                    timer_follow = 0;

                    if (timerAnimationAttack >= 1.03f || player_found != null)
                    {
                        timerAnimationAttack = 0;
                        animationAttackDone = false;
                        shark_attack_done = true;
                    }
                }

            }

            // }
        }
        else
        {
            if (player_found != null)
            {
                if (!animationAttackDone)
                {
                    GoUnderground(true);
                    animationAttackDone = true;
                }
                else
                {

                    timerAnimationAttack += Time.deltaTime;

                    if (timerAnimationAttack >= 0.38f)
                    {
                        timerAnimationAttack = 0;
                        shark_collider.enabled = false;
                        //sprite_rend.sprite = shadow_sprite;
                        Transform parent = player_found.transform.parent;
                        Player_Manager player_manager = parent.GetComponent<Player_Manager>();
                        player_manager.GetDamage(transform);
                        player_found = null;
                        shark_attack_done = false;
                        animationAttackDone = false;
                    }
                }

            }
            else
            {
                if (!animationAttackDone)
                {
                    //(bool)myBB.GetParameter("is_enemy_hit") == true
                    get_damage_collider.enabled = true;
                    state = Kelpi_State.STUNNED;

                    stun_filler.enabled = true;
                    stun_filler.fillAmount = 1 - timer_stunned_count / time_stunned;

                    timer_stunned_count += Time.deltaTime;

                    if (timer_stunned_count > time_stunned || (bool)myBT.myBB.GetParameter("is_enemy_hit") == true)
                    {
                        sprite_rend.color = Color.red;
                        GoUnderground(true);
                        animationAttackDone = true;
                    }

                }
                else
                {
                    //make kelpi fail attack and then wait seconds
                    timerAnimationAttack += Time.deltaTime;

                    if (timerAnimationAttack >= 0.38f)
                    {
                        sprite_rend.color = Color.white;
                        animationAttackDone = false;
                        timerAnimationAttack = 0;
                        stun_filler.enabled = false;
                        myBT.myBB.SetParameter("is_enemy_hit", false);
                        //this.StartAction();
                        myBT.myBB.SetParameter("shootBall", true);
                        isFinish = true;
                    }
                    
                }

            }
        }

        return BT_Status.RUNNING;
    }


    void GoUnderground(bool GoUnder)
    {
        myAnimator.SetBool("KelpieAttack", !GoUnder);
        myAnimator.SetBool("KelpieGoUnderground", GoUnder);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 player_pos = player.transform.position;
            player_pos.y += sprite_rend_player.bounds.size.y * 0.5f;
            player_pos.y -= sprite_rend.bounds.size.y * 0.5f;
            Gizmos.DrawWireSphere(player_pos, distance_to_arrive_player);
        }
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
