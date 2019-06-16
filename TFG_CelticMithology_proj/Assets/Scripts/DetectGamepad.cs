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

        if (temp!=null && temp[0] != "")
        {

            if (Input.GetMouseButtonDown(0))
            {
                if(playerImpr != null&& !playerImpr.isImproving)
                    eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
                else
                    eventSystem.SetSelectedGameObject(improveGo);
                
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
