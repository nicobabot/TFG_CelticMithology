using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Detect_Collision_Slash : MonoBehaviour {

    public LayerMask enemy_layer;
    GameObject enemy_collided = null;
    BoxCollider2D collider;

    // Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Collider2D[] Is_Enemy_Collided()
    {
        collider = GetComponent<BoxCollider2D>();
        Collider2D[] enemies_found = Physics2D.OverlapBoxAll(transform.position, collider.size, 0.0f, enemy_layer);
      
        return enemies_found;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, collider.size);
    }

        /*private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "enemy")
            {
                enemy_collided = collision.gameObject;
            }
        }*/

    }
