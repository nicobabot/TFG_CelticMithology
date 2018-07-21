using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 0.0f;
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

    enum Comprobation
    {
        BIGGER,
        SMALLER
    }


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

    }

    Vector3 DetectDirection()
    {
        moving_in_diagonals = false;
        Vector3 temp_pos = transform.position;

        if (Input.GetKey(KeyCode.D))
        {
            temp_pos.x += Time.deltaTime * speed;
            current_direction = Direction.RIGHT;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            temp_pos.x -= Time.deltaTime * speed;
            current_direction = Direction.LEFT;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            temp_pos.y += Time.deltaTime * speed;
            current_direction = Direction.UP;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            temp_pos.y -= Time.deltaTime * speed;
            current_direction = Direction.DOWN;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            moving_in_diagonals = true;
            temp_pos.y += Time.deltaTime * speed;
            temp_pos.x += Time.deltaTime * speed;
            current_direction = Direction.UP_RIGHT;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            moving_in_diagonals = true;
            temp_pos.y += Time.deltaTime * speed;
            temp_pos.x -= Time.deltaTime * speed;
            current_direction = Direction.UP_LEFT;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            moving_in_diagonals = true;
            temp_pos.y -= Time.deltaTime * speed;
            temp_pos.x += Time.deltaTime * speed;
            current_direction = Direction.DOWN_RIGHT;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
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

}
