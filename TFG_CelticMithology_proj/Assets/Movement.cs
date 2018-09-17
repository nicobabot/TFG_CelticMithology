using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 0.0f;
    public float dead_zone = 0.15f;
    public Tilemap walkability;
    public Tile walkable_tile;
    public Tile non_walkable_tile;
    public BoxCollider2D collider;
    SpriteRenderer sprite_rend;
    Sprite player_sprite;
    bool moving_in_diagonals = false;

    Vector3 sprite_top_left = Vector3.zero;
    Vector3 sprite_top_right = Vector3.zero;

    Vector3 sprite_bottom_right = Vector3.zero;
    Vector3 sprite_bottom_left = Vector3.zero;

    enum Direction
    {
        RIGHT,
        LEFT,
        DOWN,
        UP,
        UP_RIGHT,
        UP_LEFT,
        DOWN_RIGHT,
        DOWN_LEFT
    }
    Direction current_direction;


    // Use this for initialization
    void Start () {
        sprite_rend = GetComponent<SpriteRenderer>();
        player_sprite = sprite_rend.sprite;
    }
	
	// Update is called once per frame
	void Update () {

        sprite_bottom_left = collider.bounds.min;
        sprite_top_right = collider.bounds.max;

        sprite_bottom_right = new Vector3(sprite_top_right.x, sprite_bottom_left.y);
        sprite_top_left = new Vector3(sprite_bottom_left.x, sprite_top_right.y);
        
        Vector3 temp_pos = DetectDirection();

        if (!moving_in_diagonals)
        {
            if (CanIWalk(sprite_top_left, sprite_top_right, sprite_bottom_right, sprite_bottom_left, current_direction))
            {
                transform.position = temp_pos;
            }
        }
        else
        {
            if (CanIWalkInDiagonals(sprite_top_left, sprite_top_right, sprite_bottom_right, sprite_bottom_left, current_direction))
            {
                transform.position = temp_pos;
            }
        }

    }

    Vector3 DetectDirection()
    {
        moving_in_diagonals = false;
        Vector3 temp_pos = transform.position;

        float x_movement = Input.GetAxis("Horizontal");
        float y_movement = Input.GetAxis("Vertical");

        if (x_movement> dead_zone && y_movement< dead_zone && y_movement > -(dead_zone))
        {
            temp_pos.x += Time.deltaTime * speed;
            current_direction = Direction.RIGHT;
        }
        else if (x_movement< -(dead_zone) && y_movement < dead_zone && y_movement > -(dead_zone))
        {
            temp_pos.x -= Time.deltaTime * speed;
            current_direction = Direction.LEFT;
        }
        else if (y_movement> dead_zone && x_movement < dead_zone && x_movement > -(dead_zone))
        {
            temp_pos.y += Time.deltaTime * speed;
            current_direction = Direction.UP;
        }
        else if (y_movement < -(dead_zone) && x_movement < dead_zone && x_movement > -(dead_zone))
        {
            temp_pos.y -= Time.deltaTime * speed;
            current_direction = Direction.DOWN;
        }
        else if (x_movement > dead_zone && y_movement > dead_zone)
        {
            moving_in_diagonals = true;
            temp_pos.y += Time.deltaTime * speed;
            temp_pos.x += Time.deltaTime * speed;
            current_direction = Direction.UP_RIGHT;
        }
         else if (x_movement < -(dead_zone) && y_movement > dead_zone)
         {
             moving_in_diagonals = true;
             temp_pos.y += Time.deltaTime * speed;
             temp_pos.x -= Time.deltaTime * speed;
             current_direction = Direction.UP_LEFT;
         }
         else if (x_movement > dead_zone && y_movement < -(dead_zone))
         {
             moving_in_diagonals = true;
             temp_pos.y -= Time.deltaTime * speed;
             temp_pos.x += Time.deltaTime * speed;
             current_direction = Direction.DOWN_RIGHT;
         }
         else if (x_movement < -(dead_zone) && y_movement < -(dead_zone))
         {
             moving_in_diagonals = true;
             temp_pos.y -= Time.deltaTime * speed;
             temp_pos.x -= Time.deltaTime * speed;
             current_direction = Direction.DOWN_LEFT;
         }

        return temp_pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(sprite_top_left, 0.1f);
        Gizmos.DrawWireSphere(sprite_top_right, 0.1f);
        Gizmos.DrawWireSphere(sprite_bottom_right, 0.1f);
        Gizmos.DrawWireSphere(sprite_bottom_left, 0.1f);
        
    }

    private bool CanIWalk(Vector3 sprite_top_left, Vector3 sprite_top_right, Vector3 sprite_bottom_right, Vector3 sprite_bottom_left, Direction dir)
    {
        bool ret = false;

        switch (dir)
        {
            case Direction.RIGHT:
                Vector3Int tile_top_right= Vector3Int.zero;
                tile_top_right = walkability.LocalToCell(sprite_top_right);
                tile_top_right.x += 1;

                Vector3Int tile_bottom_right = Vector3Int.zero;
                tile_bottom_right = walkability.LocalToCell(sprite_bottom_right);
                tile_bottom_right.x += 1;

                ret = RightComprovation(sprite_top_right, sprite_bottom_right, tile_top_right, tile_bottom_right);

                break;
            case Direction.LEFT:
                Vector3Int tile_top_left = Vector3Int.zero;
                tile_top_left = walkability.LocalToCell(sprite_top_left);
                tile_top_left.x -= 1;

                Vector3Int tile_bottom_left = Vector3Int.zero;
                tile_bottom_left = walkability.LocalToCell(sprite_bottom_left);
                tile_bottom_left.x -= 1;

                ret = LeftComprovation(sprite_top_left, sprite_bottom_left, tile_top_left, tile_bottom_left);
                break;
            case Direction.UP:

                Vector3Int tile_top_right_up = Vector3Int.zero;
                tile_top_right_up = walkability.LocalToCell(sprite_top_right);
                tile_top_right_up.y += 1;

                Vector3Int tile_top_left_up = Vector3Int.zero;
                tile_top_left_up = walkability.LocalToCell(sprite_top_left);
                tile_top_left_up.y += 1;

                ret = UpComprovation(sprite_top_right, sprite_top_left, tile_top_right_up, tile_top_left_up);
                break;
            case Direction.DOWN:
                Vector3Int tile_bottom_right_down = Vector3Int.zero;
                tile_bottom_right_down = walkability.LocalToCell(sprite_bottom_right);
                tile_bottom_right_down.y -= 1;

                Vector3Int tile_bottom_left_down = Vector3Int.zero;
                tile_bottom_left_down = walkability.LocalToCell(sprite_bottom_left);
                tile_bottom_left_down.y -= 1;

                ret = DownComprovation(sprite_bottom_right, sprite_bottom_left, tile_bottom_right_down, tile_bottom_left_down);
                break;

        }

       

        return ret;
    }

    bool RightComprovation(Vector3 sprite_top_right, Vector3 sprite_bottom_right , Vector3Int tile_top_right_pos, Vector3Int tile_bottom_right_pos)
    {
        bool ret = false;
        bool check1_top = false;
        bool check2_bottom = false;

        TileBase tile_top_rigt = walkability.GetTile(tile_top_right_pos);
        TileBase tile_bottom_rigt = walkability.GetTile(tile_bottom_right_pos);

        if (tile_top_rigt == walkable_tile)
        {
            check1_top = true;

        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_top_right_pos);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_top_right.x < tile_x_comprovation)
            {
                check1_top = true;
            }

        }

        if(tile_bottom_rigt == walkable_tile)
        {
            check2_bottom = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_bottom_right_pos);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_bottom_right.x < tile_x_comprovation)
            {
                check2_bottom = true;
            }
        }

        if(check1_top ==true && check2_bottom == true)
        {
            ret = true;
        }


        return ret;
    }

    bool LeftComprovation(Vector3 sprite_top_left, Vector3 sprite_bottom_left, Vector3Int tile_top_left_pos, Vector3Int tile_bottom_leftt_pos)
    {
        bool ret = false;
        bool check1_top = false;
        bool check2_bottom = false;

        TileBase tile_top_left = walkability.GetTile(tile_top_left_pos);
        TileBase tile_bottom_left = walkability.GetTile(tile_bottom_leftt_pos);

        if (tile_top_left == walkable_tile)
        {
            check1_top = true;

        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_top_left_pos);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_top_left.x > tile_x_comprovation)
            {
                check1_top = true;
            }

        }

        if (tile_bottom_left == walkable_tile)
        {
            check2_bottom = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_bottom_leftt_pos);

            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x + (1.5f / 2);


            if (sprite_bottom_left.x > tile_x_comprovation)
            {
                check2_bottom = true;
            }
        }

        if (check1_top == true && check2_bottom == true)
        {
            ret = true;
        }


        return ret;
    }

    bool UpComprovation(Vector3 sprite_top_right, Vector3 sprite_top_left, Vector3Int tile_top_right_pos, Vector3Int tile_top_left_pos)
    {
        bool ret = false;
        bool check1_top = false;
        bool check2_bottom = false;

        TileBase tile_top_right = walkability.GetTile(tile_top_right_pos);
        TileBase tile_top_left = walkability.GetTile(tile_top_left_pos);

        if (tile_top_right == walkable_tile)
        {
            check1_top = true;

        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_top_right_pos);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_right.y < tile_y_comprovation)
            {
                check1_top = true;
            }

        }

        if (tile_top_left == walkable_tile)
        {
            check2_bottom = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_top_left_pos);

            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y - (1.5f / 2);


            if (sprite_top_left.y < tile_y_comprovation)
            {
                check2_bottom = true;
            }
        }

        if (check1_top == true && check2_bottom == true)
        {
            ret = true;
        }


        return ret;
    }

    bool DownComprovation(Vector3 sprite_bottom_right, Vector3 sprite_bottom_left, Vector3Int tile_bottom_right_pos, Vector3Int tile_bottom_left_pos)
    {
        bool ret = false;
        bool check1_top = false;
        bool check2_bottom = false;

        TileBase tile_bottom_right = walkability.GetTile(tile_bottom_right_pos);
        TileBase tile_bottom_left = walkability.GetTile(tile_bottom_left_pos);

        if (tile_bottom_right == walkable_tile)
        {
            check1_top = true;

        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_bottom_right_pos);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_bottom_right.y > tile_y_comprovation)
            {
                check1_top = true;
            }

        }

        if (tile_bottom_left == walkable_tile)
        {
            check2_bottom = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_bottom_left_pos);

            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);


            if (sprite_bottom_left.y > tile_y_comprovation)
            {
                check2_bottom = true;
            }
        }

        if (check1_top == true && check2_bottom == true)
        {
            ret = true;
        }


        return ret;
    }

    private bool CanIWalkInDiagonals(Vector3 sprite_top_left, Vector3 sprite_top_right, Vector3 sprite_bottom_right, Vector3 sprite_bottom_left, Direction dir)
    {
        bool ret = false;
        Vector3Int tile_diagonal = Vector3Int.zero;
        Vector3Int tile_right = Vector3Int.zero;
        Vector3Int tile_left = Vector3Int.zero;
        Vector3Int tile_top = Vector3Int.zero;
        Vector3Int tile_down = Vector3Int.zero;

        switch (dir)
        {
            case Direction.UP_RIGHT:
                
                tile_diagonal = walkability.LocalToCell(transform.position);
                tile_diagonal.x += 1;
                tile_diagonal.y += 1;

                tile_right = walkability.LocalToCell(transform.position);
                tile_right.x += 1;

                tile_top = walkability.LocalToCell(transform.position);
                tile_top.y += 1;

                ret = UpRightComprovation(sprite_top_right, sprite_bottom_right, sprite_top_left,
                    tile_diagonal, tile_right, tile_top);

                break;
            case Direction.UP_LEFT:
                tile_diagonal = walkability.LocalToCell(transform.position);
                tile_diagonal.x -= 1;
                tile_diagonal.y += 1;

                tile_left = walkability.LocalToCell(transform.position);
                tile_left.x -= 1;


                tile_top = walkability.LocalToCell(transform.position);
                tile_top.y += 1;

                ret = UpLeftComprovation(sprite_top_left, sprite_bottom_left, sprite_top_right, tile_diagonal,
                    tile_left, tile_top);

                break;
            case Direction.DOWN_RIGHT:

                tile_diagonal = walkability.LocalToCell(transform.position);
                tile_diagonal.x += 1;
                tile_diagonal.y -= 1;

                tile_right = walkability.LocalToCell(transform.position);
                tile_right.x += 1;


                tile_down = walkability.LocalToCell(transform.position);
                tile_down.y -= 1;

                ret = DownRightComprovation(sprite_bottom_right, sprite_top_right, sprite_bottom_left, tile_diagonal,
                    tile_right, tile_down);

                break;
            case Direction.DOWN_LEFT:

                tile_diagonal = walkability.LocalToCell(transform.position);
                tile_diagonal.x -= 1;
                tile_diagonal.y -= 1;

                tile_left = walkability.LocalToCell(transform.position);
                tile_left.x -= 1;


                tile_down = walkability.LocalToCell(transform.position);
                tile_down.y -= 1;

                ret = DownLeftComprovation(sprite_bottom_left, sprite_top_left, sprite_bottom_right, tile_diagonal,
                    tile_left, tile_down);


                break;

        }

        return ret;
    }

    bool UpRightComprovation(Vector3 sprite_top_right, Vector3 sprite_bottom_right, Vector3 sprite_top_left, Vector3Int tile_diagonal,
        Vector3Int tile_right, Vector3Int tile_top)
    {
        bool ret = false;
        bool check1_top_r = false;
        bool check2_bottom = false;
        bool check3_top_l = false;

        TileBase tile_diagonal_base = walkability.GetTile(tile_diagonal);
        TileBase tile_right_base = walkability.GetTile(tile_right);
        TileBase tile_top_base = walkability.GetTile(tile_top);

        if (tile_diagonal_base == walkable_tile)
        {
            check1_top_r = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_right.y < tile_y_comprovation)
            {
                check1 = true;
            }

            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_top_right.x < tile_x_comprovation)
            {
                check2 = true;
            }

            if(check1 ==  true && check2 == true)
            {
                check1_top_r = true;
            }

        }

        if (tile_right_base == walkable_tile)
        {
            check2_bottom = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_right);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_bottom_right.x < tile_x_comprovation)
            {
                check2_bottom = true;
            }
        }

        if (tile_top_base == walkable_tile)
        {
            check3_top_l = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_top);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_left.y < tile_y_comprovation)
            {
                check3_top_l = true;
            }
        }

        if (check1_top_r == true && check2_bottom == true && check3_top_l == true)
        {
            ret = true;
        }

        return ret;
    }

    bool UpLeftComprovation(Vector3 sprite_top_left,  Vector3 sprite_bottom_left, Vector3 sprite_top_right, Vector3Int tile_diagonal,
        Vector3Int tile_left, Vector3Int tile_top)
    {
        bool ret = false;
        bool check1_top_r = false;
        bool check2_bottom = false;
        bool check3_top_l = false;

        TileBase tile_diagonal_base = walkability.GetTile(tile_diagonal);
        TileBase tile_left_base = walkability.GetTile(tile_left);
        TileBase tile_top_base = walkability.GetTile(tile_top);

        if (tile_diagonal_base == walkable_tile)
        {
            check1_top_r = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_right.y < tile_y_comprovation)
            {
                check1 = true;
            }

            float tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_top_right.x > tile_x_comprovation)
            {
                check2 = true;
            }

            if (check1 == true && check2 == true)
            {
                check1_top_r = true;
            }
        }

        if (tile_left_base == walkable_tile)
        {
            check2_bottom = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_left);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_bottom_right.x > tile_x_comprovation)
            {
                check2_bottom = true;
            }
        }

        if (tile_top_base == walkable_tile)
        {
            check3_top_l = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_top);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_left.y < tile_y_comprovation)
            {
                check3_top_l = true;
            }
        }

        if (check1_top_r == true && check2_bottom == true && check3_top_l == true)
        {
            ret = true;
        }

        return ret;
    }

    bool DownRightComprovation(Vector3 sprite_bottom_right, Vector3 sprite_top_right, Vector3 sprite_bottom_left, Vector3Int tile_diagonal,
       Vector3Int tile_right, Vector3Int tile_down)
    {
        bool ret = false;
        bool check1_top_r = false;
        bool check2_bottom = false;
        bool check3_top_l = false;

        TileBase tile_diagonal_base = walkability.GetTile(tile_diagonal);
        TileBase tile_right_base = walkability.GetTile(tile_right);
        TileBase tile_down_base = walkability.GetTile(tile_down);

        if (tile_diagonal_base == walkable_tile)
        {
            check1_top_r = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_top_right.y > tile_y_comprovation)
            {
                check1 = true;
            }

            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_top_right.x < tile_x_comprovation)
            {
                check2 = true;
            }

            if (check1 == true && check2 == true)
            {
                check1_top_r = true;
            }
        }

        if (tile_right_base == walkable_tile)
        {
            check2_bottom = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_right);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_bottom_right.x < tile_x_comprovation)
            {
                check2_bottom = true;
            }
        }

        if (tile_down_base == walkable_tile)
        {
            check3_top_l = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_down);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_top_left.y > tile_y_comprovation)
            {
                check3_top_l = true;
            }
        }

        if (check1_top_r == true && check2_bottom == true && check3_top_l == true)
        {
            ret = true;
        }

        return ret;
    }

    bool DownLeftComprovation(Vector3 sprite_bottom_left, Vector3 sprite_top_left, Vector3 sprite_bottom_right, Vector3Int tile_diagonal,
      Vector3Int tile_left, Vector3Int tile_down)
    {
        bool ret = false;
        bool check1_top_r = false;
        bool check2_bottom = false;
        bool check3_top_l = false;

        TileBase tile_diagonal_base = walkability.GetTile(tile_diagonal);
        TileBase tile_left_base = walkability.GetTile(tile_left);
        TileBase tile_down_base = walkability.GetTile(tile_down);

        if (tile_diagonal_base == walkable_tile)
        {
            check1_top_r = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_bottom_left.y > tile_y_comprovation)
            {
                check1 = true;
            }

            float tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_bottom_left.x > tile_x_comprovation)
            {
                check2 = true;
            }

            if (check1 == true && check2 == true)
            {
                check1_top_r = true;
            }
        }

        if (tile_left_base == walkable_tile)
        {
            check2_bottom = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_left);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_top_left.x > tile_x_comprovation)
            {
                check2_bottom = true;
            }
        }

        if (tile_down_base == walkable_tile)
        {
            check3_top_l = true;
        }
        else
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_down);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_bottom_right.y > tile_y_comprovation)
            {
                check3_top_l = true;
            }
        }

        if (check1_top_r == true && check2_bottom == true && check3_top_l == true)
        {
            ret = true;
        }

        return ret;
    }

}
