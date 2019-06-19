using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Action_FollowPlayerRanged))]
public class Action_MeleSlashPlayer : ActionBase
{

    public float time_to_make_slash;
    public GameObject father_colliders;
    public LayerMask player_mask;
    public float yStartDagdaText;
    public float yEndDagdaText;
    public Image filler;
    public CircleCollider2D circleCol;

    [Header("The collider that player detects to make damage the enemy")]
    public BoxCollider2D get_damage_collider;

    private Action_FollowPlayerRanged follow_playerRanged_scr;
    private Action_FollowPlayer follow_player_scr;

    private GameObject player;
    private float timer_attack = 0.0f;
    private bool slash_done = false;
    private bool is_player_detected = false;
    private Collider2D player_detection_slash;
    private Direction dir_collider;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }
        if(myBT.enemy_type == Enemy_type.DULLAHAN_ENEMY)
            follow_playerRanged_scr = GetComponent<Action_FollowPlayerRanged>();
        else follow_player_scr = GetComponent<Action_FollowPlayer>();


        //EndActionDisable();

        player_detection_slash = null;
        is_player_detected = false;

        if (ProceduralDungeonGenerator.mapGenerator.damageDagda != null)
        {
            ProceduralDungeonGenerator.mapGenerator.damageDagda.enabled = false;
            ProceduralDungeonGenerator.mapGenerator.damageDagda.gameObject.transform.localPosition = new Vector3(ProceduralDungeonGenerator.mapGenerator.damageDagda.gameObject.transform.localPosition.x, yStartDagdaText);
            ProceduralDungeonGenerator.mapGenerator.damageDagda.alpha = 1.0f;
        }

        if (myBT.enemy_type == Enemy_type.DULLAHAN_ENEMY)
            dir_collider = follow_playerRanged_scr.DetectDirection(transform.position, player.transform.position);
        else dir_collider = follow_player_scr.DetectDirection(transform.position, player.transform.position);
        return BT_Status.RUNNING;
    }

   /* private void OnDrawGizmosSelected()
    {
        GameObject go = father_colliders.transform.GetChild((int)dir_collider).gameObject;

        BoxCollider2D col = go.GetComponent<BoxCollider2D>();
        Vector3 posToTest = Vector3.zero;
        posToTest = col.transform.position + new Vector3(col.offset.x, col.offset.y);


        Gizmos.DrawCube(posToTest, col.size);
    }*/

    override public BT_Status UpdateAction()
    {

        if((bool)myBT.myBB.GetParameter("is_enemy_hit") == true)
        {
            EndActionDisable();
            return BT_Status.FAIL;
        }


        timer_attack += Time.deltaTime;

        GameObject go = null;

        go = father_colliders.transform.GetChild((int)dir_collider).gameObject;

        go.SetActive(true);

        if (filler != null)
        {
            filler.enabled = true;
            filler.fillAmount = 1 - (timer_attack / (time_to_make_slash * 0.5f));
        }

        float realTimeSlash = 0.0f;

        if(myBT.enemy_type == Enemy_type.DAGDA_ENEMY || myBT.enemy_type == Enemy_type.DEARDUG_ENEMY)
        {
            realTimeSlash = time_to_make_slash;
        }
        else
        {
            realTimeSlash = time_to_make_slash - 0.49f;
        }

        if (timer_attack >= (time_to_make_slash*0.5f))
        {

            if (timer_attack < realTimeSlash)
            {
                //get_damage_collider.enabled = false;
                BoxCollider2D col = go.GetComponent<BoxCollider2D>();

                Vector3 posToTest = Vector3.zero;

                posToTest = col.transform.position + new Vector3(col.offset.x, col.offset.y);

                player_detection_slash = Physics2D.OverlapBox(posToTest, col.size, 0, player_mask);

                if (player_detection_slash != null)
                {
                    Transform parent = player_detection_slash.transform.parent;
                    if (parent != null)
                    {
                        is_player_detected = true;
                        Player_Manager player_manager = parent.GetComponent<Player_Manager>();
                        if (myBT.enemy_type == Enemy_type.DAGDA_ENEMY)
                        {
                            player_manager.GetDamage(transform, false);
                            if (ProceduralDungeonGenerator.mapGenerator.damageDagda)
                            {
                                ProceduralDungeonGenerator.mapGenerator.damageDagda.enabled = true;
                                ProceduralDungeonGenerator.mapGenerator.damageDagda.gameObject.transform.DOLocalMoveY(yEndDagdaText, 0.15f).OnComplete(() => ProceduralDungeonGenerator.mapGenerator.damageDagda.DOFade(0.0f, 0.5f));
                            }
                        }
                        else player_manager.GetDamage(transform);
                    }

                }

                go.SetActive(false);
            }
            else
            {
                if (go != null)
                    go.SetActive(false);



                EndActionDisable();
                return BT_Status.SUCCESS;
            }
        }
        return BT_Status.RUNNING;
    }

    void EndActionDisable()
    {

        isFinish = true;

        is_player_detected = false;
        Disable_Colliders_Attack();

        if (filler != null)
        {
            filler.fillAmount = 1;
            filler.enabled = false;
        }

        player_detection_slash = null;
        timer_attack = 0.0f;
        //get_damage_collider.enabled = true;
    }

    public void Disable_Colliders_Attack()
    {
        int num_col = father_colliders.transform.childCount;

        for (int i=0; i < num_col; i++)
        {
            father_colliders.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
