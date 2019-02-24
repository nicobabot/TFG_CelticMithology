using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ExitDirectionPoint
{
    public ProceduralDungeonGenerator.ExitDirection dir;
    public Vector3Int nextRoomPos;
}

public class Procedural_Room : MonoBehaviour
{
    public ExitDirectionPoint[] exits;

    private int _x_pos;
    private int _y_pos;
    private int _tilewidth;
    private int _tileheight;

    private ProceduralDungeonGenerator.TileType[][] room;

    private int numExits = 0;

    public Procedural_Room(int new_x_pos, int new_y_pos, int new_tilewidth, int new_tileheight)
    {
        _x_pos = new_x_pos;
        _y_pos = new_y_pos;
        _tilewidth = new_tilewidth;
        _tileheight = new_tileheight;

        numExits = Random.Range(1, 5);

        SetExits();

        room = new ProceduralDungeonGenerator.TileType[_tileheight][];

        for (int i = 0; i < _tilewidth; i++)
        {
            room[i] = new ProceduralDungeonGenerator.TileType[_tilewidth];
        }

    }

    void SetExits()
    {

        // exits = new ExitDirectionPoint[numExits];

        exits = new ExitDirectionPoint[1];

        for (int i = 0; i < exits.Length; i++)
        {
            // exits[i].dir = (ProceduralDungeonGenerator.ExitDirection)Random.Range(0, 4);

            exits[i] = new ExitDirectionPoint();
            exits[i].dir = ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT;

            switch (exits[i].dir)
            {
                case ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT:
                    exits[i].nextRoomPos.x = _x_pos + _tilewidth + 1;
                    exits[i].nextRoomPos.y = _y_pos;
                    break;
            }

        }

    }

    public void DrawRoom()
    {
        for(int i=0; i< _tilewidth; i++)
        {
            for(int j=0; j< _tileheight; j++)
            {

                Vector3 tile_pos = ProceduralDungeonGenerator.mapGenerator.grid.CellToWorld(new Vector3Int(_x_pos + i, _y_pos + j, 0));


                switch (room[i][j])
                {
                    case ProceduralDungeonGenerator.TileType.LEFT_WALL:
                        GameObject temp = Instantiate(ProceduralDungeonGenerator.mapGenerator.floorTile, ProceduralDungeonGenerator.mapGenerator.transform);
                        temp.transform.position = tile_pos;
                        break;
                }
            }
        }
    }

    bool IsInsideRoom(Vector3 point)
    {
        bool ret = false;

        if (point.x < _x_pos + _tilewidth && point.y < _y_pos + _tilewidth)
        {
            ret = true;
        }

        return ret;
    }


}
