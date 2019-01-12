﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier_Blackboard : Blackboard {

    public ParameterInt life;
    public ParameterBool is_enemy_hit;
    public ParameterGameObject player;

    // Use this for initialization
    override public void Start () {
        base.Start();

        list.Add(life);
        list.Add(is_enemy_hit);
        list.Add(player);
    }
	
}
