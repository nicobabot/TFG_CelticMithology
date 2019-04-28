using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Action_ThrowBouncingBalls : ActionBase
{

    public GameObject WaterBall;
    public BoxCollider2D Collider;
    public Action_FollowPlayer followPlayer;
    public Transform rightTrans;

    [Header("Positions")]
    public GameObject Positions;

    public int numOfBalls = 3;
    public float timeBetweenSpawn = 0.5f;
    GameObject player;
    float timerCountBalls = 0.0f;
    int ballsSpawned = 0;
    Transform tempTrans;

    override public BT_Status StartAction()
    {
        Collider.enabled = false;
        timerCountBalls = timeBetweenSpawn;
        ballsSpawned = 0;
        player = (GameObject)myBT.myBB.GetParameter("player");
        tempTrans = Positions.transform.GetChild(Random.Range(0, Positions.transform.childCount));
        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        transform.position = Vector3.MoveTowards(transform.position, tempTrans.position, Time.deltaTime * 5.0f);

        if ((tempTrans.position - transform.position).magnitude < 0.1f)
        {

            //if (timerCountBalls >= timeBetweenSpawn)
            for (int i = 0; i < numOfBalls; i++)
            {
                timerCountBalls = 0;
                ballsSpawned++;
                GameObject go = Instantiate(WaterBall);
                BouncingBall bounce = go.GetComponent<BouncingBall>();

                if (bounce != null)
                {
                    Direction dir = followPlayer.DetectDirection(transform.position, player.transform.position);
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
            isFinish = true;
        }



        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }
}

