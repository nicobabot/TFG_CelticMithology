using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Material_Spawner : MonoBehaviour {

    public Material_InGame material_to_spawn;

    [Range(1, 15)]
    public int max_rand_materials = 5;

    CircleCollider2D col_circle;
    bool is_spawn_done = false;



    // Use this for initialization
    void Start () {
        col_circle = GetComponent<CircleCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {

        if (is_spawn_done == false)
        {
            for (int i = 0; i < max_rand_materials; i++)
            {
                Vector2 point_to_spawn = transform.position + Random.insideUnitSphere * col_circle.radius;

                Transform trans = transform.GetChild((int)material_to_spawn);
                GameObject go = Instantiate(trans.gameObject);
                go.SetActive(true);
                go.transform.position = point_to_spawn;
            }
            is_spawn_done = true;
        }
	}
}
