using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_SkeletonInvoke : ActionBase
{
    public CircleCollider2D colInvoke;
    public int numSkeletons;
    public GameObject skeletontObj;

    public GameObject rayGO;
    private Transform rayTrans;

    private GameObject player;
    private float speed = 1.0f;
    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }
        rayTrans = rayGO.transform;

        rayGO.SetActive(true);

        StartCoroutine(RayShoot());

        return BT_Status.RUNNING;
    }

    IEnumerator RayShoot()
    {
        SpawnSkeletons();

        StartCoroutine(StartFocusingWithRay());

        yield return new WaitForSeconds(5.0f);



    }

    IEnumerator StartFocusingWithRay()
    {

        Vector3 vecToPlayer = player.transform.position - rayTrans.transform.position;
        vecToPlayer.z = 0;
        Debug.DrawRay(rayTrans.position, vecToPlayer, Color.red);

        float angToRotate = Vector3.Angle(rayTrans.right, vecToPlayer);

        //rayTrans.rotation = Quaternion.AngleAxis(angToRotate, rayTrans.forward);
        rayTrans.eulerAngles = new Vector3(0, 0, angToRotate);


        /*float step = speed * Time.deltaTime;


        //Vector3 newDir = Vector3.RotateTowards(rayTrans.forward, vecToPlayer, step, 0.0f);
        Debug.DrawRay(rayTrans.position, newDir, Color.red);

        rayTrans.rotation = Quaternion.LookRotation(newDir);*/

        yield return new WaitForSeconds(0.01f);

        StartCoroutine(StartFocusingWithRay());
    }

    void SpawnSkeletons()
    {
        for (int i = 0; i < numSkeletons; i++)
        {
            GameObject go = Instantiate(skeletontObj);
            Vector2 circlePos = new Vector2(colInvoke.transform.position.x, colInvoke.transform.position.y);
            Vector2 newPos = Random.insideUnitCircle * colInvoke.radius + circlePos;
            go.transform.position = newPos;
            go.GetComponent<SkeletonDetection>().SetMorriganBB((Morrigan_Blackboard)myBT.myBB);
        }
    }

    override public BT_Status UpdateAction()
    {
        Vector3 vecToPlayer = player.transform.position - rayTrans.transform.position;
        vecToPlayer.z = 0;
        Debug.DrawRay(rayTrans.position, vecToPlayer, Color.red);

        float angToRotate = Vector3.Angle(rayTrans.right, vecToPlayer);

        //rayTrans.rotation = Quaternion.AngleAxis(angToRotate, rayTrans.forward);
        rayTrans.eulerAngles = new Vector3(0, 0, -angToRotate);
        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }

}
