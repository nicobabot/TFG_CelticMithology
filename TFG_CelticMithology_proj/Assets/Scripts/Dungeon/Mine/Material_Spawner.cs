using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Material_Spawner : MonoBehaviour {

    public Material_InGame material_to_spawn;

    [Range(2,5)]
    public int min_rand_materials = 2;
    [Range(5, 15)]
    public int max_rand_materials = 5;

    CircleCollider2D col_circle;
    bool is_spawn_done = false;
    bool spawn_material = false;

    // Use this for initialization
    void Start () {
        col_circle = GetComponent<CircleCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {

        Vector2 point_to_spawn = Random.insideUnitSphere * col_circle.radius;

        if (spawn_material && is_spawn_done == false)
        {
            switch (material_to_spawn)
            {
                case Material_InGame.WOOD_MATERIAL:
                    break;

                case Material_InGame.IRON_MATERIAL:
                    break;

                case Material_InGame.SILVER_MATERIAL:
                    break;

                case Material_InGame.DIAMOND_MATERIAL:
                    break;
            }
            is_spawn_done = true;
        }
	}
}
