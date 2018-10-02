using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Movement : MonoBehaviour {

    Rigidbody2D rb;
    public float speed = 5.0f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        float input_movement_horizontal = Input.GetAxisRaw("Horizontal");
        float input_movement_vertical = Input.GetAxisRaw("Vertical");

        if (input_movement_horizontal > 0.5f || input_movement_horizontal < -0.5f)
        {
            rb.velocity = new Vector3(input_movement_horizontal * speed, rb.velocity.y);
        }
        else if (input_movement_horizontal < 0.5f || input_movement_horizontal > -0.5f)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y);
        }

        if (input_movement_vertical > 0.5f || input_movement_vertical < -0.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, input_movement_vertical * speed);
        }
        else if (input_movement_vertical < 0.5f || input_movement_vertical > -0.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f);
        }



    }
}
