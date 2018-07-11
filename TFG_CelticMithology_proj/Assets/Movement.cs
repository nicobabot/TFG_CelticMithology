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
        }
        if (Input.GetKey(KeyCode.A))
        {
            temp_pos.x -= Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            temp_pos.y += Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            temp_pos.y -= Time.deltaTime * speed;
        }

        //Vector3 pos = transform.position;
        //top left
        Vector3 sprite_point_top_left = temp_pos + player_sprite.bounds.min;
        // Bottom right
        Vector3 sprite_point_bottom_right= temp_pos + player_sprite.bounds.max;

        //temp_pos.x -= (player_sprite.rect.width/2)/ ;
        //temp_pos.y -= (player_sprite.rect.height/2)/ player_sprite.pixelsPerUnit;

        if (CanIWalk(sprite_point_top_left) && CanIWalk(sprite_point_bottom_right))
        {
            transform.position = temp_pos;
        }

    }

    private bool CanIWalk(Vector3 position)
    {
        bool ret = false;

        Vector3Int tile_in_position = walkability.LocalToCell(position);
        TileBase tile_in = walkability.GetTile(tile_in_position);
        if(tile_in == walkable_tile)
        {
            ret = true;
        }
        else if(tile_in == walkable_tile)
        {
            ret = false;
        }

        return ret;
    }

}
