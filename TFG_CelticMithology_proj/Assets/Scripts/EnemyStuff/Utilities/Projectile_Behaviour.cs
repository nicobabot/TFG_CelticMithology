using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour {

    float min_distance=0.1f;
    float velocity=1.0f;
    Vector3 direction;
    Vector3 hit_wall_point;
    

	// Use this for initialization
	public void CreateProjectile (Vector3 new_dir, Vector3 new_hitwall, float new_min_distance, float new_velocity) {
        direction = new_dir;
        hit_wall_point = new_hitwall;
        min_distance = new_min_distance;
        velocity = new_velocity;
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 dir = hit_wall_point - transform.position;

        float step = Time.deltaTime * velocity;

        transform.position = Vector3.MoveTowards(transform.position, hit_wall_point, step);

        if(dir.magnitude < min_distance)
        {
            //Particles
            gameObject.SetActive(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_combat_collider"))
        {
            //Particles
            gameObject.SetActive(false);
        }
    }

}
