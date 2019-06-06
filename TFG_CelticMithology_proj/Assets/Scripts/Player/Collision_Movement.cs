using UnityEngine;
[RequireComponent (typeof(SpriteRenderer))]
public class Collision_Movement : MonoBehaviour
{
    public Collider2D combat_player_collider;
    public Collider2D falling_player_collider;

    private Rigidbody2D rb;
    private Player_Manager play_manager_scr;
    private SpriteRenderer sprite_rend_scr;
    private Animator anim;

    public float timer_to_make_idle = 0.25f;
    float timer_to_idle = 0.0f;

    float input_movement_horizontal;
    float input_movement_vertical;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        play_manager_scr = GetComponent<Player_Manager>();
        sprite_rend_scr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public void SpriteDir()
    {
        input_movement_horizontal = Input.GetAxisRaw("Horizontal");
        input_movement_vertical = Input.GetAxisRaw("Vertical");


        bool is_idle_hor = false;
        bool is_idle_vert = false;

        //Just to keep tracking of the direction of the player
        if (input_movement_horizontal > 0.5f)
        {
            anim.SetInteger("player_direction", 0);
            sprite_rend_scr.flipX = false;
            play_manager_scr.player_direction = Player_Manager.Player_Direction.RIGHT_PLAYER;
        }
        else if (input_movement_horizontal < -0.5f)
        {
            anim.SetInteger("player_direction", 0);
            sprite_rend_scr.flipX = true;
            play_manager_scr.player_direction = Player_Manager.Player_Direction.LEFT_PLAYER;
        }
        //else is_idle_hor = true;

        if (input_movement_vertical > 0.5f)
        {
            anim.SetInteger("player_direction", 2);
            play_manager_scr.player_direction = Player_Manager.Player_Direction.UP_PLAYER;
        }
        else if (input_movement_vertical < -0.5f)
        {
            anim.SetInteger("player_direction", 1);
            play_manager_scr.player_direction = Player_Manager.Player_Direction.DOWN_PLAYER;
        }
        /*else is_idle_vert = true;

        if (is_idle_hor && is_idle_vert)
        {
            timer_to_idle += Time.deltaTime;

            if (timer_to_idle > timer_to_make_idle)
            {
                anim.SetBool("player_idle", true);
            }
        }
        else
        {
            timer_to_idle = 0.0f;
            anim.SetBool("player_idle", false);
        }*/
    }

    // Update is called once per frame
    public void Movement_Update(float new_speed, bool is_dashing = false)
    {
        if (!is_dashing)
        {
            input_movement_horizontal = Input.GetAxisRaw("Horizontal");
            input_movement_vertical = Input.GetAxisRaw("Vertical");
        }

        //If the player is dashing we want to deactivate the combat collider to
        //be inmortal
        if (is_dashing == true)
        {
            if (combat_player_collider.enabled)
            {
                combat_player_collider.enabled = false;
                falling_player_collider.enabled = false;
            }
        }
        else
        {
            //If the player finished dashing we want to activate the combat collider
            if (!combat_player_collider.enabled && play_manager_scr.is_invulnerable == false && !play_manager_scr._inmortalMode)
            {
                combat_player_collider.enabled = true;
                falling_player_collider.enabled = true;
            }
        }

        bool is_idle_hor = false;
        bool is_idle_vert = false;

        //Just to keep tracking of the direction of the player
        if (input_movement_horizontal > 0.5f)
        {
            anim.SetInteger("player_direction", 0);
            sprite_rend_scr.flipX = false;
            play_manager_scr.player_direction = Player_Manager.Player_Direction.RIGHT_PLAYER;
        }
        else if (input_movement_horizontal < -0.5f)
        {
            anim.SetInteger("player_direction", 0);
            sprite_rend_scr.flipX = true;
            play_manager_scr.player_direction = Player_Manager.Player_Direction.LEFT_PLAYER;
        }
        else is_idle_hor = true;

        if (input_movement_vertical > 0.5f)
        {
            anim.SetInteger("player_direction", 2);
            play_manager_scr.player_direction = Player_Manager.Player_Direction.UP_PLAYER;
        }
        else if (input_movement_vertical < -0.5f)
        {
            anim.SetInteger("player_direction", 1);
            play_manager_scr.player_direction = Player_Manager.Player_Direction.DOWN_PLAYER;
        }
        else is_idle_vert = true;

        if (is_idle_hor && is_idle_vert)
        {
            timer_to_idle += Time.deltaTime;

            if (timer_to_idle > timer_to_make_idle) {
                anim.SetBool("player_idle", true);
            }
        }
        else
        {
            timer_to_idle = 0.0f;
            anim.SetBool("player_idle", false);
        }

        //Movement calculation
        if (input_movement_horizontal > 0.5f || input_movement_horizontal < -0.5f)
        {
            rb.velocity = new Vector3(input_movement_horizontal * new_speed, rb.velocity.y);
            if (!is_dashing) play_manager_scr.current_state = Player_Manager.Player_States.MOVING_PLAYER;
        }
        else if (input_movement_horizontal < 0.5f || input_movement_horizontal > -0.5f)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y);
        }

        if (input_movement_vertical > 0.5f || input_movement_vertical < -0.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, input_movement_vertical * new_speed);
            if (!is_dashing) play_manager_scr.current_state = Player_Manager.Player_States.MOVING_PLAYER;
        }
        else if (input_movement_vertical < 0.5f || input_movement_vertical > -0.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f);
        }

        //Just to change the state of the player
        if ((input_movement_horizontal < 0.5f && input_movement_horizontal > -0.5f) &&
            (input_movement_vertical < 0.5f && input_movement_vertical > -0.5f))
        {
            if (!is_dashing) play_manager_scr.current_state = Player_Manager.Player_States.IDLE_PLAYER;
        }
    }
}