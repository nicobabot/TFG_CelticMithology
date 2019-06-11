using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Action_ThrowBouncingBalls : ActionBase
{

    public GameObject WaterBall;
    public GameObject InmortalGo;
    public BoxCollider2D Collider;
    public Action_FollowPlayer followPlayer;
    public Transform rightTrans;

    [Header("Positions")]
    public GameObject Positions;

    private SpriteRenderer mySpriteRend;
    private Animator myAnimator;

    public int numOfBalls = 3;
    public float timeBetweenSpawn = 0.5f;
    GameObject player;
    float timerCountBalls = 0.0f;
    int ballsSpawned = 0;
    Transform tempTrans;
    bool throwBalls = false;
    bool doneThrow = false;

    float timerAnimation = 0.0f;

    override public BT_Status StartAction()
    {
        Collider.enabled = false;
        InmortalGo.SetActive(true);
        timerCountBalls = timeBetweenSpawn;
        ballsSpawned = 0;
        throwBalls = false;
        player = (GameObject)myBT.myBB.GetParameter("player");
        tempTrans = Positions.transform.GetChild(Random.Range(0, Positions.transform.childCount));
        timerAnimation = 0;
        doneThrow = false;

        mySpriteRend = (SpriteRenderer)myBT.myBB.GetParameter("mySpriteRend");
        myAnimator = (Animator)myBT.myBB.GetParameter("myAnimator");
        myAnimator.SetBool("KelpieGoUnderground", false);


        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        Direction testDir = followPlayer.DetectDirection(transform.position, player.transform.position);
        switch (testDir)
        {
            case Direction.LEFT:
                mySpriteRend.flipX = false;
                break;
            case Direction.RIGHT:
                mySpriteRend.flipX = true;
                break;
        }


        if (!throwBalls) {

            transform.position = Vector3.MoveTowards(transform.position, tempTrans.position, Time.deltaTime * 5.0f);
            if ((tempTrans.position - transform.position).magnitude < 0.1f)
            {
                myAnimator.SetBool("KelpieShoot", true);
                throwBalls = true;
            }
        }
        else
        {

            timerAnimation += Time.deltaTime;

            if (timerAnimation >= 1.25f && !doneThrow)
            {
                for (int i = 0; i < numOfBalls; i++)
                {
                    timerCountBalls = 0;
                    ballsSpawned++;
                    GameObject go = Instantiate(WaterBall);
                    BouncingBall bounce = go.GetComponent<BouncingBall>();

                    if (bounce != null)
                    {
                        Direction dir;
                        if (transform.position.x > player.transform.position.x) dir = Direction.LEFT;
                        else dir = Direction.RIGHT;

                        switch (dir)
                        {
                            case Direction.LEFT:
                                Vector3 left = -rightTrans.transform.right;

                                if (i == 0)
                                {
                                    bounce.SetDirection(left);
                                }
                                else if (i == 1)
                                {
                                    Vector3 newDirUp = Quaternion.AngleAxis(45.0f, Vector3.forward) * left;
                                    bounce.SetDirection(newDirUp);
                                }
                                else if (i == 2)
                                {
                                    Vector3 newDirDown = Quaternion.AngleAxis(-45.0f, Vector3.forward) * left;
                                    bounce.SetDirection(newDirDown);
                                }
                                break;
                            case Direction.RIGHT:
                                Vector3 right = rightTrans.transform.right;

                                if (i == 0)
                                {
                                    bounce.SetDirection(right);
                                }
                                else if (i == 1)
                                {
                                    Vector3 newDirUp = Quaternion.AngleAxis(45.0f, Vector3.forward) * right;
                                    bounce.SetDirection(newDirUp);
                                }
                                else if (i == 2)
                                {
                                    Vector3 newDirDown = Quaternion.AngleAxis(-45.0f, Vector3.forward) * right;
                                    bounce.SetDirection(newDirDown);
                                }


                                break;
                        }
                    }
                    go.transform.position = transform.position;
                }

                doneThrow = true;
            }
            else if (timerAnimation >= 2.10f && doneThrow)
            {
                myAnimator.SetBool("KelpieShoot", false);
                doneThrow = false;
                isFinish = true;
                InmortalGo.SetActive(false);
            }
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }
}

