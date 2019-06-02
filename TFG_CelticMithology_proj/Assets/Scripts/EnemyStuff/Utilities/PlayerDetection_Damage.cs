using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection_Damage : MonoBehaviour {

    public bool need_enemy_type_detection = true;
   [HideInInspector] public bool playerHitted = false;
    public Fader fader_scr;
    Live_Manager live_manager_scr;
    Player_Manager player_manager_scr;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_combat_collider"))
        {
            if (!Detect_Enemy_Type_And_Action())
            {
                Transform parent = collision.transform.parent;

                if (parent != null)
                {
                    live_manager_scr = parent.GetComponent<Live_Manager>();
                    player_manager_scr = parent.GetComponent<Player_Manager>();

                    if (live_manager_scr != null && player_manager_scr != null)
                    {
                        player_manager_scr.GetDamage(transform);
                        playerHitted = true;
                    }
                }
            }
        }
    }

    bool Detect_Enemy_Type_And_Action()
    {
        bool is_pushing = false;
        if (need_enemy_type_detection)
        {
            bool type_found = false;

            //Need update with all types
            Transform parent = transform.parent;
            if (parent = null) {
                BT_Entity enemy_temp = parent.GetComponent<BT_Soldier>();
                if (enemy_temp != null && type_found == false)
                {
                    type_found = true;
                    is_pushing = (bool)enemy_temp.myBB.GetParameter("is_enemy_hit");
                }

                enemy_temp = parent.GetComponent<BT_Caorthannach>();
                if (enemy_temp != null && type_found == false)
                {
                    type_found = true;
                    is_pushing = (bool)enemy_temp.myBB.GetParameter("is_enemy_hit");
                }

                enemy_temp = parent.GetComponent<BT_Kelpi>();
                if (enemy_temp != null && type_found == false)
                {
                    type_found = true;
                    is_pushing = (bool)enemy_temp.myBB.GetParameter("is_enemy_hit");
                }

            }
        }
        return is_pushing;

    }

}
