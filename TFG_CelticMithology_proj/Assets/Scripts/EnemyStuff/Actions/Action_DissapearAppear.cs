using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Action_DissapearAppear : ActionBase
{
    public float rangeTeleport;
    public float timeDissapeared;
    public float speedAlpha = 1;
    public BoxCollider2D[] col;
    public GameObject myCanvas;

    private GameObject player;

    Vector3 teleportPosition;
    SpriteRenderer mySprite;


    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        Vector2 temp = Random.insideUnitCircle * rangeTeleport;
        teleportPosition = player.transform.position + new Vector3(temp.x, temp.y, 0);

        mySprite = (SpriteRenderer)myBT.myBB.GetParameter("mySpriteRend");

        StartCoroutine(FadeOffOn());
        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
     

        return BT_Status.RUNNING;
    }

    IEnumerator FadeOffOn()
    {
        myCanvas.SetActive(false);
        DisableEnableCol(false);

        mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, mySprite.color.a - speedAlpha);

        yield return mySprite.DOFade(0.0f, 0.25f).WaitForCompletion();

        transform.position = teleportPosition;

        yield return new WaitForSeconds(timeDissapeared);

        yield return mySprite.DOFade(1.0f, 0.25f).WaitForCompletion();

        myCanvas.SetActive(true);
        DisableEnableCol(true);

        isFinish = true;
    }

    void DisableEnableCol(bool activate)
    {
        foreach(BoxCollider2D myCol in col)
        {
            myCol.enabled = activate;
        }
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }

}
