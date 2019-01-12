using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_Attack : MonoBehaviour {

    public GameObject father_collider_slash_attack;
    public float time_slashing = 1.0f;
    public CameraManager cam_manager;

    Player_Manager player_manager_sct;
    float timer_slash = 0.0f;
    GameObject enemy_collided = null;

    // Use this for initialization
    void Start () {
        player_manager_sct = GetComponent<Player_Manager>();
    }
	
	// Update is called once per frame
	public void Attack_Slash_Update () {

        GameObject collider_to_activate = father_collider_slash_attack.transform.GetChild((int)player_manager_sct.player_direction).gameObject;

        if (!collider_to_activate.active)
        {
            collider_to_activate.SetActive(true);
        }

        Detect_Collision_Slash collision_slash_scr = collider_to_activate.GetComponent<Detect_Collision_Slash>();

        if(collision_slash_scr!= null)
        {
            enemy_collided = collision_slash_scr.Is_Enemy_Collided();
            if (enemy_collided != null)
            {
                Debug.Log("Enemy detected: ", enemy_collided);
                cam_manager.Cam_Shake();

                enemy_collided = null;
                //Call enemy damage function
            }
        }

        timer_slash += Time.deltaTime;

        if(timer_slash> time_slashing)
        {
            timer_slash = 0;
            collider_to_activate.SetActive(false);
            player_manager_sct.current_state = Player_Manager.Player_States.IDLE_PLAYER;
        }




    }
}
