using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacLir_Blackboard : Blackboard
{
    public ParameterInt life;
    public ParameterInt total_life;
    public ParameterBool is_enemy_hit;
    public ParameterBool player_detected_while_charging;
    public ParameterGameObject player;
    public ParameterEnumDirection direction;
    public ParameterBool playerIsInsideRoom;

    public ParameterSpriteRend mySpriteRend;
    public ParameterAnimator myAnimator;

    // Use this for initialization
    override public void Start()
    {
        base.Start();

        list.Add(life);
        list.Add(total_life);
        list.Add(is_enemy_hit);
        list.Add(player_detected_while_charging);
        if (ProceduralDungeonGenerator.mapGenerator != null)
            player.myValue = ProceduralDungeonGenerator.mapGenerator.Player;
        list.Add(player);
        list.Add(direction);
        list.Add(playerIsInsideRoom);
        list.Add(mySpriteRend);
        list.Add(myAnimator);
    }
}
