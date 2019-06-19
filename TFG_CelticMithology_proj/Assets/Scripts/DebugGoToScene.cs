using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugGoToScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene("DungeonGenerator", LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene("DungeonGenerator2", LoadSceneMode.Single);
        }
	}
}
