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
    private BoxCollider2D rayCol;
    private SpriteRenderer raySpr;

    private GameObject player;
    private float speed = 1.0f;
    private Coroutine rayCoroutine;
    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }
        rayTrans = rayGO.transform;
        rayCol = rayGO.GetComponentInChildren<BoxCollider2D>();
        raySpr = rayGO.GetComponentInChildren<SpriteRenderer>();

        rayGO.SetActive(true);
        rayCol.enabled = false;
        raySpr.color = new Color(raySpr.color.r, raySpr.color.g, raySpr.color.b, 0.3294118f);

        StartCoroutine(RayShoot());

        return BT_Status.RUNNING;
    }

    IEnumerator RayShoot()
    {
        SpawnSkeletons();

        rayCoroutine = StartCoroutine(StartFocusingWithRay());

        yield return new WaitForSeconds(5.0f);

        if ((bool)myBT.myBB.GetParameter("invSkelPlayerDet"))
        {
            yield return new WaitForSeconds(1.5f);
            ExecuteRay();
        }
        else
        {
            ExecuteRay();
        }

        yield return new WaitForSeconds(1.0f);

        isFinish = true;

        rayGO.SetActive(false);
    }

    void ExecuteRay()
    {
        StopCoroutine(rayCoroutine);
        raySpr.color = Color.white;
        rayCol.enabled = true;
    }

    IEnumerator StartFocusingWithRay()
    {
        Vector3 vecToPlayer = player.transform.position - rayTrans.transform.position;
        vecToPlayer.z = 0;
        rayTrans.right = -vecToPlayer;

        yield return new WaitForSeconds(0.01f);

        rayCoroutine = StartCoroutine(StartFocusingWithRay());
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


        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }

}
