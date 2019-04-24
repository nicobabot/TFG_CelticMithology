using UnityEngine;
using UnityEngine.UI;

public class Live_Manager : MonoBehaviour
{
    public GameObject Father_UI_Player_Live;
    private int lives = 0;
    public int hearts_division = 2;

    public Vector2 StartPosition;
    public int numHearts = 4;
    public GameObject heartPrefab;
    public float offsetHeart = 1.5f;

    // Use this for initialization
    private void Start()
    {
        if (Father_UI_Player_Live == null)
        {
            Debug.Log("Father_UI_Player_Live null");
        }
        else
        {
            lives = numHearts;

            HearthPositioning();
        }

    }

    void HearthPositioning()
    {
        //float heartWidth = heartSprite.rect.width;

       /* GameObject go = Instantiate(heartPrefab, Father_UI_Player_Live.transform);
        //go.transform.position = StartPosition;
        RectTransform rectTrans = go.GetComponent<RectTransform>();*/

        for (int i = 0; i < numHearts; i++)
        {
            GameObject go = Instantiate(heartPrefab, Father_UI_Player_Live.transform);
            go.name = "Live" + i;
            RectTransform rectTrans = go.GetComponent<RectTransform>();
            rectTrans.position = new Vector3(StartPosition.x + (rectTrans.rect.width + offsetHeart) * i, StartPosition.y, rectTrans.position.z);
        }

    }

    public void AddHeart()
    {
        GameObject[] childs_temporal_vector;
        int num_childs = Father_UI_Player_Live.transform.childCount;
        childs_temporal_vector = new GameObject[num_childs];
        int temp_index = 0;
        for (int i = num_childs; i > 0; i--)
        {
            childs_temporal_vector[temp_index] = Father_UI_Player_Live.transform.GetChild(i - 1).gameObject;
            temp_index++;
        }

        Image img_comp = childs_temporal_vector[0].GetComponent<Image>();
        if (img_comp.fillAmount == 1)
        {
            GameObject go = Instantiate(heartPrefab, Father_UI_Player_Live.transform);
            go.name = "Live" + lives;
            RectTransform rectTrans = go.GetComponent<RectTransform>();
            Transform trans = childs_temporal_vector[0].transform;
            rectTrans.position = new Vector3(trans.position.x + (rectTrans.rect.width + offsetHeart), trans.position.y, trans.position.z);
        }
        else if (img_comp.fillAmount == 0.5f)
        {
            img_comp.fillAmount += 0.5f;

            GameObject go = Instantiate(heartPrefab, Father_UI_Player_Live.transform);
            go.name = "Live" + lives;
            RectTransform rectTrans = go.GetComponent<RectTransform>();
            Transform trans = childs_temporal_vector[0].transform;
            rectTrans.position = new Vector3(trans.position.x + (rectTrans.rect.width + offsetHeart), trans.position.y, trans.position.z);

            Image img = go.GetComponent<Image>();
            img.fillAmount -= 0.5f;

        }

      
    }

    // Update is called once per frame
    private void Update()
    {
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
        for (int i = num_childs; i > 0; i--)
        {
            childs_temporal_vector[temp_index] = Father_UI_Player_Live.transform.GetChild(i - 1).gameObject;
            temp_index++;
        }

        foreach (GameObject go in childs_temporal_vector)
        {
            Image img_comp = go.GetComponent<Image>();
            float filled = img_comp.fillAmount;
            bool modification = false;

            float slice = 1.0f / (float)hearts_division;

            for (int i = 1; i <= hearts_division; i++)
            {
                float value_sliced = slice * i;

                if (filled == 0.0f)
                {
                    break;
                }
                else if (filled == Mathf.Clamp(value_sliced, 0.0f, filled))
                {
                    img_comp.fillAmount -= slice;

                    if (img_comp.fillAmount == 0.0f)
                    {
                        Destroy(go);
                    }

                    modification = true;
                    break;
                }
            }

            if (modification == true)
            {
                break;
            }
        }
    }
}