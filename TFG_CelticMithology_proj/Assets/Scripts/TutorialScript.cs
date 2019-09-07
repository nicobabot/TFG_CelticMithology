using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour {

    public int numTutorial = 0;

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("playerTutorial", 1);	
	}
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Abutton")) && numTutorial == 0)
        {
            SceneManager.LoadScene("DungeonGenerator", LoadSceneMode.Single);
            //SceneManager.LoadScene("TutorialScene2", LoadSceneMode.Single);
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Abutton")) && numTutorial == 1)
        {
            SceneManager.LoadScene("DungeonGenerator", LoadSceneMode.Single);
        }
	}
}
