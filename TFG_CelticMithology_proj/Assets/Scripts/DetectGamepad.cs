using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectGamepad : MonoBehaviour {

    public EventSystem eventSystem;
    public ImproveManager playerImpr;
    public GameObject improveGo;
    GameObject go;
	// Use this for initialization
	void Start () {
        go = eventSystem.firstSelectedGameObject;
    }
	
	// Update is called once per frame
	void Update () {

        string[] temp = Input.GetJoystickNames();

        if (temp != null && temp.Length > 0 && temp[0] != "")
        {

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                if(playerImpr != null && playerImpr.isImproving)
                    eventSystem.SetSelectedGameObject(improveGo);
                else
                    eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            eventSystem.SetSelectedGameObject(null);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
