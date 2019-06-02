using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_SkeletonInvoke : ActionBase
{
    public CircleCollider2D colInvoke;
    public int numSkeletons;
    public GameObject skeletontObj;
    public BoxCollider2D myHitCollider;
    public float offsetBetweenSkeletons = 1.5f;
    private CircleCollider2D skeletonCol;

    public GameObject rayGO;
    private Transform rayTrans;
    private BoxCollider2D rayCol;
    private SpriteRenderer raySpr;
    private PlayerDetection_Damage rayDetect;

    private GameObject player;
    private Player_Manager managerPlayer;
    private float speed = 1.0f;
    private Coroutine rayCoroutine;
    private Coroutine actionCoroutine;
    private Transform[] sheletonsInstancied;

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

        skeletonCol = skeletontObj.GetComponent<CircleCollider2D>();
        managerPlayer = player.GetComponent<Player_Manager>();

        rayGO.SetActive(true);
        rayCol.enabled = false;
        raySpr.color = new Color(raySpr.color.r, raySpr.color.g, raySpr.color.b, 0.3294118f);
        rayDetect = rayGO.GetComponentInChildren<PlayerDetection_Damage>();

        sheletonsInstancied = new Transform[numSkeletons];

        actionCoroutine = StartCoroutine(RayShoot());

        return BT_Status.RUNNING;
    }

    public void StopRayRoutine()
    {
        if(actionCoroutine!=null)
        StopCoroutine(actionCoroutine);
    }

    IEnumerator RayShoot()
    {
        rayGO.SetActive(true);
        myHitCollider.enabled = false;

        SpawnSkeletons();

        rayCoroutine = StartCoroutine(StartFocusingWithRay());

        yield return new WaitForSeconds(5.0f);

        if ((bool)myBT.myBB.GetParameter("invSkelPlayerDet"))
        {
            StopCoroutine(rayCoroutine);

            managerPlayer.current_state = Player_Manager.Player_States.MID_STUNNED_PLAYER;
            yield return new WaitForSeconds(2.0f);
            ExecuteRay();
        }
        else
        {
            StopCoroutine(rayCoroutine);
            yield return new WaitForSeconds(0.5f);
            ExecuteRay();
        }

        myHitCollider.enabled = true;

        yield return new WaitForSeconds(1.0f);

        ResetAction();

    }

    void ExecuteRay()
    {
        raySpr.color = Color.white;
        rayCol.enabled = true;
    }

    void ResetAction()
    {
        StopRayRoutine();
        isFinish = true;
        myBT.myBB.SetParameter("invSkelPlayerDet", false);
        rayGO.SetActive(false);
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
            Vector2 newPos = Vector2.zero;

            do
            {
                newPos = Random.insideUnitCircle * colInvoke.radius + circlePos;
            } while (IsSuperPosingAnything(newPos));


            go.transform.position = newPos;
            go.GetComponent<SkeletonDetection>().SetMorriganBB((Morrigan_Blackboard)myBT.myBB);
            sheletonsInstancied[i] = go.transform;
        }
    }

    bool IsSuperPosingAnything(Vector3 skeletonPos)
    {
        bool ret = false;
        for (int i = 0; i < sheletonsInstancied.Length; i++)
        {

            if (sheletonsInstancied[i] == null) break;

            if ((sheletonsInstancied[i].position - skeletonPos).magnitude < skeletonCol.radius + offsetBetweenSkeletons)
            {
                ret = true;
                break;
            }

        }
        return ret;
    }

    override public BT_Status UpdateAction()
    {
        if (rayDetect.playerHitted)
        {
            ((BT_Morrigan)myBT).wantToInvoke = true;
            rayDetect.playerHitted = false;

            ResetAction();
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }

}
