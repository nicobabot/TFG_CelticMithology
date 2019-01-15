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
        Vector3 new_position = Vector3.zero;

        if (myBT.pathfinder_scr.walkability.LocalToCell(player.transform.position) != cell_destiny_pos)
        {
            Recalculate_Path();
        }


        if (cells_changed < tiles_list.Count)
        {
            actual_node = tiles_list[cells_changed];
            int x_tile = actual_node.GetTileX();
            int y_tile = actual_node.GetTileY();
            new_position =  myBT.pathfinder_scr.walkability.CellToLocal(new Vector3Int(x_tile, y_tile, 0));

            transform.position = Vector3.MoveTowards(transform.position, new_position, speed*Time.deltaTime);

            if (transform.position == new_position)
            {
                cells_changed++;
            }

        }

        DetectDirection(new_position);

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

    void DetectDirection(Vector3 new_position)
    {

        Direction dir_x = Direction.NEUTRAL;
        Direction dir_y = Direction.NEUTRAL;

        float x = transform.position.x;
        float y = transform.position.y;

        if (x < new_position.x)
        {
            dir_x = Direction.RIGHT;
        }
        else
        {
            dir_x = Direction.LEFT;
        }

        if(y < new_position.y)
        {
            dir_y = Direction.UP;
        }
        else
        {
            dir_y = Direction.DOWN;
        }


        /* if(dir_x == Direction.RIGHT && dir_y == Direction.NEUTRAL)
         {
             //Play animation
             Debug.Log("Going Right");
             myBT.myBB.SetParameter("direction", dir_x);
         }
         else if (dir_x == Direction.LEFT && dir_y == Direction.NEUTRAL)
         {
             //Play animation
             Debug.Log("Going left");
             myBT.myBB.SetParameter("direction", dir_x);
         }
         else if(dir_y == Direction.UP && dir_x == Direction.NEUTRAL)
         {
             //Play animation
             Debug.Log("Going up");
             myBT.myBB.SetParameter("direction", dir_y);
         }
         else if (dir_y == Direction.DOWN && dir_x == Direction.NEUTRAL)
         {
             //Play animation
             Debug.Log("Going down");
             myBT.myBB.SetParameter("direction", dir_y);
         }
         else
         {*/
        float dif_x = Mathf.Abs(x - new_position.x);
            float dif_y = Mathf.Abs(y - new_position.y);

            if (dif_x> dif_y)
            {
                dir_y = Direction.NEUTRAL;
                Debug.Log("Going " + dir_x.ToString());
                myBT.myBB.SetParameter("direction", dir_x);
            }
            else
            {
                dir_x = Direction.NEUTRAL;
                Debug.Log("Going " + dir_y.ToString());
                myBT.myBB.SetParameter("direction", dir_y);
            }
        //}


    }

    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }

}
