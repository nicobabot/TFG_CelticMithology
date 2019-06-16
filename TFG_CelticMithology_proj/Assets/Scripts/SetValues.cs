using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValues : MonoBehaviour {

    private void Awake()
    {
        PlayerPrefs.SetInt("playerDamage", 1);
        PlayerPrefs.SetInt("playerLive", 4);
        PlayerPrefs.SetFloat("playerImprove", 0.0f);
        PlayerPrefs.SetInt("playerTutorial", 0);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
