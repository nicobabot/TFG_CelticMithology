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

    private int numExits = 0;

    public Procedural_Room(int new_x_pos, int new_y_pos, int new_tilewidth, int new_tileheight)
    {
        _x_pos = new_x_pos;
        _y_pos = new_y_pos;
        _tilewidth = new_tilewidth;
        _tileheight = new_tileheight;

        grid = ProceduralDungeonGenerator.mapGenerator.grid;

        //numExits = Random.Range(1, 5);

        numExits = 1;

        SetExits();

        room = new ProceduralDungeonGenerator.TileType[_tileheight][];

        for (int i = 0; i < _tilewidth; i++)
        {
            room[i] = new ProceduralDungeonGenerator.TileType[_tilewidth];
        }

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
        for(int i=0; i< _tilewidth; i++)
        {
            for(int j=0; j< _tileheight; j++)
            {

                Vector3 tile_pos = grid.CellToWorld(new Vector3Int(_x_pos + i, _y_pos + j, 0));


                switch (room[i][j])
                {
                    case ProceduralDungeonGenerator.TileType.LEFT_WALL:
                        GameObject temp = ProceduralDungeonGenerator.mapGenerator.InstantiateWithTile();
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
