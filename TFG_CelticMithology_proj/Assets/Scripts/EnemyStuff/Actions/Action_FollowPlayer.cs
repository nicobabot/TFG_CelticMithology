using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Action_FollowPlayer : ActionBase {

    public float time_to_change_cell = 1.0f;
    float timer_changing = 0.0f;
    int cells_changed = 0;
    List<PathNode> tiles_list;

    override public BT_Status StartAction()
    {

        Vector3 destiny_pos = ((BT_Soldier)myBT).player.transform.position;

        Vector3Int cell_destiny_pos = myBT.pathfinder_scr.walkability.LocalToCell(destiny_pos);

        Vector3Int cell_pos = myBT.pathfinder_scr.walkability.LocalToCell(transform.position);

        tiles_list = myBT.pathfinder_scr.CalculatePath(new PathNode(cell_pos.x, cell_pos.y), new PathNode(cell_destiny_pos.x, cell_destiny_pos.y));


        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        timer_changing += Time.deltaTime;
        if (timer_changing > time_to_change_cell && cells_changed < tiles_list.Count)
        {
            timer_changing = 0;
            PathNode temp_node = tiles_list[cells_changed];
            int x_tile = temp_node.GetTileX();
            int y_tile = temp_node.GetTileY();
            Vector3 new_position = myBT.pathfinder_scr.walkability.CellToLocal(new Vector3Int(x_tile, y_tile, 0));
            transform.position = new_position;
            cells_changed++;
        }


        return BT_Status.RUNNING;
    }
    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }

}
