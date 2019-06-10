﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalObject : MonoBehaviour {

    public string scene = "";
    public Player_Manager Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player_movement_collider"))
        {
            Player.WriteInJSON();
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
