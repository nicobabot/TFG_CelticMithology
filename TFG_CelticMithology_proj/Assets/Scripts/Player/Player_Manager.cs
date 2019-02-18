using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collision_Movement))]
[RequireComponent(typeof(Slash_Attack))]
[RequireComponent(typeof(Player_PushBack))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player_Manager : MonoBehaviour
{
    public float time_dashing = 1.0f;
    public float movement_speed = 5.0f;
    public float dash_speed = 10.0f;
    public GameObject Menu;
    public Fader fader_scr;
    public Live_Manager live_manager_scr;

    [Header("Invulnerable stuff")]
    public float total_time_invulnerable = 1.5f;
    public float speed_invulerable = 1.5f;
    public Collider2D combat_collider;
    [HideInInspector] public bool is_invulnerable = false;
    private float timer_invulerable = 0.0f;

    public enum Player_States
    {
        IDLE_PLAYER,
        MOVING_PLAYER,
        DASHING_PLAYER,
        SLASHING_PLAYER,
        FALLING_PLAYER,
        PUSHBACK_PLAYER,
        IN_MENU_PLAYER
    }
    [Space()]
    public Player_States current_state;

    public enum Player_Direction
    {
        UP_PLAYER = 0,
        DOWN_PLAYER,
        RIGHT_PLAYER,
        LEFT_PLAYER
    }
    public Player_Direction player_direction;

    private Player_Stats player_stats;
    private Collision_Movement movement_script;
    private Slash_Attack slash_attack_script;
    private Player_PushBack pushback_script;

    private Animator anim;
    private SpriteRenderer sprite_rend;

    private float timer_dash = 0.0f;
    private bool want_to_dash = false;
    private bool in_mine = false;

    private float timer_fall = 0.0f;

    // Use this for initialization
    private void Start()
    {
        movement_script = GetComponent<Collision_Movement>();
        slash_attack_script = GetComponent<Slash_Attack>();
        pushback_script = GetComponent<Player_PushBack>();

        anim = GetComponent<Animator>();
        sprite_rend = GetComponent<SpriteRenderer>();

        player_stats = GetComponent<Player_Stats>();

        timer_dash = 0.0f;

        current_state = Player_States.IDLE_PLAYER;

    }

    // Update is called once per frame
    private void Update()
    {

        if (is_invulnerable)
        {
            combat_collider.enabled = false;
            timer_invulerable += Time.deltaTime;

            //Maybe Here alpha effect
            Color temp = sprite_rend.color;
           sprite_rend.color = new Color(temp.r, temp.g, temp.b, Mathf.Sin(Time.time * speed_invulerable));

            if (timer_invulerable >= total_time_invulnerable)
            {
                sprite_rend.color = new Color(temp.r, temp.g, temp.b, 255);
                timer_invulerable = 0;
                combat_collider.enabled = true;
                is_invulnerable = false;
            }
        }


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Abutton")) 
            && current_state != Player_States.PUSHBACK_PLAYER && current_state != Player_States.SLASHING_PLAYER
            && current_state != Player_States.IDLE_PLAYER)
        {
            slash_attack_script.Update_Attack_Colliders_To_None_Active();
            current_state = Player_States.DASHING_PLAYER;
        }
        else if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Xbutton"))
            && current_state != Player_States.PUSHBACK_PLAYER
            && current_state != Player_States.DASHING_PLAYER)
        {
            if (!in_mine)
            {
                anim.SetBool("player_idle", false);
                current_state = Player_States.SLASHING_PLAYER;
            }
            else
            {
                //Functionality in mine
            }
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("MenuButton")) && current_state != Player_States.PUSHBACK_PLAYER)
        {

            if (Time.timeScale == 1)
            {
                current_state = Player_States.IN_MENU_PLAYER;
                Time.timeScale = 0;
            }
            else
            {
                current_state = Player_States.IDLE_PLAYER;
                Time.timeScale = 1;
            }

            Menu.SetActive(!Menu.active);

        }


        if (current_state == Player_States.DASHING_PLAYER && timer_dash <= time_dashing)
        {
            anim.SetBool("player_roll", true);
            timer_dash += Time.deltaTime;

            time_dashing = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;

            movement_script.Movement_Update(dash_speed, true);
        }
        else if (current_state == Player_States.SLASHING_PLAYER)
        {
            slash_attack_script.Attack_Slash_Update();
        }
        else if (current_state == Player_States.FALLING_PLAYER)
        {
            //todo
            is_invulnerable = true;

            //Start Animation
            anim.SetBool("player_fall", true);
            if (timer_fall >= anim.GetCurrentAnimatorClipInfo(0)[0].clip.length)
            {
                timer_fall = 0.0f;
                Vector3 temp_vect = Vector3.zero;
                transform.position = temp_vect;
                anim.SetBool("player_fall", false);
                current_state = Player_States.IDLE_PLAYER;
            }
            else timer_fall += Time.deltaTime;

            
        }
        else if (current_state == Player_States.PUSHBACK_PLAYER)
        {
            is_invulnerable = true;
            anim.SetBool("player_attack", false);
            slash_attack_script.Update_Attack_Colliders_To_None_Active();
            pushback_script.PushBack_Update();
        }
        else if (current_state == Player_States.IN_MENU_PLAYER)
        {
        }
        else
        {
            anim.SetBool("player_roll", false);
            timer_dash = 0.0f;
            movement_script.Movement_Update(movement_speed);
        }
    }

    public void Set_Enemy_Pushback(Transform enemy_trans)
    {
        pushback_script.enemy_pos = enemy_trans;
    }

    public bool Get_In_Mine()
    {
        return in_mine;
    }

    public void GetDamage(Transform enemy_push)
    {
        Set_Enemy_Pushback(enemy_push);
        current_state = Player_States.PUSHBACK_PLAYER;
        fader_scr.Fade_image.enabled = true;
        fader_scr.FadeOut(false, true);
        live_manager_scr.DetectedDamage();
    }

    public void Set_In_Mine(bool new_state)
    {
        in_mine = new_state;
    }

    public Player_Stats Get_Player_Stats()
    {
        return player_stats;
    }

}