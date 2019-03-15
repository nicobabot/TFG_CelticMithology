using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ExitDirectionPoint
{
    public bool needRoomCreation = true;
    public ProceduralDungeonGenerator.ExitDirection dir;
    public Vector3Int nextRoomPos;
}

public class Procedural_Room
{
    public List<ExitDirectionPoint> usableExits;
    public bool wantToDraw = true;
    private List<ExitDirectionPoint> exits;

    private int _x_pos;
    private int _y_pos;
    private int _tilewidth;
    private int _tileheight;

    private Tilemap grid;

    private ProceduralDungeonGenerator.TileType[][] room;

    private bool hasFirstExit = false;

    private GameObject Room_Go;

    private int numExits = 0;

    public Procedural_Room(int new_x_pos, int new_y_pos, int new_tilewidth, int new_tileheight, int num_room,
        ProceduralDungeonGenerator.ExitDirection dir, int levelDepth)
    {
        _x_pos = new_x_pos;
        _y_pos = new_y_pos;
        _tilewidth = new_tilewidth;
        _tileheight = new_tileheight;

        Room_Go = new GameObject("Room " + num_room.ToString());

        grid = ProceduralDungeonGenerator.mapGenerator.grid;

        exits = new List<ExitDirectionPoint>();
        usableExits = new List<ExitDirectionPoint>();

        //usableExits
        if (dir != ProceduralDungeonGenerator.ExitDirection.NONE_DIR)
        {
            hasFirstExit = true;
            PosibleExits(dir);
        }
        PosibleExits();

        //if (exits.Count != 0) {
        //    numExits = Random.Range(1, exits.Count);
        //} else numExits = 0;

        //Need an algorithm to see how much depth will rest to think about the exits;
        int tempExitsNum = exits.Count - 1;
        if (levelDepth + tempExitsNum <= ProceduralDungeonGenerator.mapGenerator.realDepth)
        {
            numExits = tempExitsNum;
        }
        else
        {
            do
            {
                tempExitsNum -= 1;
            } while (levelDepth + tempExitsNum > ProceduralDungeonGenerator.mapGenerator.realDepth && tempExitsNum > 0);
            numExits = tempExitsNum;
        }

        if (tempExitsNum != 0)
        {

            SetExits();

            room = new ProceduralDungeonGenerator.TileType[_tilewidth][];

            for (int i = 0; i < _tilewidth; i++)
            {
                room[i] = new ProceduralDungeonGenerator.TileType[_tileheight];
            }

            //if (numExits < 1)
            //{
            //    ProceduralDungeonGenerator.Destroy(Room_Go);
            //    wantToDraw = false;
            //}

            SetGroundAndWall();
            SetColliders();
        }
        else
        {
            wantToDraw = false;
        }

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
        AddLeftColliders();

        //Right wall collider
        AddRightColliders();

        //Up wall collider
        AddUpColliders();

        //Down wall collider
        AddDownColliders();

    }

    void SetExits(bool test = false)
    {

        if (numExits > 0 && hasFirstExit == true)
        {
            usableExits.Add(exits[0]);
            numExits--;
        }

        ProceduralDungeonGenerator.ExitDirection new_dir = new ProceduralDungeonGenerator.ExitDirection();

        for (int i = 0; i < numExits; i++)
        {
            ExitDirectionPoint temp = new ExitDirectionPoint();
            do
            {
                new_dir = (ProceduralDungeonGenerator.ExitDirection)Random.Range(0, 4);

            } while (IsAlreadyExitDirection(new_dir) == true);


            temp = GetExitInfo(new_dir);
            if(temp!=null)
            usableExits.Add(temp);
        }
    }



