using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Dead : ActionBase
{
    public GameObject part_sys;
    public GameObject healthObject;

    override public BT_Status StartAction()
    {
        GameObject temp = Instantiate(part_sys);
        temp.transform.position = transform.position;
        temp.transform.rotation = Quaternion.Euler(-180, 0, 0);
        temp.GetComponent<ParticleSystem>().Play();

        gameObject.SetActive(false);

        int result = Random.Range(1, 101);
        if(result >= 90 && result <= 100)
        {
            GameObject go = Instantiate(healthObject);
            go.transform.position = transform.position;
        }

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
