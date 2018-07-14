using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 0.0f;
    public Tilemap walkability;
    public Tile walkable_tile;
    public Tile non_walkable_tile;
    SpriteRenderer sprite_rend;
    Sprite player_sprite;

    enum Direction
    {
        RIGHT,
        LEFT,
        DOWN,
        UP
    }
    Direction current_direction;


    // Use this for initialization
    void Start () {
        sprite_rend = GetComponent<SpriteRenderer>();
        player_sprite = sprite_rend.sprite;
    }
	
	// Update is called once per frame
	void Update () {

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
        else if(Input.GetKey(KeyCode.W))
        {
            temp_pos.y += Time.deltaTime * speed;
            current_direction = Direction.UP;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            temp_pos.y -= Time.deltaTime * speed;
            current_direction = Direction.DOWN;
        }

        //top left
        Vector3 sprite_point_top_left = temp_pos + player_sprite.bounds.min;
        Vector3 sprite_point_bottom_right = temp_pos + player_sprite.bounds.max;

        float width = player_sprite.bounds.max.x;
        float height = player_sprite.bounds.max.y;

        if (CanIWalk(temp_pos, sprite_point_top_left, width, height, current_direction))
        { 
            transform.position = temp_pos;
        }

    }

    private bool CanIWalk(Vector3 position, Vector3 top_left_position, float width, float height, Direction dir)
    {
        bool ret = false;

        Vector3Int tile_in_position = Vector3Int.zero;

        switch (dir)
        {
            case Direction.RIGHT:
                tile_in_position = walkability.LocalToCell(position);
                tile_in_position.x += 1;
                break;
            case Direction.LEFT:
                tile_in_position = walkability.LocalToCell(position);
                tile_in_position.x -= 1;
                break;
            case Direction.UP:
                tile_in_position = walkability.LocalToCell(position);
                tile_in_position.y += 1;
                break;
            case Direction.DOWN:
                tile_in_position = walkability.LocalToCell(position);
                tile_in_position.y -= 1;
                break;

        }

        TileBase tile_in = walkability.GetTile(tile_in_position);
        if(tile_in == walkable_tile)
        {
            ret = true;
        }
        else if(tile_in == non_walkable_tile)
        {
            if (dir == Direction.RIGHT)
            {
                Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_in_position);
                //Need to find tile width and height units without magic number
                float tile_x_comprovation = world_position_tile.x - (1.5f / 2);
                
                if (top_left_position.x + width < tile_x_comprovation)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            else if(dir == Direction.LEFT)
            {
                Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_in_position);
                float tile_x_comprovation = Mathf.Abs(world_position_tile.x) + (1.5f / 2);
                
                if (Mathf.Abs(top_left_position.x) < tile_x_comprovation)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            else if (dir == Direction.UP)
            {
                Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_in_position);
                float tile_y_comprovation = world_position_tile.y - walkability.size.y * 0.5f;
                if (top_left_position.y > tile_y_comprovation)
                {
                    ret = false;
                }
                else
                {
                    ret = true;
                }
            }
            else if (dir == Direction.DOWN)
            {
                Vector3 world_position_tile = walkability.GetCellCenterWorld(tile_in_position);
                float tile_y_comprovation = world_position_tile.y + walkability.size.y * 0.5f;
                if (top_left_position.y + height< tile_y_comprovation)
                {
                    ret = false;
                }
                else
                {
                    ret = true;
                }
            }

        }

        return ret;
    }

}
