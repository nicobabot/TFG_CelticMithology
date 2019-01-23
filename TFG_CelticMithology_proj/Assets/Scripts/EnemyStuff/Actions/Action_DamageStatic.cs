using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Action_DamageStatic : ActionBase
{

    public float time_doing_damage = 0.5f;
    public Color damage;

    float time_damage = 0.0f;
    SpriteRenderer sprite_rend;
    Color normal_color;

    override public BT_Status StartAction()
    {
        sprite_rend = gameObject.GetComponent<SpriteRenderer>();
        normal_color = sprite_rend.color;
        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        time_damage += Time.deltaTime;

        sprite_rend.color = damage;

        if (time_damage >= time_doing_damage)
        {
            myBT.myBB.SetParameter("is_enemy_hit", false);
            sprite_rend.color = normal_color;
            time_damage = 0;
            isFinish = true;
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}