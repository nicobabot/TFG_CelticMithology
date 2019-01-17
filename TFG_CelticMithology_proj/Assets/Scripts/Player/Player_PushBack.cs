using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Player_Manager))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player_PushBack : MonoBehaviour {

    public float pushback_force;
    public float time_doing_pushback=0.1f;
    public Transform enemy_pos;
    float timer_pushback=0.0f;
    Player_Manager player_manager_sct;
    Rigidbody2D rb;


    // Use this for initialization
    void Start () {
        player_manager_sct = GetComponent<Player_Manager>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	public void PushBack_Update()
    {

        Vector3 dir_push = transform.position - enemy_pos.position;
        dir_push = dir_push.normalized * pushback_force;

        timer_pushback += Time.deltaTime;

        rb.velocity = dir_push;

        if (timer_pushback > time_doing_pushback)
        {
            rb.velocity = Vector2.zero;
            timer_pushback = 0;
            player_manager_sct.current_state = Player_Manager.Player_States.IDLE_PLAYER;
        }

    }
}
