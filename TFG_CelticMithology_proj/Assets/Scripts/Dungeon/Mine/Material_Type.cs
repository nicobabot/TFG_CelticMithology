using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_Type : MonoBehaviour {

    public Material_InGame type;
    public bool is_used = false;

    private Player_Stats player_stats;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (is_used)
        {
            //Destroy material
            gameObject.SetActive(false);
        }

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("player_change_state_collider"))
        {
            player_stats = collision.transform.parent.GetComponent<Player_Stats>();

            if(player_stats != null)
            {
                player_stats.Add_Material(1, type);
                is_used = true;
            }

        }

    }

}
