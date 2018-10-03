using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour {

    Collision_Movement movement_script;
    float timer_dash = 0.0f;
    public float timedashing = 1.0f;
    public float speed = 5.0f;
    public float dashspeed = 10.0f;

    bool want_to_dash = false;

    // Use this for initialization
    void Start () {

        movement_script = GetComponent<Collision_Movement>();

        if (movement_script == null)
        {
            Debug.Log("Movement script not found _ Player_Manager");
        }

        timer_dash = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            want_to_dash = true;
        }

        if (want_to_dash && timer_dash <= timedashing)
        {
            timer_dash += Time.deltaTime;
            movement_script.Movement_Update(dashspeed);
        }
        else {
            want_to_dash = false;
            timer_dash = 0.0f;
            movement_script.Movement_Update(speed);

        }

    }
}
