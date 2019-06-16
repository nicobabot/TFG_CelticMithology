using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectGamepad : MonoBehaviour {

    public EventSystem eventSystem;
    GameObject go;
	// Use this for initialization
	void Start () {
        go = eventSystem.firstSelectedGameObject;
    }
	
	// Update is called once per frame
	void Update () {

        string[] temp = Input.GetJoystickNames();

        if (temp!=null && temp[0] != "")
        {

            if (Input.GetMouseButtonDown(0))
            {
                eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
