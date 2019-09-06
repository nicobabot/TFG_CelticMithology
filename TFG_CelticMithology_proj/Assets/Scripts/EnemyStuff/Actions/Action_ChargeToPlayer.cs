using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action_ChargeToPlayer : ActionBase
{
    [Header("Charge")]
    public LayerMask wall_layer;
    public GameObject inmortalGO;
    public float charge_speed = 5;
    public Collider2D charge_collider;
    public Image Charger_Filler;
    public Collider2D get_damage;

    [HideInInspector]public bool can_charge = false;
    

    [Header("Stunned")]
    public float Time_stunned = 1;
    private float timer_stunned = 0.0f;
    public Image Stunned_Filler;

    public enum ChargingState
    {
        STARTING_ACTION,
        WAITING_TO_CHARGE,
        CHARGING,
        STUNNED
    }
    public ChargingState state_charge = ChargingState.STARTING_ACTION;

    private GameObject player;
    private Vector3 point_to_charge = Vector3.zero;

    private float time_charging_anim = 1.33f;
    private float timer_charging = 0.0f;

    private Animator myAnimator;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        myAnimator = (Animator)myBT.myBB.GetParameter("myAnimator");

        myAnimator.SetInteger("Phase", 2);

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        float step = 0;

        //Make animation of waiting for charge
        if (!can_charge)
        {
            state_charge = ChargingState.WAITING_TO_CHARGE;
            timer_charging += Time.deltaTime;

            get_damage.enabled = false;

            myAnimator.SetBool("player_hit", false);

            Charger_Filler.enabled = true;
            Charger_Filler.fillAmount = 1 - timer_charging / time_charging_anim;

            if (timer_charging > time_charging_anim)
            {
                Charger_Filler.enabled = false;
                can_charge = true;
                charge_collider.enabled = true;
                CalculatePointToCharge();
            }

        }
        else
        {
            Vector3 mag_to_point = Vector3.zero;

            if (state_charge != ChargingState.STUNNED)
            {
                state_charge = ChargingState.CHARGING;
                step = Time.deltaTime * charge_speed;

                mag_to_point = point_to_charge - transform.position;
            }

            if (mag_to_point.magnitude < 0.85f)
            {
                if ((bool)myBT.myBB.GetParameter("player_detected_charging") && state_charge != ChargingState.STUNNED)
                {
                    //Next charge
                    can_charge = false;
                    myBT.myBB.SetParameter("player_detected_charging", false);
                    myAnimator.SetBool("player_hit", true);
                    ResetValues();
                }
                else
                {
                    state_charge = ChargingState.STUNNED;
                    //stunned
                    timer_stunned += Time.deltaTime;
                    get_damage.enabled = true;
                    inmortalGO.SetActive(false);

                    Stunned_Filler.enabled = true;
                    Stunned_Filler.fillAmount = 1 - timer_stunned / Time_stunned;

                    myAnimator.SetBool("Stunned", true);

                    if (timer_stunned > Time_stunned || (bool)myBT.myBB.GetParameter("is_enemy_hit") == true)
                    {
                        Stunned_Filler.enabled = false;
                        can_charge = false;
                        myBT.myBB.SetParameter("is_enemy_hit", false);
                        myBT.myBB.SetParameter("player_detected_charging", false);
                        ResetValues();
                        myAnimator.SetBool("Stunned", false);
                    }
                }
                charge_collider.enabled = false;
                //See if player get hit
                //if not stun
                //if yes next charge
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, point_to_charge, step);
            }
        }

        return BT_Status.RUNNING;
    }

    void CalculatePointToCharge()
    {
        Vector2 dir_to_player = player.transform.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir_to_player.normalized, Mathf.Infinity, wall_layer);
        if (hit != null)
        {
                point_to_charge = hit.point;
        }

        Direction dir = DetectDirection(transform.position, point_to_charge);
        myAnimator.SetFloat("direction", (float)dir);

        //Flip if goin left/rigth
        bool fliped = GetComponent<SpriteRenderer>().flipX;

        if (point_to_charge.x > transform.position.x && fliped == true)
        {

            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (point_to_charge.x < transform.position.x && fliped == false)
        {

            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point_to_charge, 0.75f);
    }

    void ResetValues()
    {
        timer_stunned = 0.0f;
        timer_charging = 0.0f;
        Stunned_Filler.fillAmount = 1.0f;
        Charger_Filler.fillAmount = 1.0f;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }

    public Direction DetectDirection(Vector3 my_position, Vector3 new_position)
    {
        Direction dir_x = Direction.NEUTRAL;
        Direction dir_y = Direction.NEUTRAL;

        float x = my_position.x;
        float y = my_position.y;

        if (x < new_position.x) dir_x = Direction.RIGHT;
        else dir_x = Direction.LEFT;

        if (y < new_position.y) dir_y = Direction.UP;
        else dir_y = Direction.DOWN;

        float dif_x = Mathf.Abs(x - new_position.x);
        float dif_y = Mathf.Abs(y - new_position.y);

        if (dif_x > dif_y)
        {
            dir_y = Direction.NEUTRAL;
            //Debug.Log("Going " + dir_x.ToString());
            myBT.myBB.SetParameter("direction", dir_x);
            return dir_x;
        }
        else
        {
            dir_x = Direction.NEUTRAL;
            //Debug.Log("Going " + dir_y.ToString());
            myBT.myBB.SetParameter("direction", dir_y);
            return dir_y;
        }
    }

}
