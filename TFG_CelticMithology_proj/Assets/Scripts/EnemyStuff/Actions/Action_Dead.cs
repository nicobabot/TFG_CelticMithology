using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Action_Dead : ActionBase
{
    public GameObject part_sys;
    public GameObject healthObject;


    override public BT_Status StartAction()
    {
        GameObject temp = Instantiate(part_sys);
        temp.transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        temp.transform.rotation = Quaternion.Euler(-180, 0, 0);
        temp.GetComponent<ParticleSystem>().Play();

        ProceduralDungeonGenerator.mapGenerator.audioSourceDeads.clip = ProceduralDungeonGenerator.mapGenerator.explosionClip;
        ProceduralDungeonGenerator.mapGenerator.audioSourceDeads.Play();

        if (myBT.enemy_type == Enemy_type.MORRIGAN_ENEMY)
        {
            SceneManager.LoadScene("WinMenu", LoadSceneMode.Single);
        }
        else
        {
            gameObject.SetActive(false);

            int result = Random.Range(1, 101);
            if (result >= 80 && result <= 100)
            {
                GameObject go = Instantiate(healthObject);
                go.transform.position = transform.position;
            }
        }

        Destroy(gameObject);

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
