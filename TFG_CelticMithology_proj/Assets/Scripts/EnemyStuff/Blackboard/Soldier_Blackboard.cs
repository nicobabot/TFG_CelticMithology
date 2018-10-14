using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier_Blackboard : Blackboard {

    public ParameterInt life;


    // Use this for initialization
    override public void Start () {
        base.Start();

        list.Add(life);

    }
	
}
