using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Action_FollowPlayer : ActionBase {

    
    override public BT_Status StartAction()
    {

        Vector3 destiny_pos = ((BT_Soldier)myBT).player.transform.position;

        Vector3Int cell_destiny_pos = myBT.pathfinder_scr.walkability.LocalToCell(destiny_pos);

        Vector3Int cell_pos = myBT.pathfinder_scr.walkability.LocalToCell(transform.position);

        List<PathNode> tiles_list = myBT.pathfinder_scr.CalculatePath(new PathNode(cell_pos.x, cell_pos.y), new PathNode(cell_destiny_pos.x, cell_destiny_pos.y));


        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

       
        return BT_Status.RUNNING;
    }
    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }

}
