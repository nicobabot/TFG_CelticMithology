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
        DOWN_LEFT,
        NO_MOVING

    }
    Direction current_direction;

    // Use this for initialization
    void Start () {
        sprite_rend = GetComponent<SpriteRenderer>();
        player_sprite = sprite_rend.sprite;
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 temp_pos_transf = Vector3.zero;

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
            Direction can_move_diagonal = CanIWalkInDiagonals(sprite_top_left, sprite_top_right, sprite_bottom_right, sprite_bottom_left, current_direction);

            if (can_move_diagonal != Direction.NO_MOVING)
            {
                if(can_move_diagonal == Direction.UP || can_move_diagonal == Direction.DOWN)
                {
                    temp_pos_transf = transform.position;
                    temp_pos_transf.y = temp_pos.y;
                    transform.position = temp_pos_transf;
                }
                else if (can_move_diagonal == Direction.RIGHT || can_move_diagonal == Direction.LEFT)
                {
                    temp_pos_transf = transform.position;
                    temp_pos_transf.x = temp_pos.x;
                    transform.position = temp_pos_transf;
                }
                else
                {
                    transform.position = temp_pos;
                }

            }
        }

    }

    Vector3 DetectDirection()
    {
        moving_in_diagonals = false;
        Vector3 temp_pos = transform.position;

        float x_movement = Input.GetAxisRaw("Horizontal");
        float y_movement = Input.GetAxisRaw("Vertical");

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
            Vector3Int tile_pos = Vector3Int.zero;
            tile_pos = walkability.LocalToCell(transform.position);
            tile_pos.x += 1;
            TileBase tile_pos_base = walkability.GetTile(tile_pos);
            if (tile_pos_base == walkable_tile)
            {
                ret = true;
            }
           else
            {
                Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_pos);
                //Need to find tile width and height units without magic number
                float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

                if (sprite_bottom_right.x < tile_x_comprovation)
                {
                    ret = true;
                }
            }

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
            Vector3Int tile_pos = Vector3Int.zero;
            tile_pos = walkability.LocalToCell(transform.position);
            tile_pos.x -= 1;
            TileBase tile_pos_base = walkability.GetTile(tile_pos);
            if (tile_pos_base == walkable_tile)
            {
                ret = true;
            }
            else
            {
                Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_pos);
                //Need to find tile width and height units without magic number
                float tile_x_comprovation = world_position_tile.x + (1.5f / 2);

                if (sprite_bottom_right.x > tile_x_comprovation)
                {
                    ret = true;
                }
            }
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
            Vector3Int tile_pos = Vector3Int.zero;
            tile_pos = walkability.LocalToCell(transform.position);
            tile_pos.y += 1;
            TileBase tile_pos_base = walkability.GetTile(tile_pos);
            if (tile_pos_base == walkable_tile)
            {
                ret = true;
            }
            else
            {
                Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_pos);
                //Need to find tile width and height units without magic number
                float tile_y_comprovation = world_position_tile.y - (1.5f / 2);

                if (sprite_top_right.y < tile_y_comprovation)
                {
                    ret = true;
                }
            }
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
            Vector3Int tile_pos = Vector3Int.zero;
            tile_pos = walkability.LocalToCell(transform.position);
            tile_pos.y -= 1;
            TileBase tile_pos_base = walkability.GetTile(tile_pos);
            if (tile_pos_base == walkable_tile)
            {
                ret = true;
            }
            else
            {
                Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_pos);
                //Need to find tile width and height units without magic number
                float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

                if (sprite_top_right.y > tile_y_comprovation)
                {
                    ret = true;
                }
            }
        }


        return ret;
    }

    private Direction CanIWalkInDiagonals(Vector3 sprite_top_left, Vector3 sprite_top_right, Vector3 sprite_bottom_right, Vector3 sprite_bottom_left, Direction dir)
    {
        Direction ret = Direction.NO_MOVING;
        Vector3Int tile_diagonal = Vector3Int.zero;
        Vector3Int tile_right = Vector3Int.zero;
        Vector3Int tile_left = Vector3Int.zero;
        Vector3Int tile_top = Vector3Int.zero;
        Vector3Int tile_down = Vector3Int.zero;
        Vector3Int tile_right_down = Vector3Int.zero;
        Vector3Int tile_right_up = Vector3Int.zero;
        Vector3Int tile_down_left = Vector3Int.zero;
        Vector3Int tile_top_left = Vector3Int.zero;
        Vector3Int tile_left_down = Vector3Int.zero;
        Vector3Int tile_top_right = Vector3Int.zero;
        Vector3Int tile_down_right = Vector3Int.zero;


        switch (dir)
        {
            case Direction.UP_RIGHT:
                
                tile_diagonal = walkability.LocalToCell(transform.position);
                tile_diagonal.x += 1;
                tile_diagonal.y += 1;

                tile_right = walkability.LocalToCell(transform.position);
                tile_right.x += 1;

                tile_right_down = walkability.LocalToCell(transform.position);
                tile_right_down.x += 1;
                tile_right_down.y -= 1;

                tile_top = walkability.LocalToCell(transform.position);
                tile_top.y += 1;

                tile_top_left = walkability.LocalToCell(transform.position);
                tile_top_left.x -= 1;
                tile_top_left.y += 1;


                ret = UpRightComprovation(sprite_top_right, sprite_bottom_right, sprite_top_left,
                    tile_diagonal, tile_right, tile_top, tile_right_down, tile_top_left);

                break;
            case Direction.UP_LEFT:
                tile_diagonal = walkability.LocalToCell(transform.position);
                tile_diagonal.x -= 1;
                tile_diagonal.y += 1;

                tile_left = walkability.LocalToCell(transform.position);
                tile_left.x -= 1;

                tile_left_down = walkability.LocalToCell(transform.position);
                tile_left_down.x -= 1;
                tile_left_down.y -= 1;

                tile_top_right = walkability.LocalToCell(transform.position);
                tile_top_right.x += 1;
                tile_top_right.y += 1;

                tile_top = walkability.LocalToCell(transform.position);
                tile_top.y += 1;

                ret = UpLeftComprovation(sprite_top_left, sprite_bottom_left, sprite_top_right, tile_diagonal,
                    tile_left, tile_top, tile_left_down, tile_top_right);

                break;
            case Direction.DOWN_RIGHT:

                tile_diagonal = walkability.LocalToCell(transform.position);
                tile_diagonal.x += 1;
                tile_diagonal.y -= 1;

                tile_right = walkability.LocalToCell(transform.position);
                tile_right.x += 1;

                tile_right_up = walkability.LocalToCell(transform.position);
                tile_right_up.x += 1;
                tile_right_up.y += 1;

                tile_down = walkability.LocalToCell(transform.position);
                tile_down.y -= 1;

                tile_down_left = walkability.LocalToCell(transform.position);
                tile_down_left.y -= 1;
                tile_down_left.x -= 1;


                ret = DownRightComprovation(sprite_bottom_right, sprite_top_right, sprite_bottom_left, tile_diagonal,
                    tile_right, tile_down, tile_right_up, tile_down_left);

                break;
            case Direction.DOWN_LEFT:

                tile_diagonal = walkability.LocalToCell(transform.position);
                tile_diagonal.x -= 1;
                tile_diagonal.y -= 1;

                tile_left = walkability.LocalToCell(transform.position);
                tile_left.x -= 1;

                tile_top_left = walkability.LocalToCell(transform.position);
                tile_top_left.x -= 1;
                tile_top_left.y += 1;

                tile_down = walkability.LocalToCell(transform.position);
                tile_down.y -= 1;

                tile_down_right = walkability.LocalToCell(transform.position);
                tile_down_right.x += 1;
                tile_down_right.y -= 1;


                ret = DownLeftComprovation(sprite_bottom_left, sprite_top_left, sprite_bottom_right, tile_diagonal,
                    tile_left, tile_down, tile_top_left, tile_down_right);


                break;

        }

        return ret;
    }

    Direction UpRightComprovation(Vector3 sprite_top_right, Vector3 sprite_bottom_right, Vector3 sprite_top_left, Vector3Int tile_diagonal,
        Vector3Int tile_right, Vector3Int tile_top, Vector3Int tile_right_down, Vector3Int tile_top_left)
    {
        Direction ret = Direction.NO_MOVING;
        bool check_diagonal = false;
        bool check_right = false;
        bool check_up = false;

        TileBase tile_diagonal_base = walkability.GetTile(tile_diagonal);
        TileBase tile_right_base = walkability.GetTile(tile_right);
        TileBase tile_top_base = walkability.GetTile(tile_top);

        TileBase tile_right_down_base = walkability.GetTile(tile_right_down);
        TileBase tile_top_left_base = walkability.GetTile(tile_top_left);


        if (tile_diagonal_base == walkable_tile)
        {
            check_diagonal = true;
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
                check_diagonal = true;
            }

        }

        if (tile_right_base == walkable_tile && tile_right_down_base == walkable_tile)
        {
            check_right = true;
        }
        else
        {
            bool check1_r_d = false;
            bool check2_r_d = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_right);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_bottom_right.x < tile_x_comprovation)
            {
                check1_r_d = true;
            }

            world_position_tile = walkability.GetCellCenterWorld(tile_right_down);
            //Need to find tile width and height units without magic number
            tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_bottom_right.x < tile_x_comprovation)
            {
                check2_r_d = true;
            }

            if (check1_r_d && check2_r_d)
            {
                check_right = true;
            }
        }

        if (tile_top_base == walkable_tile && tile_top_left_base == walkable_tile)
        {
            check_up = true;
        }
        else
        {
            bool check1_l_t = false;
            bool check2_l_t = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_top);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_left.y < tile_y_comprovation)
            {
                check1_l_t = true;
            }

            world_position_tile = walkability.GetCellCenterWorld(tile_top_left);
            //Need to find tile width and height units without magic number
            tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_left.y < tile_y_comprovation)
            {
                check2_l_t = true;
            }

            if (check1_l_t && check2_l_t)
            {
                check_up = true;
            }

        }

        if (check_diagonal == true && check_right == true && check_up == true)
        {
            ret = Direction.UP_RIGHT;
        }
        else if (tile_diagonal_base == walkable_tile && tile_right_base == walkable_tile && tile_top_base == walkable_tile
           && tile_right_down_base == non_walkable_tile && tile_top_left_base == non_walkable_tile)
        {
            ret = Direction.UP_RIGHT;
        }
        else if (check_up == true && check_right == false)
        {
            ret = Direction.UP;
        }
        else if(check_up == false && check_right == true)
        {
            ret = Direction.RIGHT;
        }
        else if(check_diagonal == false && check_right == true && check_up == true)
        {

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            float tile_x_comprovation = world_position_tile.x - (1.25f / 2);

            if (sprite_top_right.x < tile_x_comprovation)
            {
                ret = Direction.UP;
            }
            else
            {
                float tile_y_comprovation = world_position_tile.y - (1.25f / 2);
                if (sprite_top_right.y < tile_y_comprovation)
                {
                    ret = Direction.RIGHT;
                }
            }

        }
        return ret;
    }

    Direction UpLeftComprovation(Vector3 sprite_top_left,  Vector3 sprite_bottom_left, Vector3 sprite_top_right, Vector3Int tile_diagonal,
        Vector3Int tile_left, Vector3Int tile_top, Vector3Int tile_left_down, Vector3Int tile_top_right)
    {
        Direction ret = Direction.NO_MOVING;
        bool check_diagonal = false;
        bool check_left = false;
        bool check_up= false;

        TileBase tile_diagonal_base = walkability.GetTile(tile_diagonal);
        TileBase tile_left_base = walkability.GetTile(tile_left);
        TileBase tile_top_base = walkability.GetTile(tile_top);

        TileBase tile_left_down_base = walkability.GetTile(tile_left_down);
        TileBase tile_top_right_base = walkability.GetTile(tile_top_right);

        if (tile_diagonal_base == walkable_tile)
        {
            check_diagonal = true;
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
                check_diagonal = true;
            }
        }

        if (tile_left_base == walkable_tile && tile_left_down_base == walkable_tile)
        {
            check_left = true;
        }
        else
        {

            bool check1_l_d = false;
            bool check2_l_d = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_left);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_bottom_left.x > tile_x_comprovation)
            {
                check1_l_d = true;
            }

            world_position_tile = walkability.GetCellCenterWorld(tile_left_down);
            //Need to find tile width and height units without magic number
            tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_bottom_left.x > tile_x_comprovation)
            {
                check2_l_d = true;
            }

            if (check1_l_d && check2_l_d)
            {
                check_left = true;
            }

        }

        if (tile_top_base == walkable_tile && tile_top_right_base == walkable_tile)
        {
            check_up = true;
        }
        else
        {
            bool check1_r_t = false;
            bool check2_r_t = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_top);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_left.y < tile_y_comprovation)
            {
                check1_r_t = true;
            }

            world_position_tile = walkability.GetCellCenterWorld(tile_top_right);
            //Need to find tile width and height units without magic number
            tile_y_comprovation = world_position_tile.y - (1.5f / 2);

            if (sprite_top_left.y < tile_y_comprovation)
            {
                check2_r_t = true;
            }

            if (check1_r_t && check2_r_t)
            {
                check_up = true;
            }

        }

        if (check_diagonal == true && check_up == true && check_left == true)
        {
            ret = Direction.UP_LEFT;
        }
        else if (tile_diagonal_base == walkable_tile && tile_left_base == walkable_tile && tile_top_base == walkable_tile
        && tile_left_down_base == non_walkable_tile && tile_top_right_base == non_walkable_tile)
        {
            ret = Direction.UP_LEFT;
        }
        else if (check_up == true && check_left == false)
        {
            ret = Direction.UP;
        }
        else if (check_up == false && check_left == true)
        {
            ret = Direction.LEFT;
        }
        else if (check_diagonal == false && check_left == true && check_up == true)
        {

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            float tile_x_comprovation = world_position_tile.x + (1.25f / 2);

            if (sprite_top_left.x > tile_x_comprovation)
            {
                ret = Direction.UP;
            }
            else
            {
                float tile_y_comprovation = world_position_tile.y - (1.25f / 2);
                if (sprite_top_left.y < tile_y_comprovation)
                {
                    ret = Direction.LEFT;
                }
            }

        }

        return ret;
    }

    Direction DownRightComprovation(Vector3 sprite_bottom_right, Vector3 sprite_top_right, Vector3 sprite_bottom_left, Vector3Int tile_diagonal,
       Vector3Int tile_right, Vector3Int tile_down, Vector3Int tile_right_up, Vector3Int tile_down_left)
    {
        Direction ret = Direction.NO_MOVING;
        bool check_diagonal = false;
        bool check_right = false;
        bool check_down = false;

        TileBase tile_diagonal_base = walkability.GetTile(tile_diagonal);
        TileBase tile_right_base = walkability.GetTile(tile_right);
        TileBase tile_down_base = walkability.GetTile(tile_down);

        TileBase tile_right_up_base = walkability.GetTile(tile_right_up);
        TileBase tile_down_left_base = walkability.GetTile(tile_down_left);

        if (tile_diagonal_base == walkable_tile)
        {
            check_diagonal = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_bottom_right.y > tile_y_comprovation)
            {
                check1 = true;
            }

            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_bottom_right.x < tile_x_comprovation)
            {
                check2 = true;
            }

            if (check1 == true && check2 == true)
            {
                check_diagonal = true;
            }
        }

        if (tile_right_base == walkable_tile && tile_right_up_base == walkable_tile)
        {
            check_right = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_right);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_top_right.x < tile_x_comprovation)
            {
                check1 = true;
            }

            world_position_tile = walkability.GetCellCenterWorld(tile_right_up);
            //Need to find tile width and height units without magic number
            tile_x_comprovation = world_position_tile.x - (1.5f / 2);

            if (sprite_top_right.x < tile_x_comprovation)
            {
                check2 = true;
            }

            if(check1 && check2)
            {
                check_right = true;
            }

        }

        if (tile_down_base == walkable_tile && tile_down_left_base == walkable_tile)
        {
            check_down = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_down);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_bottom_left.y > tile_y_comprovation)
            {
                check1 = true;
            }

            world_position_tile = walkability.GetCellCenterWorld(tile_down_left);
            //Need to find tile width and height units without magic number
            tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_bottom_left.y > tile_y_comprovation)
            {
                check1 = true;
            }

            if(check1 == true && check2 == true)
            {
                check_down = true;
            }

        }

        if (check_diagonal == true && check_right == true && check_down == true)
        {
            ret = Direction.DOWN_RIGHT;
        }
        else if (tile_diagonal_base == walkable_tile && tile_right_base == walkable_tile && tile_down_base == walkable_tile 
            && tile_down_left_base == non_walkable_tile && tile_right_up_base == non_walkable_tile)
        {
            ret = Direction.DOWN_RIGHT;
        }
        else if (check_down == true && check_right == false)
        {
            ret = Direction.DOWN;
        }
        else if (check_down == false && check_right == true)
        {
            ret = Direction.RIGHT;
        }
        if (check_diagonal == false && check_right == true && check_down == true)
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            float tile_x_comprovation = world_position_tile.x - (1.25f / 2);

            if (sprite_bottom_right.x < tile_x_comprovation)
            {
                ret = Direction.DOWN;
            }
            else
            {
                float tile_y_comprovation = world_position_tile.y + (1.25f / 2);
                if (sprite_bottom_right.y > tile_y_comprovation)
                {
                    ret = Direction.RIGHT;
                }
            }
        }

        return ret;
    }

    Direction DownLeftComprovation(Vector3 sprite_bottom_left, Vector3 sprite_top_left, Vector3 sprite_bottom_right, Vector3Int tile_diagonal,
      Vector3Int tile_left, Vector3Int tile_down, Vector3Int tile_left_up, Vector3Int tile_down_right)
    {
        Direction ret = Direction.NO_MOVING;
        bool check_diagonal= false;
        bool check_left = false;
        bool check_down = false;

        TileBase tile_diagonal_base = walkability.GetTile(tile_diagonal);
        TileBase tile_left_base = walkability.GetTile(tile_left);
        TileBase tile_down_base = walkability.GetTile(tile_down);

        TileBase tile_left_up_base = walkability.GetTile(tile_left_up);
        TileBase tile_down_right_base = walkability.GetTile(tile_down_right);

        if (tile_diagonal_base == walkable_tile)
        {
            check_diagonal = true;
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
                check_diagonal = true;
            }
        }

        if (tile_left_base == walkable_tile && tile_left_up_base == walkable_tile)
        {
            check_left = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_left);
            //Need to find tile width and height units without magic number
            float tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_top_left.x > tile_x_comprovation)
            {
                check1 = true;
            }

            world_position_tile = walkability.GetCellCenterWorld(tile_left_up);
            //Need to find tile width and height units without magic number
            tile_x_comprovation = world_position_tile.x + (1.5f / 2);

            if (sprite_top_left.x > tile_x_comprovation)
            {
                check2 = true;
            }

            if (check1 && check2)
            {
                check_left = true;
            }

        }

        if (tile_down_base == walkable_tile && tile_down_right_base == walkable_tile)
        {
            check_down = true;
        }
        else
        {
            bool check1 = false;
            bool check2 = false;

            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_down);
            //Need to find tile width and height units without magic number
            float tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_bottom_right.y > tile_y_comprovation)
            {
                check1 = true;
            }

            world_position_tile = walkability.GetCellCenterWorld(tile_down_right);
            //Need to find tile width and height units without magic number
            tile_y_comprovation = world_position_tile.y + (1.5f / 2);

            if (sprite_bottom_right.y > tile_y_comprovation)
            {
                check2 = true;
            }

            if (check1 && check2)
            {
                check_down = true;
            }

        }

        if (check_diagonal == true && check_left == true && check_down == true)
        {
            ret = Direction.DOWN_LEFT;
        }
        else if (tile_diagonal_base == walkable_tile && tile_left_base == walkable_tile && tile_down_base == walkable_tile
            && tile_down_right_base == non_walkable_tile && tile_left_up_base == non_walkable_tile)
        {
            ret = Direction.DOWN_LEFT;
        }
        else if (check_down == true && check_left == false)
        {
            ret = Direction.DOWN;
        }
        else if (check_down == false && check_left == true)
        {
            ret = Direction.LEFT;
        }
        if (check_diagonal == false && check_left == true && check_down == true)
        {
            Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_diagonal);
            float tile_x_comprovation = world_position_tile.x + (1.25f / 2);

            if (sprite_bottom_left.x > tile_x_comprovation)
            {
                ret = Direction.DOWN;
            }
            else
            {
                float tile_y_comprovation = world_position_tile.y + (1.25f / 2);
                if (sprite_bottom_left.y > tile_y_comprovation)
                {
                    ret = Direction.LEFT;
                }
            }
        }


        return ret;
    }

}
