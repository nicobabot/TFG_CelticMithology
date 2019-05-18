using System.Collections.Generic;
using UnityEngine;

public class Action_WanderAttack : ActionBase
{
    //Values calculate path

    [SerializeField] private Transform RandomCalculatorGO;
    [SerializeField] private float speed;
    private CircleCollider2D circCollider;

    private Vector2 destiny_pos = Vector3.zero;
    private bool needToGetDir = true;
    private SpriteRenderer mySpriteRend;
    private Animator myAnimator;


    bool can_reach = true;

    override public BT_Status StartAction()
    {
        mySpriteRend = (SpriteRenderer)myBT.myBB.GetParameter("mySpriteRend");

        myAnimator = (Animator)myBT.myBB.GetParameter("myAnimator");

        circCollider = RandomCalculatorGO.GetComponent<CircleCollider2D>();

        /*if (myAnimator != null && myBT.enemy_type != Enemy_type.KELPIE_ENEMY)
        {
            myAnimator.SetBool("enemy_startwalking", true);
            myAnimator.SetFloat("offsetAnimation", Random.Range(0.0f, 1.0f));
        }*/

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        if (needToGetDir)
        {
            needToGetDir = false;
            CalculateDir();
        }

        transform.position = Vector3.MoveTowards(transform.position, destiny_pos, speed * Time.deltaTime);

        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);

        if ((destiny_pos - myPos).magnitude < 0.1f)
        {
            needToGetDir = true;
        }

        return BT_Status.RUNNING;
    }

    void CalculateDir()
    {
        Vector2 myPos = new Vector2(RandomCalculatorGO.position.x, RandomCalculatorGO.position.y);

        destiny_pos = myPos + (Random.insideUnitCircle * circCollider.radius);
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}