using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToBoss : MonoBehaviour {

    public int numDungeon;
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = ProceduralDungeonGenerator.mapGenerator.Player;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameObject go = GameObject.Find("Kelpie_Obj(Clone)");
            if (go == null) return;
            player.transform.position = go.transform.position;
        }


            if (Input.GetKeyDown(KeyCode.F10))
        {
            if (numDungeon == 0)
            {
                GameObject go = GameObject.Find("MacLir_Obj(Clone)");
                if (go == null) return;
                player.transform.position = go.transform.position;
            }
            else if (numDungeon == 1)
            {
                GameObject go = GameObject.Find("MyMorrigan(Clone)");
                if (go == null) return;
                player.transform.position = go.transform.position;
            }
        }
	}
}