    void PosibleExits(ProceduralDungeonGenerator.ExitDirection dir = ProceduralDungeonGenerator.ExitDirection.NONE_DIR)
    {
        if (dir== ProceduralDungeonGenerator.ExitDirection.NONE_DIR)
        {
            if (!IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT))
            {
                ComprovationRight();
            }
            if (!IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT))
            {
                ComprovationLeft();
            }
            if (!IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT))
            {
                ComprovationDown();
            }
            if (!IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.UP_EXIT))
            {
                ComprovationUp();
            }
        }
        else
        {
            switch (dir)
            {
                case ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT:
                    ComprovationRight(true);
                    break;
                case ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT:
                    ComprovationLeft(true);
                    break;
                case ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT:
                    ComprovationDown(true);
                    break;
                case ProceduralDungeonGenerator.ExitDirection.UP_EXIT:
                    ComprovationUp(true);
                    break;
            }
        }

    }

    void ComprovationRight(bool noNeedComp = false)
    {
        Vector3Int rightExit = new Vector3Int(_x_pos + _tilewidth, _y_pos, 0);
        if (!ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(grid.CellToWorld(rightExit)) || noNeedComp)
        {
            ExitDirectionPoint temp_right = new ExitDirectionPoint();
            temp_right.dir = ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT;
            temp_right.nextRoomPos = rightExit;
            temp_right.needRoomCreation = !noNeedComp;
            exits.Add(temp_right);
        }
    }
    void ComprovationLeft(bool noNeedComp = false)
    {
        Vector3Int leftExit = new Vector3Int(_x_pos - _tilewidth, _y_pos, 0);
        if (!ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(grid.CellToWorld(leftExit)) || noNeedComp)
        {
            ExitDirectionPoint temp_left = new ExitDirectionPoint();
            temp_left.dir = ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT;
            temp_left.nextRoomPos = leftExit;
            temp_left.needRoomCreation = !noNeedComp;
            exits.Add(temp_left);
        }
    }
    void ComprovationUp(bool noNeedComp = false)
    {
        Vector3Int upExit = new Vector3Int(_x_pos, _y_pos + _tileheight, 0);
        if (!ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(grid.CellToWorld(upExit)) || noNeedComp)
        {
            ExitDirectionPoint temp_up = new ExitDirectionPoint();
            temp_up.dir = ProceduralDungeonGenerator.ExitDirection.UP_EXIT;
            temp_up.nextRoomPos = upExit;
            temp_up.needRoomCreation = !noNeedComp;
            exits.Add(temp_up);
        }
    }
    void ComprovationDown(bool noNeedComp = false)
    {
        Vector3Int downExit = new Vector3Int(_x_pos, _y_pos - _tileheight, 0);
        if (!ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(grid.CellToWorld(downExit)) || noNeedComp)
        {
            ExitDirectionPoint temp_down = new ExitDirectionPoint();
            temp_down.dir = ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT;
            temp_down.nextRoomPos = downExit;
            temp_down.needRoomCreation = !noNeedComp;
            exits.Add(temp_down);
        }
    }

    void AddRightColliders()
    {
        if (!IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT))
        {
            GameObject collider_right_go = new GameObject();
            collider_right_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1), _y_pos + ((_tileheight * 0.5f) - 0.5f));
            collider_right_go.transform.SetParent(Room_Go.transform);

            BoxCollider2D right_collider = collider_right_go.AddComponent<BoxCollider2D>();
            right_collider.size = new Vector2(1, _tileheight);
        }
        else
        {
            GameObject collider_right_go_1 = new GameObject();
            collider_right_go_1.transform.position = new Vector3(_x_pos + (_tilewidth - 1), _y_pos + ((_tileheight / 5) - 0.5f));
            collider_right_go_1.transform.SetParent(Room_Go.transform);

            BoxCollider2D right_collider1 = collider_right_go_1.AddComponent<BoxCollider2D>();
            right_collider1.size = new Vector2(1, (_tileheight / 5) * 2);

            GameObject collider_right_go_2 = new GameObject();
            collider_right_go_2.transform.position = new Vector3(_x_pos + (_tilewidth - 1), _y_pos + ((_tileheight / 5) * (5 - 1) - 0.5f));
            collider_right_go_2.transform.SetParent(Room_Go.transform);

            BoxCollider2D right_collider2 = collider_right_go_2.AddComponent<BoxCollider2D>();
            right_collider2.size = new Vector2(1, (_tileheight / 5) * 2);
        }
    }
    void AddLeftColliders()
    {
        if (!IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT))
        {
            GameObject collider_left_go = new GameObject();
            collider_left_go.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight * 0.5f) - 0.5f));
            collider_left_go.transform.SetParent(Room_Go.transform);

            BoxCollider2D left_collider = collider_left_go.AddComponent<BoxCollider2D>();
            left_collider.size = new Vector2(1, _tileheight);
        }
        else
        {
            GameObject collider_left_go_1 = new GameObject();
            collider_left_go_1.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight / 5) - 0.5f));
            collider_left_go_1.transform.SetParent(Room_Go.transform);

            BoxCollider2D left_collider1 = collider_left_go_1.AddComponent<BoxCollider2D>();
            left_collider1.size = new Vector2(1, (_tileheight / 5) * 2);

            GameObject collider_left_go_2 = new GameObject();
            collider_left_go_2.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight / 5) * (5 - 1) - 0.5f));
            collider_left_go_2.transform.SetParent(Room_Go.transform);

            BoxCollider2D left_collider2 = collider_left_go_2.AddComponent<BoxCollider2D>();
            left_collider2.size = new Vector2(1, (_tileheight / 5) * 2);
        }
    }
    void AddUpColliders()
    {
        if (!IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.UP_EXIT))
        {
            GameObject collider_up_go = new GameObject();
            collider_up_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1) * 0.5f, _y_pos + (_tileheight - 1));
            collider_up_go.transform.SetParent(Room_Go.transform);

            BoxCollider2D up_collider = collider_up_go.AddComponent<BoxCollider2D>();
            up_collider.size = new Vector2(_tilewidth, 1);
        }
        else
        {
            GameObject collider_up_go_1 = new GameObject();
            //collider_up_go_1.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight / 5) - 0.5f));
            collider_up_go_1.transform.position = new Vector3(_x_pos + (((_tilewidth / 5)) - 0.5f), _y_pos + (_tileheight - 1));
            collider_up_go_1.transform.SetParent(Room_Go.transform);

            BoxCollider2D up_collider1 = collider_up_go_1.AddComponent<BoxCollider2D>();
            up_collider1.size = new Vector2((_tilewidth / 5) * 2, 1);

            GameObject collider_up_go_2 = new GameObject();
            collider_up_go_2.transform.position = new Vector3(_x_pos + ((_tilewidth / 5) * (5 - 1)) - 0.5f, _y_pos + (_tileheight - 1));
            collider_up_go_2.transform.SetParent(Room_Go.transform);

            BoxCollider2D up_collider2 = collider_up_go_2.AddComponent<BoxCollider2D>();
            up_collider2.size = new Vector2((_tilewidth / 5) * 2, 1);
        }
    }
    void AddDownColliders()
    {
        if (!IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT))
        {
            GameObject collider_down_go = new GameObject();
            collider_down_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1) * 0.5f, _y_pos);
            collider_down_go.transform.SetParent(Room_Go.transform);

            BoxCollider2D down_collider = collider_down_go.AddComponent<BoxCollider2D>();
            down_collider.size = new Vector2(_tilewidth, 1);
        }
        else
        {
            GameObject collider_down_go_1 = new GameObject();
            collider_down_go_1.transform.position = new Vector3(_x_pos + (((_tilewidth / 5)) - 0.5f), _y_pos);
            collider_down_go_1.transform.SetParent(Room_Go.transform);

            BoxCollider2D down_collider1 = collider_down_go_1.AddComponent<BoxCollider2D>();
            down_collider1.size = new Vector2((_tilewidth / 5) * 2, 1);

            GameObject collider_down_go_2 = new GameObject();
            collider_down_go_2.transform.position = new Vector3(_x_pos + ((_tilewidth / 5) * (5 - 1)) - 0.5f, _y_pos);
            collider_down_go_2.transform.SetParent(Room_Go.transform);

            BoxCollider2D down_collider2 = collider_down_go_2.AddComponent<BoxCollider2D>();
            down_collider2.size = new Vector2((_tilewidth / 5) * 2, 1);
        }
    }

    bool IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection mydir)
    {
        bool ret = false;
        for (int i = 0; i < usableExits.Count; i++)
        {
            if (usableExits[i] != null)
            {
                if(usableExits[i].dir== mydir)
                {
                    ret = true;
                }
            }

        }

        return ret;
    }

    ExitDirectionPoint GetExitInfo(ProceduralDungeonGenerator.ExitDirection mydir)
    {
        ExitDirectionPoint temp = null;

        for (int i = 0; i < exits.Count; i++)
        {
            if (exits[i] != null)
            {
                if (exits[i].dir == mydir)
                {
                    temp = exits[i];
                }
            }

        }

        return temp;
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

        if (point.x >= _x_pos && point.x <= _x_pos + _tilewidth && point.y >= _y_pos && point.y <= _y_pos + _tileheight)
        {
            ret = true;
        }

        return ret;
    }


}
