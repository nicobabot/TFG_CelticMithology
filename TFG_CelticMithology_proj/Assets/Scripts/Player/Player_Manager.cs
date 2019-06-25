using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
[RequireComponent(typeof(Collision_Movement))]
[RequireComponent(typeof(Slash_Attack))]
[RequireComponent(typeof(Player_PushBack))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

[System.Serializable]
class Stats
{
    public int lives = 0;
    public int damage = 0;
}

public class Player_Manager : MonoBehaviour
{
    public float time_dashing = 1.0f;
    public float movement_speed = 5.0f;
    public float dash_speed = 10.0f;
    public GameObject Menu;
    public GameObject ImproveMenu;
    public Fader fader_scr;
    public Live_Manager live_manager_scr;
    public TextMeshProUGUI strengthText;

    [Header("Invulnerable stuff")]
    public float total_time_invulnerable = 1.5f;
    public float speed_invulerable = 1.5f;
    public Collider2D combat_collider;
    [HideInInspector] public bool is_invulnerable = false;
    [HideInInspector] public bool noNeedInvulnerable = false;
    private float timer_invulerable = 0.0f;
    private ImproveManager improveScr;
    public enum Player_States
    {
        IDLE_PLAYER,
        MOVING_PLAYER,
        DASHING_PLAYER,
        SLASHING_PLAYER,
        FALLING_PLAYER,
        PUSHBACK_PLAYER,
        IN_MENU_PLAYER,
        MID_STUNNED_PLAYER
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

    private Stats myStats;

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
    private float numOfInput = 0;
    private int lastInput = -1;

    public GameObject inmortalText;
    [HideInInspector] public bool _inmortalMode = false;




    private void Awake()
    {
        /*string path_to_save_file = "/JSON/playerStats.json";
        string filePath = Application.dataPath + path_to_save_file;
         Stats stat = new Stats();


         string dataAsJson = File.ReadAllText(filePath);

         stat = JsonUtility.FromJson<Stats>(dataAsJson);

         myStats = stat;*/
        //player_stats.
        //player_stats.Right_Hand_Object = new Object_InGame();

        player_stats = GetComponent<Player_Stats>();
        improveScr = GetComponent<ImproveManager>();
        player_stats.Right_Hand_Object = new Object_InGame();
        player_stats.Right_Hand_Object.damage = PlayerPrefs.GetInt("playerDamage"); 
        live_manager_scr.numHearts = PlayerPrefs.GetInt("playerLive");
        improveScr.improveBar.fillAmount = PlayerPrefs.GetFloat("playerImprove");

        //Debug.Log(dataAsJson);
    }

    // Use this for initialization
    private void Start()
    {
        movement_script = GetComponent<Collision_Movement>();
        slash_attack_script = GetComponent<Slash_Attack>();
        pushback_script = GetComponent<Player_PushBack>();

        anim = GetComponent<Animator>();
        sprite_rend = GetComponent<SpriteRenderer>();

        timer_dash = 0.0f;

        current_state = Player_States.IDLE_PLAYER;
        strengthText.text = "Strenght lvl " + player_stats.Right_Hand_Object.damage;
    }

    public void PlayerUpdateStrenght()
    {
        strengthText.text = "Strenght lvl " + player_stats.Right_Hand_Object.damage;
    }

    // Update is called once per frame
    private void Update()
    {   

        if (Input.GetKeyDown(KeyCode.I))
        {
            _inmortalMode = !_inmortalMode;
            inmortalText.SetActive(_inmortalMode);
        }

        if (_inmortalMode)
        {
            combat_collider.enabled = false;
        }


        if (is_invulnerable && !noNeedInvulnerable)
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


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Abutton")) && current_state != Player_States.MID_STUNNED_PLAYER
            && current_state != Player_States.PUSHBACK_PLAYER && current_state != Player_States.SLASHING_PLAYER
            && current_state != Player_States.IDLE_PLAYER && Time.timeScale == 1)
        {
            slash_attack_script.Update_Attack_Colliders_To_None_Active();
            current_state = Player_States.DASHING_PLAYER;
        }
        else if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Xbutton"))
            && current_state != Player_States.PUSHBACK_PLAYER
            && current_state != Player_States.DASHING_PLAYER
            && Time.timeScale == 1 && current_state != Player_States.MID_STUNNED_PLAYER)
        {
            if (!in_mine)
            {
                //int dir = anim.GetInteger("player_direction");

                //anim.Rebind();

               // anim.SetInteger("player_direction", dir);

                anim.SetBool("player_idle", false);
                //slash_attack_script.timer_slash = 0;
                //slash_attack_script.is_slash_done = false;
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
                Menu.SetActive(true);
            }
            else if(Time.timeScale == 0 && !ImproveMenu.activeSelf)
            {
                current_state = Player_States.IDLE_PLAYER;
                Time.timeScale = 1;
                Menu.SetActive(false);
            }

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
        else if (current_state == Player_States.MID_STUNNED_PLAYER)
        {
            movement_script.StopMoving();
            movement_script.SpriteDir();

            string[] temp = Input.GetJoystickNames();

            if (temp != null && temp.Length > 0 && temp[0] != "")
            {
                AnyInputController();

                if (numOfInput >= 50)
                {
                    numOfInput = 0;
                    current_state = Player_States.IDLE_PLAYER;
                }
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    numOfInput++;
                    if (numOfInput >= 5)
                    {
                        numOfInput = 0;
                        current_state = Player_States.IDLE_PLAYER;
                    }
                }
            }
        

        }
        else if (current_state == Player_States.FALLING_PLAYER)
        {
            //todo

            /*
             * 
              if (noNeedInvulnerable)
            {
                is_invulnerable = false;
                noNeedInvulnerable = false;
            }
            else
             * */

            is_invulnerable = true;

            //Start Animation
            anim.SetBool("player_fall", true);
            if (timer_fall >= anim.GetCurrentAnimatorClipInfo(0)[0].clip.length)
            {
                timer_fall = 0.0f;
                Vector3 temp_vect = new Vector3(-25.0f, 2.0f, 0.0f);
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
            lastInput = -1;
            movement_script.Movement_Update(movement_speed);
        }
    }

    void AnyInputController()
    {
        float input_movement_horizontal = Input.GetAxisRaw("Horizontal");
        float input_movement_vertical = Input.GetAxisRaw("Vertical");

        if (input_movement_horizontal > 0.5f && lastInput != 0)
        {
            numOfInput++;
            lastInput = 0;
        }
        else if (input_movement_horizontal < -0.5f && lastInput != 1)
        {
            numOfInput++;
            lastInput = 1;
        }

        if (input_movement_vertical > 0.5f && lastInput != 2)
        {
            numOfInput++;
            lastInput = 2;
        }
        else if (input_movement_vertical < -0.5f && lastInput != 3)
        {
            numOfInput++;
            lastInput = 3;
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

    public void GetDamage(Transform enemy_push, bool realDamage = true)
    {
        Set_Enemy_Pushback(enemy_push);
        noNeedInvulnerable = !realDamage;
        current_state = Player_States.PUSHBACK_PLAYER;
        fader_scr.Fade_image.enabled = true;
        fader_scr.FadeOut(false, true);
        if(realDamage)
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

    public void WriteInJSON()
    {
        /*string path_to_save_file = "/JSON/playerStats.json";
        string filePath = Application.dataPath + path_to_save_file;

        Stats newStat =new Stats();
        newStat.damage = player_stats.Right_Hand_Object.damage;
        newStat.lives = live_manager_scr.maxLives;

        string myStatsJson = JsonUtility.ToJson(newStat);

        File.WriteAllText(filePath, myStatsJson);
        Debug.Log(myStatsJson);*/

        PlayerPrefs.SetInt("playerDamage", player_stats.Right_Hand_Object.damage);
        PlayerPrefs.SetInt("playerLive", live_manager_scr.maxLives);
        PlayerPrefs.SetFloat("playerImprove", improveScr.improveBar.fillAmount);
    }

    public void DisablePause()
    {
        current_state = Player_States.IDLE_PLAYER;
        Time.timeScale = 1;
    }

    public void SetPlayerInPause()
    {
        if (Time.timeScale == 1)
        {
            current_state = Player_States.IN_MENU_PLAYER;
            Time.timeScale = 0;
        }
    }

}