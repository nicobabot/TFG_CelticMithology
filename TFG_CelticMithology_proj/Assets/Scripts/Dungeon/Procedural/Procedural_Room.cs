using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ExitDirectionPoint
{
    public ProceduralDungeonGenerator.ExitDirection dir;
    public Vector3Int nextRoomPos;
}

public class Procedural_Room
{
    public ExitDirectionPoint[] exits;

    private int _x_pos;
    private int _y_pos;
    private int _tilewidth;
    private int _tileheight;

    private Tilemap grid;

    private ProceduralDungeonGenerator.TileType[][] room;

    private GameObject Room_Go;

    private int numExits = 0;

    public Procedural_Room(int new_x_pos, int new_y_pos, int new_tilewidth, int new_tileheight, int num_room)
    {
        _x_pos = new_x_pos;
        _y_pos = new_y_pos;
        _tilewidth = new_tilewidth;
        _tileheight = new_tileheight;

        Room_Go = new GameObject("Room " + num_room.ToString());

        grid = ProceduralDungeonGenerator.mapGenerator.grid;

        //numExits = Random.Range(1, 5);

        numExits = 1;

        SetExits();

        room = new ProceduralDungeonGenerator.TileType[_tileheight][];

        for (int i = 0; i < _tilewidth; i++)
        {
            room[i] = new ProceduralDungeonGenerator.TileType[_tilewidth];
        }

        SetGroundAndWall();
        SetColliders();
    }

    void SetGroundAndWall()
    {

        for (int j = 0; j < _tileheight; j++)
        {
            for (int i = 0; i < _tilewidth; i++)
            {

                DetectIfIsWall(i,j);

            }
        }
    }

    void DetectIfIsWall(int i, int j)
    {
        if(i == 0)
        {
            if (j == 0)
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_UP_CORNER;
            }
            else if (j == (_tileheight - 1))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_UP_CORNER;
            }
            else {
                room[i][j] = ProceduralDungeonGenerator.TileType.UP_WALL;
            }
        } 
        else if (i == (_tilewidth-1))
        {
            if (j == 0)
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_DOWN_CORNER;
            }
            else if (j == (_tileheight - 1))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_DOWN_CORNER;
            }
            else
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.DOWN_WALL;
            }
        }
        else if (j == 0)
        {
            if(i != 0 && i != (_tilewidth - 1))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.DOWN_WALL;
            }
        }
        else if(j == (_tileheight - 1))
        {
            if(i != 0 && i != (_tilewidth - 1))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_WALL;
            }
        }
        else
        {
            room[i][j] = ProceduralDungeonGenerator.TileType.FLOOR;
        }
    }

    void SetColliders()
    {
        //Left wall collider
        GameObject collider_left_go = new GameObject();
        collider_left_go.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight*0.5f)-0.5f));
        collider_left_go.transform.SetParent(Room_Go.transform);

        BoxCollider2D left_collider = collider_left_go.AddComponent<BoxCollider2D>();
        left_collider.size = new Vector2(1,_tileheight);


        //Right wall collider
        GameObject collider_right_go = new GameObject();
        collider_right_go.transform.position = new Vector3(_x_pos + (_tilewidth-1), _y_pos + ((_tileheight * 0.5f) - 0.5f));
        collider_right_go.transform.SetParent(Room_Go.transform);

        BoxCollider2D right_collider = collider_right_go.AddComponent<BoxCollider2D>();
        right_collider.size = new Vector2(1, _tileheight);

        //Up wall collider
        GameObject collider_up_go = new GameObject();
        collider_up_go.transform.position = new Vector3(_x_pos + ((_tileheight * 0.5f) - 0.5f), _y_pos + (_tileheight-1));
        collider_up_go.transform.SetParent(Room_Go.transform);

        BoxCollider2D up_collider = collider_up_go.AddComponent<BoxCollider2D>();
        up_collider.size = new Vector2(_tilewidth, 1);

        //Down wall collider
        GameObject collider_down_go = new GameObject();
        collider_down_go.transform.position = new Vector3(_x_pos + ((_tileheight * 0.5f) - 0.5f), _y_pos);
        collider_down_go.transform.SetParent(Room_Go.transform);

        BoxCollider2D down_collider = collider_down_go.AddComponent<BoxCollider2D>();
        down_collider.size = new Vector2(_tilewidth, 1);

    }

    void SetExits(bool test = false)
    {

         exits = new ExitDirectionPoint[numExits];

        for (int i = 0; i < exits.Length; i++)
        {
            exits[i] = new ExitDirectionPoint();
            do {
                exits[i].dir = (ProceduralDungeonGenerator.ExitDirection)Random.Range(0, 4);

                switch (exits[i].dir)
                {
                    case ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT:
                        exits[i].nextRoomPos.x = _x_pos + _tilewidth + 1;
                        exits[i].nextRoomPos.y = _y_pos;
                        break;
                    case ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT:
                        exits[i].nextRoomPos.x = _x_pos - _tilewidth - 1;
                        exits[i].nextRoomPos.y = _y_pos;
                        break;
                    case ProceduralDungeonGenerator.ExitDirection.UP_EXIT:
                        exits[i].nextRoomPos.x = _x_pos;
                        exits[i].nextRoomPos.y = _y_pos + _tileheight + 1;
                        break;
                    case ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT:
                        exits[i].nextRoomPos.x = _x_pos;
                        exits[i].nextRoomPos.y = _y_pos - _tileheight - 1;
                        break;
                }
            } while (ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(grid.CellToWorld(exits[i].nextRoomPos)));
            

        }

    }

    public void DrawRoom()
    {

        GameObject temp = null;

        for (int i=0; i< _tilewidth; i++)
        {
            for(int j=0; j< _tileheight; j++)
            {

                Vector3 tile_pos = grid.CellToWorld(new Vector3Int(_x_pos + i, _y_pos + j, 0));


                switch (room[i][j])
                {
                    case ProceduralDungeonGenerator.TileType.FLOOR:
                        temp = ProceduralDungeonGenerator.mapGenerator.InstantiateWithTile(false,false, Room_Go.transform);
                        temp.transform.position = tile_pos;
                        break;
                    case ProceduralDungeonGenerator.TileType.LEFT_DOWN_CORNER:
                    case ProceduralDungeonGenerator.TileType.LEFT_UP_CORNER:
                    case ProceduralDungeonGenerator.TileType.RIGHT_DOWN_CORNER:
                    case ProceduralDungeonGenerator.TileType.RIGHT_UP_CORNER:
                        temp = ProceduralDungeonGenerator.mapGenerator.InstantiateWithTile(false, true, Room_Go.transform);
                        temp.transform.position = tile_pos;
                        break;
                    default:
                        temp = ProceduralDungeonGenerator.mapGenerator.InstantiateWithTile(true, false, Room_Go.transform);
                        temp.transform.position = tile_pos;
                        break;
                }
            }
        }
    }

    public bool IsInsideRoom(Vector3 point)
    {
        bool ret = false;
        
        if (point.x >= _x_pos && point.x < _x_pos + _tilewidth && point.y >= _y_pos && point.y < _y_pos + _tilewidth)
        {
            ret = true;
        }

        return ret;
    }


}
