using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Action_CrowInvocation : ActionBase
{

    public GameObject crowPrefab;
    public int numCrows = 1;
    public float radius = 1.0f;
    public float fadeDuration = 0.25f;
    public BoxCollider2D myHitCollider;
    public GameObject inmortalGO;
    private GameObject player;


    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_CrowInvocation");
        }
        myHitCollider.enabled = false;
        inmortalGO.SetActive(true);

        StartCoroutine(InvokeCrows());

        return BT_Status.RUNNING;
    }

    IEnumerator InvokeCrows()
    {

        float theta = 0f;
        float deltTheta = (2f * Mathf.PI) / numCrows;
        GameObject[] crowsGo = new GameObject[numCrows];

        // Vector3 old_pos = Vector3.zero;
        for (int i = 0; i < numCrows; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            crowsGo[i] = Instantiate(crowPrefab);
            Vector3 tempPos = transform.position + pos;
            crowsGo[i].transform.position = new Vector3(tempPos.x, tempPos.y + 0.75f, 0f);
            theta += deltTheta;

            SpriteRenderer mySprite = crowsGo[i].GetComponentInChildren<SpriteRenderer>();
            yield return mySprite.DOFade(1.0f, fadeDuration).WaitForCompletion();

            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < numCrows; i++)
        {
            BT_Soldier crow = crowsGo[i].GetComponentInChildren<BT_Soldier>();
            crow.crowCanGo = true;
            BoxCollider2D myCol = crowsGo[i].transform.GetChild(1).GetComponent<BoxCollider2D>();
            myCol.enabled = true;
            yield return new WaitForSeconds(0.35f);
        }

        yield return new WaitForSeconds(0.5f);

        myHitCollider.enabled = true;
        inmortalGO.SetActive(false);

        //Throw bat transformation anim

        isFinish = true;
        myBT.myBB.SetParameter("invokedCrows", true);
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
