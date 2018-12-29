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

    override public BT_Status StartAction()
    {
        Recalculate_Path();

        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {
        if (myBT.pathfinder_scr.walkability.LocalToCell(((BT_Soldier)myBT).player.transform.position) != cell_destiny_pos)
        {
            Recalculate_Path();
        }
        else if (cells_changed < tiles_list.Count)
        {
            PathNode temp_node = tiles_list[cells_changed];
            int x_tile = temp_node.GetTileX();
            int y_tile = temp_node.GetTileY();
            Vector3 new_position = myBT.pathfinder_scr.walkability.CellToLocal(new Vector3Int(x_tile, y_tile, 0));

            //Anchor point
            float y_pos = new_position.y + GetComponent<SpriteRenderer>().size.y * 0.5f;
            new_position.y = y_pos;


            float new_x = transform.position.x;
            float new_y = transform.position.y;

            bool is_x_bigger = false;
            bool is_y_bigger = false;

            if (new_position.x > new_x) {
                is_x_bigger = true;
                new_x += Time.deltaTime * speed;
            }
            else
                new_x -= Time.deltaTime * speed;

            if (new_position.y > new_y) {
                is_y_bigger = true;
                new_y += Time.deltaTime * speed;
            }
            else
                new_y -= Time.deltaTime * speed;

            transform.position = new Vector3(new_x, new_y, 0);

            bool can_change_x = false;
            bool can_change_y = false;

            if (is_x_bigger) {
                if (transform.position.x >= new_position.x - speed*Time.deltaTime)
                {
                    can_change_x = true;
                }
            }
            else
            {
                if (transform.position.x <= new_position.x + speed * Time.deltaTime)
                {
                    can_change_x = true;
                }

            }

            if (is_y_bigger)
            {
                if (transform.position.y >= new_position.y - speed * Time.deltaTime)
                {
                    can_change_y = true;
                }
            }
            else
            {
                if (transform.position.y <= new_position.y + speed * Time.deltaTime)
                {
                    can_change_y = true;
                }

            }

            if (can_change_x == true && can_change_y == true)
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

        destiny_pos = ((BT_Soldier)myBT).player.transform.position;

        cell_destiny_pos = myBT.pathfinder_scr.walkability.LocalToCell(destiny_pos);

        cell_pos = myBT.pathfinder_scr.walkability.LocalToCell(transform.GetChild(0).transform.position);

        tiles_list = myBT.pathfinder_scr.CalculatePath(new PathNode(cell_pos.x, cell_pos.y), new PathNode(cell_destiny_pos.x, cell_destiny_pos.y));
    }

    override public BT_Status EndAction()
    {

        return BT_Status.SUCCESS;
    }

}
