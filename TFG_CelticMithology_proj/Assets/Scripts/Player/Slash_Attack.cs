using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Slash_Attack : MonoBehaviour
{
    public GameObject father_collider_slash_attack;
    public float time_slashing = 1.0f;
    public CameraManager cam_manager;
    public Player_Stats player_stats;

    private Player_Manager player_manager_sct;

    private float timer_slash = 0.0f;
    private Collider2D[] enemies_found = null;
    private bool is_slash_done = false;

    Animator anim;
    GameObject collider_to_activate;
    Rigidbody2D rb;

    private void Start()
    {
        player_manager_sct = GetComponent<Player_Manager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Attack_Slash_Update()
    {
        
        anim.SetBool("player_attack", true);

        rb.velocity = Vector2.zero;

        AnimatorClipInfo[] anim_clip = anim.GetCurrentAnimatorClipInfo(0);
        float lenght_anim = anim_clip[0].clip.length;
        //Debug.Log("Animation length: " + lenght_anim);

        timer_slash += Time.deltaTime;

        collider_to_activate = father_collider_slash_attack.transform.GetChild((int)player_manager_sct.player_direction).gameObject;

        if (!collider_to_activate.active)
        {
            collider_to_activate.SetActive(true);
        }

        Detect_Collision_Slash collision_slash_scr = collider_to_activate.GetComponent<Detect_Collision_Slash>();

        if (collision_slash_scr != null && is_slash_done == false)
        {
            enemies_found = collision_slash_scr.Is_Enemy_Collided();
            if (enemies_found.Length > 0)
            {
                React_To_Slash();
                is_slash_done = true;
            }

        }


        if (timer_slash > lenght_anim)
        {
            anim.SetBool("player_attack", false);
            is_slash_done = false;
            timer_slash = 0;
            collider_to_activate.SetActive(false);
            collider_to_activate = null;
            player_manager_sct.current_state = Player_Manager.Player_States.IDLE_PLAYER;
        }
    }
    private void OnDrawGizmos()
    {
        /*if (collider_to_activate != null)
        {
            Gizmos.DrawCube(collider_to_activate.transform.position, collider_to_activate.GetComponent<BoxCollider2D>().size);
        }*/
    }

    private void React_To_Slash()
    {
        //Debug.Log("Enemies detected: " + enemies_found.Length);
        cam_manager.Cam_Shake();

        foreach (Collider2D col in enemies_found)
        {
            Transform parent = col.transform.parent;
            if (parent != null)
            {

                //Detect enemy type
                BT_Soldier soldier = parent.GetComponent<BT_Soldier>();
                if (soldier != null)
                {
                    if (soldier.currentAction != null)
                    {
                        soldier.currentAction.isFinish = true;
                    }
                    soldier.Enemy_Live_Modification(-player_stats.Right_Hand_Object.damage);
                    Soldier_Blackboard bb_soldier = parent.GetComponent<Soldier_Blackboard>();
                    bb_soldier.is_enemy_hit.SetValue(true);
                }
                BT_Caorthannach Caorth = parent.GetComponent<BT_Caorthannach>();
                if (Caorth != null)
                {
                    if (Caorth.currentAction != null)
                    {
                        Caorth.currentAction.isFinish = true;
                    }
                    Caorth.Enemy_Live_Modification(-player_stats.Right_Hand_Object.damage);
                    Caorthannach_Blackboard bb_caorth = parent.GetComponent<Caorthannach_Blackboard>();
                    bb_caorth.is_enemy_hit.SetValue(true);
                }
                BT_Kelpi Kelpi = parent.GetComponent<BT_Kelpi>();
                if (Kelpi != null)
                {
                    /*if (Kelpi.currentAction != null)
                    {
                        Kelpi.currentAction.isFinish = true;
                    }*/
                    Kelpi.Enemy_Live_Modification(-player_stats.Right_Hand_Object.damage);
                    Kelpi_Blackboard bb_kelpi = parent.GetComponent<Kelpi_Blackboard>();
                    bb_kelpi.is_enemy_hit.SetValue(true);
                }
                BT_MacLir Maclir = parent.GetComponent<BT_MacLir>();
                if (Maclir != null)
                {
                    if (Maclir.currentAction != null)
                    {
                        Maclir.currentAction.isFinish = true;
                    }
                    Maclir.Enemy_Live_Modification(-player_stats.Right_Hand_Object.damage);
                    MacLir_Blackboard bb_maclir = parent.GetComponent<MacLir_Blackboard>();
                    bb_maclir.is_enemy_hit.SetValue(true);
                }
                BT_DearDug DearDug = parent.GetComponent<BT_DearDug>();
                if (DearDug != null)
                {
                    if (DearDug.currentAction != null)
                    {
                        DearDug.currentAction.isFinish = true;
                    }
                    DearDug.Enemy_Live_Modification(-player_stats.Right_Hand_Object.damage);
                    DearDug_Blackboard dearDugBB = parent.GetComponent<DearDug_Blackboard>();
                    dearDugBB.is_enemy_hit.SetValue(true);
                }
                BT_Banshee Banshee = parent.GetComponent<BT_Banshee>();
                if (Banshee != null)
                {
                    if (Banshee.currentAction != null)
                    {
                        Banshee.currentAction.isFinish = true;
                    }
           
                    Banshee_Blackboard bansheeBB = parent.GetComponent<Banshee_Blackboard>();
                    if (Banshee.myState == BT_Banshee.BansheeState.STUNNED_BANSHEE)
                    {
                        Banshee.Enemy_Live_Modification(-player_stats.Right_Hand_Object.damage);
                        bansheeBB.is_enemy_hit.SetValue(true);
                    }
                    else bansheeBB.want_to_hit.SetValue(true);
                }
                BT_Dagda Dagda = parent.GetComponent<BT_Dagda>();
                if (Dagda != null)
                {
                    if (Dagda.currentAction != null)
                    {
                        Dagda.currentAction.isFinish = true;
                    }

                    Dagda_Blackboard dagdaBB = parent.GetComponent<Dagda_Blackboard>();
                    Dagda.Enemy_Live_Modification(-player_stats.Right_Hand_Object.damage);
                    dagdaBB.is_enemy_hit.SetValue(true);
                }
                BT_Dullahan Dullahan = parent.GetComponent<BT_Dullahan>();
                if (Dullahan != null)
                {
                    if (Dullahan.currentAction != null)
                    {
                        Dullahan.currentAction.isFinish = true;
                    }

                    Dullahan_Blackboard dullahanBB = parent.GetComponent<Dullahan_Blackboard>();
                    Dullahan.Enemy_Live_Modification(-player_stats.Right_Hand_Object.damage);
                    dullahanBB.is_enemy_hit.SetValue(true);
                }

            }
            else
            {
                Debug.Log("Parent null _Slash_Attack");
            }
        }

        System.Array.Clear(enemies_found, 0, enemies_found.Length);
        //Call enemy damage function
    }

    public void Update_Attack_Colliders_To_None_Active()
    {

        collider_to_activate = null;

        for (int i=0; i< father_collider_slash_attack.transform.childCount; i++)
        {
            GameObject collider_to_deactivate = father_collider_slash_attack.transform.GetChild(i).gameObject;
            collider_to_deactivate.SetActive(false);
        }

    }

}