using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Live_Manager : MonoBehaviour {

    public GameObject Father_UI_Player_Live;
    int lives = 0;
    public int hearts_division = 2;

    // Use this for initialization
    void Start() {

        if (Father_UI_Player_Live == null)
        {
            Debug.Log("Father_UI_Player_Live null");
        }
        else
        { 
            lives = Father_UI_Player_Live.transform.childCount * hearts_division;
        }

    }

    // Update is called once per frame
    void Update() {


        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DetectedDamage();
        }

    }

    public void DetectedDamage()
    {
        GameObject[] childs_temporal_vector;
        int num_childs = Father_UI_Player_Live.transform.childCount;
        childs_temporal_vector = new GameObject[num_childs];
        int temp_index = 0;
        for (int i= num_childs; i>0; i--)
        {
            childs_temporal_vector[temp_index] = Father_UI_Player_Live.transform.GetChild(i-1).gameObject;
            temp_index++;
        }

        foreach(GameObject go in childs_temporal_vector)
        {
            Image img_comp = go.GetComponent<Image>();
            float filled = img_comp.fillAmount;
            bool modification = false;

            float slice = 1.0f / (float)hearts_division;

            for (int i = 1; i <= hearts_division; i++)
            {

                float value_sliced = slice * i;

                if (filled == 0.0f) {
                    break;
                }
                else if (filled == Mathf.Clamp(value_sliced, 0.0f, filled))
                {
                    img_comp.fillAmount -= slice;
                    modification = true;
                    break;
                }
            }

            if(modification == true)
            {
                break;
            }

        }
    }

}
