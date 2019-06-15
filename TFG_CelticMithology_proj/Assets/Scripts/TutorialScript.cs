using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("playerTutorial", 1);	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Abutton"))
        {
            SceneManager.LoadScene("DungeonGenerator", LoadSceneMode.Single);
        }
	}
}
