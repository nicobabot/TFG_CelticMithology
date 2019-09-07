using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_ShootPlayer : ActionBase {

    [Header("Shoot Settings")]
    public LayerMask layer_wall;
    public GameObject projectile;
    public float time_to_spawn_projectile = 0.75f;
    public bool only_stright_shots = false;

    [Header("Projectile Settings")]
    public float projectile_min_distance = 0.1f;
    public float projectile_velocity = 1.0f;

    private GameObject player;
    private SpriteRenderer sprite_rend_player;
    private float timer_spawn_proj = 0.0f;
    private Vector3 pushback_dir;
    private Vector3 hitpoint_wall;
    private Action_FollowPlayer follow_player_scr;

    public GameObject spawn_pos;

    private Animator anim;
    float animation_timer = 0.0f;

    private Animator myAnimator;

override public BT_Status StartAction()
    {
        timer_spawn_proj = time_to_spawn_projectile;

        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        // Remember the pivot point to be in the "mouth" or where the pojectile is shoot

        sprite_rend_player = player.GetComponent<SpriteRenderer>();
        if (sprite_rend_player == null)
        {
            Debug.Log("<color=red>Sprite renderer null _Action_PushBack");
        }

        follow_player_scr = GetComponent<Action_FollowPlayer>();
        if (follow_player_scr == null)
        {
            Debug.Log("<color=red>No follow player action _Action_PushBack");
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        timer_spawn_proj += Time.deltaTime;

        if (timer_spawn_proj > time_to_spawn_projectile)
        {
            //Start shoot animation
            float lenght_anim = 0;
            anim = GetComponent<Animator>();
            if (anim)
            {
                anim.SetBool("enemy_idle", false);

                AnimatorClipInfo[] anim_clip = anim.GetCurrentAnimatorClipInfo(0);
                lenght_anim = anim_clip[0].clip.length;
            }

            if (!only_stright_shots)
            {
                Calculate_Direction();
            }
            else
            {
                Calculate_Stright_Direction();
            }

            if (animation_timer >= (lenght_anim * 0.75f))
            {
                GameObject my_projectile = Instantiate(projectile);
                my_projectile.SetActive(true);
                my_projectile.transform.position = transform.position;

                if(myBT.enemy_type == Enemy_type.MORRIGAN_ENEMY)
                {
                   // my_projectile.transform.position = spawn_pos.transform.position;
                }

                Projectile_Behaviour projectile_scr = my_projectile.GetComponent<Projectile_Behaviour>();
                if (projectile_scr != null)
                {
                    projectile_scr.CreateProjectile(pushback_dir, hitpoint_wall, projectile_min_distance, projectile_velocity);
                }
                if (anim)
                    anim.SetBool("enemy_idle", true);

                timer_spawn_proj = 0.0f;
                animation_timer = 0.0f;
            }
            else animation_timer += Time.deltaTime;

        }

        return BT_Status.RUNNING;
    }

    void Calculate_Direction()
    {
        float size_addition_player = (sprite_rend_player.bounds.size.y * 0.5f);
        Vector3 temp_position_player = player.transform.position;
        temp_position_player.y += size_addition_player;
        pushback_dir = temp_position_player - transform.position;

        RaycastHit2D hit;

        /*if (myBT.enemy_type == Enemy_type.MORRIGAN_ENEMY)
            hit = Physics2D.Raycast(spawn_pos.transform.position, pushback_dir.normalized, Mathf.Infinity, layer_wall);
        else*/ hit = Physics2D.Raycast(transform.position, pushback_dir.normalized, Mathf.Infinity, layer_wall);

        if (hit != null)
        {
            hitpoint_wall = hit.point;
        }
    }

    void Calculate_Stright_Direction()
    {
        Direction new_direction = follow_player_scr.DetectDirection(transform.position, player.transform.position);

        switch (new_direction)
        {
            case Direction.RIGHT:
                pushback_dir = transform.right;
                //anim.SetFloat()
                break;
            case Direction.LEFT:
                pushback_dir = -transform.right;
                break;
            case Direction.UP:
                pushback_dir = transform.up;
                break;
            case Direction.DOWN:
                pushback_dir = -transform.up;
                break;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, pushback_dir, Mathf.Infinity, layer_wall);
        if (hit != null)
        {
            hitpoint_wall = hit.point;
        }

    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
