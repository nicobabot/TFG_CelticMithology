using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_To_Mine : MonoBehaviour {

    public bool change_to_mine = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_change_state_collider"))
        {
            Transform parent = collision.transform.parent;
            if (parent != null)
            {
                Player_Manager manager = parent.GetComponent<Player_Manager>();

                manager.Set_In_Mine(change_to_mine);

            }

        }
    }

}
