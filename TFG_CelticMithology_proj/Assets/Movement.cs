﻿using System.Collections;
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
    bool moving_in_diagonals = false;

    float width = 0.0f;
    float height = 0.0f;

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

        float width = player_sprite.bounds.max.x;
        float height = player_sprite.bounds.max.y;

        sprite_top_left = transform.position + player_sprite.bounds.min;
        sprite_bottom_right = transform.position + player_sprite.bounds.max;

        sprite_top_right = new Vector3(sprite_bottom_right.x, sprite_top_left.y);
        sprite_bottom_left = new Vector3(sprite_top_left.x, sprite_bottom_right.y);

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
                tile_top_right.x -= 1;

                Vector3Int tile_bottom_right = Vector3Int.zero;
                tile_bottom_right = walkability.LocalToCell(sprite_bottom_right);
                tile_bottom_right.x -= 1;

                ret = RightComprovation(sprite_top_right, sprite_bottom_right, tile_top_right, tile_bottom_right);

                break;
            case Direction.LEFT:

                break;
            case Direction.UP:

                break;
            case Direction.DOWN:

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

            if (sprite_top_right.x > tile_x_comprovation)
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

            if (sprite_bottom_right.x > tile_x_comprovation)
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

  

}
