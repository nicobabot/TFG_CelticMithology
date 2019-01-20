using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_Type : MonoBehaviour {

    public Material_InGame type;
    public bool is_used = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (is_used)
        {
            //Destroy material
            gameObject.SetActive(false);
        }

	}
}
