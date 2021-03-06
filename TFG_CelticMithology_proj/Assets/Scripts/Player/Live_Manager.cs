﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Live_Manager : MonoBehaviour
{
    public GameObject Father_UI_Player_Live;
    public AudioClip healSound;
    public AudioSource source;

    [HideInInspector] public float lives = 0;
    [HideInInspector] public int maxLives = 0;
    public int hearts_division = 2;

    public Transform liveTransform;
    public Vector2 StartPosition;
    [HideInInspector]public int numHearts = 4;
    public GameObject heartPrefab;
    public float offsetHeart = 1.5f;

    private bool _healed = true;
    private bool _canHeal = true;
    private float timerToHealAgain = 0.0f;

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
            maxLives = numHearts;

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
             AddUiHeart(i);
        }

    }

    Image AddUiHeart(int num)
    {
        GameObject go = Instantiate(heartPrefab, Father_UI_Player_Live.transform);
        go.name = "Live" + num;
        RectTransform rectTrans = go.GetComponent<RectTransform>();
        //rectTrans.position = new Vector3(StartPosition.x + (rectTrans.rect.width + offsetHeart) * num, StartPosition.y, rectTrans.position.z);
        rectTrans.position = new Vector3(liveTransform.position.x + (rectTrans.rect.width + offsetHeart) * num, liveTransform.position.y, rectTrans.position.z);
        
        return go.transform.GetChild(0).GetComponent<Image>();
    }

    public void AddHeart(bool healing = false)
    {
        if (lives < 0.0f)
            return;

        source.clip = healSound;
        source.Play();

        GameObject[] childs_temporal_vector;
        int num_childs = Father_UI_Player_Live.transform.childCount;
        childs_temporal_vector = new GameObject[num_childs];
        int temp_index = 0;
        for (int i = 0; i < num_childs; i++)
        {
            childs_temporal_vector[i] = Father_UI_Player_Live.transform.GetChild(i).gameObject;
        }

        if (healing)
        {
            _healed = true;
            _canHeal = false;

            float it = Mathf.Ceil(lives);

            if (it == maxLives)
            {
                Image imgHeal = childs_temporal_vector[(int)it - 1].transform.GetChild(0).GetComponent<Image>();
                if (imgHeal.fillAmount == 1.0f)
                    return;
            }

            if (lives == 0.5f)
            {
                Image img = childs_temporal_vector[0].transform.GetChild(0).GetComponent<Image>();
                img.fillAmount += 0.5f;
                lives += 0.5f;
            }
            else if (lives - Mathf.Round(lives) == 0)
            {
                Image img = childs_temporal_vector[(int)it].transform.GetChild(0).GetComponent<Image>();
                img.fillAmount += 0.5f;
                lives += 0.5f;
            }
            else if ((int)it - 1 < childs_temporal_vector.Length && (int)it - 1 > 0)
            {
                Image img = childs_temporal_vector[(int)it - 1].transform.GetChild(0).GetComponent<Image>();
                img.fillAmount += 0.5f;
                lives += 0.5f;
            }

          

        }
        else
        {
            float it = Mathf.Ceil(lives);

            for (int i=0; i< childs_temporal_vector.Length; i++)
            {
                Image img = childs_temporal_vector[i].transform.GetChild(0).GetComponent<Image>();
                img.fillAmount = 1.0f;
            }

            AddUiHeart((int)childs_temporal_vector.Length);
            lives = childs_temporal_vector.Length + 1.0f;
            maxLives = (int)lives;

            /*if (it >= maxLives)
            {
                Image img = childs_temporal_vector[(int)it-1].GetComponent<Image>();
                if (img.fillAmount == 1.0f)
                {
                    AddUiHeart((int)lives);
                    lives += 1.0f;
                }
                else if (img.fillAmount == 0.5f)
                {
                    img.fillAmount += 0.5f;

                    Image imgTwo = AddUiHeart((int)lives + 1);
                    imgTwo.fillAmount = 0.5f;

                    lives += 1;
                }
            }
            else if(it - Mathf.Round(lives) == 0)
            {
                Image img = childs_temporal_vector[(int)it].GetComponent<Image>();
                img.fillAmount += 1.0f;
                lives += 1.0f;
            }
            else if(it - lives > 0)
            {
                Image img = childs_temporal_vector[(int)it-1].GetComponent<Image>();
                img.fillAmount += 0.5f;
                lives += 0.5f;

                if(childs_temporal_vector[(int)it] != null)
                {
                    Image imgTwo = childs_temporal_vector[(int)it].GetComponent<Image>();
                    imgTwo.fillAmount += 0.5f;
                    lives += 0.5f;
                }

            }*/

        }

        /*Image img_comp = childs_temporal_vector[0].GetComponent<Image>();
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

        }*/


    }

    // Update is called once per frame
    private void Update()
    {
        if (_healed)
        {
            timerToHealAgain += Time.deltaTime;
            if (timerToHealAgain>0.1f)
            {
                timerToHealAgain = 0;
                _canHeal = true;
            }
        }

        if (lives <= 0)
        {
            SceneManager.LoadScene("DeadMenu", LoadSceneMode.Single);
        }
    }


    public void DetectedDamage()
    {
        lives -= 0.5f;

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
            Image img_comp = go.transform.GetChild(0).GetComponent<Image>();
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

                    /*if (img_comp.fillAmount == 0.0f)
                    {
                        Destroy(go);
                    }*/

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("health_object"))
        {
            GameObject go = collision.gameObject;
            if (_canHeal)
            {
                Destroy(collision);
                Destroy(go);
                AddHeart(true);
            }

        }
    }


}