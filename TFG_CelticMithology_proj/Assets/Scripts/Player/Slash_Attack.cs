using UnityEngine;

public class Slash_Attack : MonoBehaviour
{
    public int damage;
    public GameObject father_collider_slash_attack;
    public float time_slashing = 1.0f;
    public CameraManager cam_manager;

    private Player_Manager player_manager_sct;
    private float timer_slash = 0.0f;
    private Collider2D[] enemies_found = null;

    private bool is_slash_done = false;

    GameObject collider_to_activate;

    // Use this for initialization
    private void Start()
    {
        player_manager_sct = GetComponent<Player_Manager>();
    }

    // Update is called once per frame
    public void Attack_Slash_Update()
    {
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
            }
            is_slash_done = true;
        }

        timer_slash += Time.deltaTime;

        if (timer_slash > time_slashing)
        {
            is_slash_done = false;
            timer_slash = 0;
            collider_to_activate.SetActive(false);
            collider_to_activate = null;
            player_manager_sct.current_state = Player_Manager.Player_States.IDLE_PLAYER;
        }
    }
    private void OnDrawGizmos()
    {
        if (collider_to_activate != null)
        {
            Gizmos.DrawCube(collider_to_activate.transform.position, collider_to_activate.GetComponent<BoxCollider2D>().size);
        }
    }

    private void React_To_Slash()
    {
        Debug.Log("Enemies detected: " + enemies_found.Length);
        cam_manager.Cam_Shake();

        foreach (Collider2D col in enemies_found)
        {
            //Detect enemy type
            BT_Soldier soldier = col.GetComponent<BT_Soldier>();
            if (soldier != null)
            {
                soldier.Enemy_Live_Modification(damage);
                Soldier_Blackboard bb_soldier = col.GetComponent<Soldier_Blackboard>();
                bb_soldier.is_enemy_hit.SetValue(true);
               
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