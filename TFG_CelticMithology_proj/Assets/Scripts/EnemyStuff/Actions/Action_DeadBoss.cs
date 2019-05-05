using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_DeadBoss : ActionBase
{
    public GameObject part_sys;
    public GameObject portalObject;

    override public BT_Status StartAction()
    {
        GameObject temp = Instantiate(part_sys);
        temp.transform.position = transform.position;
        temp.transform.rotation = Quaternion.Euler(-180, 0, 0);
        temp.GetComponent<ParticleSystem>().Play();

        gameObject.SetActive(false);

        
        GameObject go = Instantiate(portalObject);
        go.transform.position = transform.position;

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
