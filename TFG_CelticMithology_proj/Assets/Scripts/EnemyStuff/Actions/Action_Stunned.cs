using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Action_Stunned : ActionBase
{
    public float Time_stunned = 1;
    public Image Stunned_Filler;

    private GameObject player;
    private Coroutine stunnedRoutine;
    bool wantFinished = false;
    Tween myTween;
    YieldInstruction test;
    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        wantFinished = false;
        Stunned_Filler.enabled = true;
        Stunned_Filler.fillAmount = 1.0f;
        stunnedRoutine = StartCoroutine(StunnedTime());

        return BT_Status.RUNNING;
    }

    IEnumerator StunnedTime()
    {
        //Throw animation
        myTween = Stunned_Filler.DOFillAmount(0.0f, Time_stunned);//.WaitForCompletion();
        yield return new WaitForSeconds(Time_stunned-0.1f);

        yield return test;

        EndValues();
    }

    override public BT_Status UpdateAction()
    {

        if ((bool)myBT.myBB.GetParameter("is_enemy_hit"))
        {
            StopCoroutine(stunnedRoutine);
            myTween.Kill();
            EndValues();
        }

        return BT_Status.RUNNING;
    }

    void EndValues()
    {
        myBT.myBB.SetParameter("is_enemy_stunned", false);
        Stunned_Filler.enabled = false;
        Stunned_Filler.fillAmount = 1;
        isFinish = true;
        wantFinished = false;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }

}
