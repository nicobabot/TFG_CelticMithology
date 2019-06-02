using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_DeadEnemySpawn : ActionBase
{
    public GameObject part_sys;
    public GameObject enemySpawn;
    public Transform middlePoint;

    override public BT_Status StartAction()
    {
        GameObject temp = Instantiate(part_sys);
        temp.transform.position = transform.position;
        temp.transform.rotation = Quaternion.Euler(-180, 0, 0);
        temp.GetComponent<ParticleSystem>().Play();

        gameObject.SetActive(false);

        
        GameObject go = Instantiate(enemySpawn);
        go.transform.position = middlePoint.position;
        if(myBT.enemy_type == Enemy_type.DAGDA_ENEMY)
        {
            Dullahan_Blackboard bb = go.GetComponentInChildren<Dullahan_Blackboard>();
            bb.playerIsInsideRoom.SetValue(true);
        }
        if (myBT.enemy_type == Enemy_type.DULLAHAN_ENEMY)
        {
            Morrigan_Blackboard bb = go.GetComponentInChildren<Morrigan_Blackboard>();
            bb.playerIsInsideRoom.SetValue(true);
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
