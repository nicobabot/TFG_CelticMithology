using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalObject : MonoBehaviour {

    public string scene = "";
    private Player_Manager Player;
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
            Player_Manager PlayerManager = ProceduralDungeonGenerator.mapGenerator.Player.GetComponent<Player_Manager>();
            PlayerManager.WriteInJSON();
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
