using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Action_FollowPlayer : ActionBase {

    public float time_to_change_cell = 1.0f;
    public float speed = 5.0f;
    float timer_changing = 0.0f;
    int cells_changed = 0;
    List<PathNode> tiles_list;

    //Values calculate path
    Vector3 destiny_pos = Vector3.zero;
    Vector3Int cell_destiny_pos = Vector3Int.zero;
    Vector3Int cell_pos = Vector3Int.zero;
    PathNode actual_node;
    GameObject player;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if(player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        Recalculate_Path();

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {

        if (myBT.pathfinder_scr.walkability.LocalToCell(player.transform.position) != cell_destiny_pos)
        {
            Recalculate_Path();
        }


        if (cells_changed < tiles_list.Count)
        {
            actual_node = tiles_list[cells_changed];
            int x_tile = actual_node.GetTileX();
            int y_tile = actual_node.GetTileY();
            Vector3 new_position = myBT.pathfinder_scr.walkability.CellToLocal(new Vector3Int(x_tile, y_tile, 0));

            transform.position = Vector3.MoveTowards(transform.position, new_position, speed*Time.deltaTime);

            if (transform.position == new_position)
            {
                cells_changed++;
            }

        }


        return BT_Status.RUNNING;
    }

    public void Recalculate_Path()
    {
        if(tiles_list!=null)
        tiles_list.Clear();

        timer_changing = 0;
        cells_changed = 0;

        destiny_pos = player.transform.position;

        cell_destiny_pos = myBT.pathfinder_scr.walkability.LocalToCell(destiny_pos);

        cell_pos = myBT.pathfinder_scr.walkability.LocalToCell(transform.position);

        tiles_list = myBT.pathfinder_scr.CalculatePath(new PathNode(cell_pos.x, cell_pos.y), new PathNode(cell_destiny_pos.x, cell_destiny_pos.y));
    }

    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }

}
