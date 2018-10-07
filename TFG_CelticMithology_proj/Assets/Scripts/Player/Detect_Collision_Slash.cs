using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect_Collision_Slash : MonoBehaviour {

    GameObject enemy_collided = null;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject Is_Enemy_Collided()
    {
        GameObject temp_go = enemy_collided;
        if (temp_go != null)
        {
            enemy_collided = null;
        }
        return temp_go;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "enemy")
        {
            enemy_collided = collision.gameObject;
        }

    }

}
