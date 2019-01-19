using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_To_Mine : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("change_to_mine"))
        {
            Transform parent = transform.parent;
            if (parent != null)
            {
                Player_Manager manager = parent.GetComponent<Player_Manager>();
                manager.Set_In_Mine(!manager.Get_In_Mine());
            }

        }
    }

}
