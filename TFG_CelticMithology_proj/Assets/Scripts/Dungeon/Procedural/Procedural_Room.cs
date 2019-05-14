using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ExitDirectionPoint
{
    public bool needRoomCreation = true;
    public ProceduralDungeonGenerator.ExitDirection dir;
    public Vector3Int nextRoomPos;
    public bool isUsed = false;
}

public class Procedural_Room
{
    public List<ExitDirectionPoint> usableExits;
    public bool wantToDraw = true;
    private List<ExitDirectionPoint> exits;

    public Dictionary<BoxCollider2D, ProceduralDungeonGenerator.ExitDirection> doors;

    private List<ProceduralDungeonGenerator.ExitDirection> colliderFinalColliders;

    private int _x_pos;
    private int _y_pos;
    private int _tilewidth;
    private int _tileheight;

    private Tilemap grid;

    private ProceduralDungeonGenerator.TileType[][] room;

    private bool hasFirstExit = false;

    [HideInInspector]public GameObject Room_Go;
    RunTimeRoomControl controlRoom;

    private int numExits = 0;
    private int countCollidersExits = 0;
    private int _mylevel = 0;
    private int _myNumRoom = 0;

    private bool _wantBoss = false;
    private bool _wantMiniBoss = false;

    public Procedural_Room(int new_x_pos, int new_y_pos, int new_tilewidth, int new_tileheight, int num_room,
        ProceduralDungeonGenerator.ExitDirection dir, int levelDepth, bool newWantBoss = false, bool newWantMiniBoss = false)
    {
        _x_pos = new_x_pos;
        _y_pos = new_y_pos;
        _tilewidth = new_tilewidth;
        _tileheight = new_tileheight;

        _mylevel = levelDepth;
        _myNumRoom = num_room;

        _wantBoss = newWantBoss;
        _wantMiniBoss = newWantMiniBoss;

        Room_Go = new GameObject("Room " + num_room.ToString());

        grid = ProceduralDungeonGenerator.mapGenerator.grid;

        exits = new List<ExitDirectionPoint>();
        usableExits = new List<ExitDirectionPoint>();
        doors = new Dictionary<BoxCollider2D, ProceduralDungeonGenerator.ExitDirection>();
        colliderFinalColliders = new List<ProceduralDungeonGenerator.ExitDirection>();

        if (dir != ProceduralDungeonGenerator.ExitDirection.NONE_DIR)
        {
            hasFirstExit = true;
            PosibleExits(dir);
        }
        PosibleExits();

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

            SetGroundAndWall();

            controlRoom = Room_Go.AddComponent<RunTimeRoomControl>();
        }
        else
        {
            wantToDraw = false;
        }

    }

    public Vector3 GetMiddlePositionRoom()
    {
        return new Vector3(_x_pos + (_tilewidth * 0.5f), (_y_pos + _tileheight * 0.5f), 0.0f);
    }
    public Vector3 GetStarterRoom()
    {
        return new Vector3(_x_pos, _y_pos, 0.0f);
    }
    public int GetLevelDepth()
    {
        return _mylevel;
    }

    public RunTimeRoomControl GetRunTimeControllScr()
    {
        if (controlRoom == null)
        {
            return null;
        }
        return controlRoom;
    }

    public bool Compare(Procedural_Room it)
    {
        bool ret = false;

        if(it._x_pos == _x_pos && it._y_pos == _y_pos && it._tilewidth == _tilewidth && it._tileheight == _tileheight)
        {
            ret = true;
        }

        return ret;
    }

    public void InitializeRoomRunTimeValues()
    {
        if (wantToDraw)
        {
            controlRoom.InitializeRoomValues(ProceduralDungeonGenerator.mapGenerator.DetectPlayer, doors, _x_pos, _y_pos, 
                _tileheight,_tilewidth, _mylevel, _myNumRoom, _wantBoss, _wantMiniBoss);
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


    public void SetDoors()
    {
        for (int j = 0; j < _tileheight; j++)
        {
            for (int i = 0; i < _tilewidth; i++)
            {
                DetectIfIsDoor(i, j);
            }
        }
    }

    void DetectIfIsDoor(int i, int j)
    {
        //IsColliderExit
        if (i == 0)
        {
            if (IsColliderExit(ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT))
            {
                if (j == _tileheight / 2)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_DOOR_1;
                }
                else if (j == (_tileheight / 2) - 1)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_DOOR_2;
                }
                else if (j == (_tileheight / 2) + 1)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_DOOR_PREV_3;
                }
                else if (j == (_tileheight / 2) - 2)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_DOOR_PREV_0;
                }
            }
        }
        else if (i == (_tilewidth - 1))
        {
            if (IsColliderExit(ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT))
            {
                if (j == _tileheight / 2)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_DOOR_1;
                }
                else if (j == (_tileheight / 2) - 1)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_DOOR_2;
                }
                else if (j == (_tileheight / 2) + 1)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_DOOR_PREV_3;
                }
                else if (j == (_tileheight / 2) - 2)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_DOOR_PREV_0;
                }
            }
        }
        else if (j == 0)
        {
            if (IsColliderExit(ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT))
            {
                if (i == _tilewidth / 2)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.UP_DOOR_1;
                }
                else if (i == (_tilewidth / 2) - 1)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.UP_DOOR_2;
                }
                else if (i == (_tilewidth / 2) + 1)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.UP_DOOR_PREV_3;
                }
                else if (i == (_tilewidth / 2) - 2)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.UP_DOOR_PREV_0;
                }
            }
        }
        else if (j == (_tileheight - 1))
        {
            if (IsColliderExit(ProceduralDungeonGenerator.ExitDirection.UP_EXIT))
            {
                if (i == _tilewidth / 2)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.DOWN_DOOR_1;
                }
                else if (i == (_tilewidth / 2) - 1)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.DOWN_DOOR_2;
                }
                else if (i == (_tilewidth / 2) + 1)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.DOWN_DOOR_PREV_3;
                }
                else if (i == (_tilewidth / 2) - 2)
                {
                    room[i][j] = ProceduralDungeonGenerator.TileType.DOWN_DOOR_PREV_0;
                }
            }
        }
    }

    void DetectIfIsWall(int i, int j)
    {
        if(i == 0)
        {
            if (j == 0)
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_DOWN_CORNER;
            }
            else if (j == 1)
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFTDOWN_UP_BORDER;
            }
            else if (j == (_tileheight - 2))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFTUP_DOWN_BORDER;
            }
            else if (j == (_tileheight - 1))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_UP_CORNER;
            }
            else {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFT_WALL;
            }
        } 
        else if (i == (_tilewidth-1))
        {
            if (j == 0)
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_DOWN_CORNER;
            }
            else if (j == 1)
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHTDOWN_UP_BORDER;
            }
            else if (j == (_tileheight - 2))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHTUP_DOWN_BORDER;
            }
            else if (j == (_tileheight - 1))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_UP_CORNER;
            }
            else
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHT_WALL;
            }
        }
        else if (j == 0)
        {
            if (i == 1)
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFTUP_RIGHT_BORDER;
            }
            else if (i == (_tilewidth - 2))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHTUP_LEFT_BORDER;
            }
            else if(i != 0 && i != (_tilewidth - 1))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.DOWN_WALL;
            }
          
        }
        else if(j == (_tileheight - 1))
        {
            if (i == 1)
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.LEFTDOWN_RIGHT_BORDER;
            }
            else if (i == (_tilewidth - 2))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.RIGHTDOWN_LEFT_BORDER;
            }
            else if (i != 0 && i != (_tilewidth - 1))
            {
                room[i][j] = ProceduralDungeonGenerator.TileType.UP_WALL;
            }
        }
        else
        {
            room[i][j] = ProceduralDungeonGenerator.TileType.FLOOR;
        }
    }

    public void SetColliders()
    {
        countCollidersExits = 0;

        //Left wall collider
        AddLeftColliders();

        //Right wall collider
        AddRightColliders();

        //Up wall collider
        AddUpColliders();

        //Down wall collider
        AddDownColliders();

        AddColliderDoors();

    }

    void AddColliderDoors()
    {
        for(int i=0; i< colliderFinalColliders.Count; i++)
        {
            switch (colliderFinalColliders[i])
            {
                case ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT:
                    GameObject collider_right_go = new GameObject("ColliderDoor Right");
                    SetLayerAndTagDoor(collider_right_go);
                    collider_right_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1), _y_pos + ((_tileheight * 0.5f) - 0.5f));
                    collider_right_go.transform.SetParent(Room_Go.transform);

                    BoxCollider2D right_collider = collider_right_go.AddComponent<BoxCollider2D>();
                    right_collider.size = new Vector2(1, _tileheight/5);
                    right_collider.isTrigger = true;
                    doors.Add(right_collider, ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT);
                    break;
                case ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT:
                    GameObject collider_left_go = new GameObject("ColliderDoor Left");
                    SetLayerAndTagDoor(collider_left_go);
                    collider_left_go.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight * 0.5f) - 0.5f));
                    collider_left_go.transform.SetParent(Room_Go.transform);

                    BoxCollider2D left_collider = collider_left_go.AddComponent<BoxCollider2D>();
                    left_collider.size = new Vector2(1, _tileheight/5);
                    left_collider.isTrigger = true;
                    doors.Add(left_collider, ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT);
                    break;
                case ProceduralDungeonGenerator.ExitDirection.UP_EXIT:
                    GameObject collider_up_go = new GameObject("ColliderDoor Up");
                    SetLayerAndTagDoor(collider_up_go);
                    collider_up_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1) * 0.5f, _y_pos + (_tileheight - 1));
                    collider_up_go.transform.SetParent(Room_Go.transform);

                    BoxCollider2D up_collider = collider_up_go.AddComponent<BoxCollider2D>();
                    up_collider.size = new Vector2(_tilewidth/5, 1);
                    up_collider.isTrigger = true;
                    doors.Add(up_collider, ProceduralDungeonGenerator.ExitDirection.UP_EXIT);
                    break;
                case ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT:
                    GameObject collider_down_go = new GameObject("ColliderDoor Down");
                    SetLayerAndTagDoor(collider_down_go);
                    collider_down_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1) * 0.5f, _y_pos);
                    collider_down_go.transform.SetParent(Room_Go.transform);

                    BoxCollider2D down_collider = collider_down_go.AddComponent<BoxCollider2D>();
                    down_collider.size = new Vector2(_tilewidth/5, 1);
                    down_collider.isTrigger = true;
                    doors.Add(down_collider, ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT);
                    break;
            }
        }
    }

    void SetExits(bool test = false)
    {
        int i = 0;
        if (numExits > 0 && hasFirstExit == true)
        {
            usableExits.Add(exits[0]);
            i++;
        }

        ProceduralDungeonGenerator.ExitDirection new_dir = new ProceduralDungeonGenerator.ExitDirection();

        for (; i < numExits; i++)
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
        ExitDirectionPoint exitTemp = GetExitInfoUsable(ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT);
        bool isInsideRoom = false;
        if (exitTemp != null)
        {
            isInsideRoom = ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(exitTemp.nextRoomPos);
        }

        if (IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT) && countCollidersExits < numExits 
            && isInsideRoom && exitTemp.isUsed)
        {
            countCollidersExits++;

            colliderFinalColliders.Add(ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT);

            GameObject collider_right_go_1 = new GameObject();
            SetLayerAndTagWall(collider_right_go_1);
            collider_right_go_1.transform.position = new Vector3(_x_pos + (_tilewidth - 1), _y_pos + ((_tileheight / 5) - 0.5f));
            collider_right_go_1.transform.SetParent(Room_Go.transform);

            BoxCollider2D right_collider1 = collider_right_go_1.AddComponent<BoxCollider2D>();
            right_collider1.size = new Vector2(1, (_tileheight / 5) * 2);


            GameObject collider_right_go_2 = new GameObject();
            SetLayerAndTagWall(collider_right_go_2);
            collider_right_go_2.transform.position = new Vector3(_x_pos + (_tilewidth - 1), _y_pos + ((_tileheight / 5) * (5 - 1) - 0.5f));
            collider_right_go_2.transform.SetParent(Room_Go.transform);

            BoxCollider2D right_collider2 = collider_right_go_2.AddComponent<BoxCollider2D>();
            right_collider2.size = new Vector2(1, (_tileheight / 5) * 2);
        }
        else { 
            GameObject collider_right_go = new GameObject();
            SetLayerAndTagWall(collider_right_go);
            collider_right_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1), _y_pos + ((_tileheight * 0.5f) - 0.5f));
            collider_right_go.transform.SetParent(Room_Go.transform);

            BoxCollider2D right_collider = collider_right_go.AddComponent<BoxCollider2D>();
            right_collider.size = new Vector2(1, _tileheight);
        }
    }
    void AddLeftColliders()
    {

        ExitDirectionPoint exitTemp = GetExitInfoUsable(ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT);
        bool isInsideRoom = false;
        if (exitTemp != null)
        {
            isInsideRoom = ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(exitTemp.nextRoomPos);
        }

        if (IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT) && countCollidersExits < numExits
            && isInsideRoom && exitTemp.isUsed)
        {
            countCollidersExits++;

            colliderFinalColliders.Add(ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT);

            GameObject collider_left_go_1 = new GameObject();
            SetLayerAndTagWall(collider_left_go_1);
            collider_left_go_1.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight / 5) - 0.5f));
            collider_left_go_1.transform.SetParent(Room_Go.transform);

            BoxCollider2D left_collider1 = collider_left_go_1.AddComponent<BoxCollider2D>();
            left_collider1.size = new Vector2(1, (_tileheight / 5) * 2);

            GameObject collider_left_go_2 = new GameObject();
            SetLayerAndTagWall(collider_left_go_2);
            collider_left_go_2.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight / 5) * (5 - 1) - 0.5f));
            collider_left_go_2.transform.SetParent(Room_Go.transform);

            BoxCollider2D left_collider2 = collider_left_go_2.AddComponent<BoxCollider2D>();
            left_collider2.size = new Vector2(1, (_tileheight / 5) * 2);
        }
        else { 
            GameObject collider_left_go = new GameObject();
            SetLayerAndTagWall(collider_left_go);
            collider_left_go.transform.position = new Vector3(_x_pos, _y_pos + ((_tileheight * 0.5f) - 0.5f));
            collider_left_go.transform.SetParent(Room_Go.transform);

            BoxCollider2D left_collider = collider_left_go.AddComponent<BoxCollider2D>();
            left_collider.size = new Vector2(1, _tileheight);
        }
    }
    void AddUpColliders()
    {
        ExitDirectionPoint exitTemp = GetExitInfoUsable(ProceduralDungeonGenerator.ExitDirection.UP_EXIT);
        bool isInsideRoom = false;
        if (exitTemp != null)
        {
            isInsideRoom = ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(exitTemp.nextRoomPos);
        }

        if (IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.UP_EXIT) && countCollidersExits < numExits 
            && isInsideRoom && exitTemp.isUsed)
        {
            countCollidersExits++;

            colliderFinalColliders.Add(ProceduralDungeonGenerator.ExitDirection.UP_EXIT);

            GameObject collider_up_go_1 = new GameObject();
            SetLayerAndTagWall(collider_up_go_1);
            collider_up_go_1.transform.position = new Vector3(_x_pos + (((_tilewidth / 5)) - 0.5f), _y_pos + (_tileheight - 1));
            collider_up_go_1.transform.SetParent(Room_Go.transform);

            BoxCollider2D up_collider1 = collider_up_go_1.AddComponent<BoxCollider2D>();
            up_collider1.size = new Vector2((_tilewidth / 5) * 2, 1);

            GameObject collider_up_go_2 = new GameObject();
            SetLayerAndTagWall(collider_up_go_2);
            collider_up_go_2.transform.position = new Vector3(_x_pos + ((_tilewidth / 5) * (5 - 1)) - 0.5f, _y_pos + (_tileheight - 1));
            collider_up_go_2.transform.SetParent(Room_Go.transform);

            BoxCollider2D up_collider2 = collider_up_go_2.AddComponent<BoxCollider2D>();
            up_collider2.size = new Vector2((_tilewidth / 5) * 2, 1);
        }
        else
        {
            GameObject collider_up_go = new GameObject();
            SetLayerAndTagWall(collider_up_go);
            collider_up_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1) * 0.5f, _y_pos + (_tileheight - 1));
            collider_up_go.transform.SetParent(Room_Go.transform);

            BoxCollider2D up_collider = collider_up_go.AddComponent<BoxCollider2D>();
            up_collider.size = new Vector2(_tilewidth, 1);
        }

    }
    void AddDownColliders()
    {
        ExitDirectionPoint exitTemp = GetExitInfoUsable(ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT);
        bool isInsideRoom = false;
        if (exitTemp != null)
        {
            isInsideRoom = ProceduralDungeonGenerator.mapGenerator.PointIsInsideAnyRoom(exitTemp.nextRoomPos);
        }

        if (IsAlreadyExitDirection(ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT) && countCollidersExits < numExits 
            && isInsideRoom && exitTemp.isUsed)
        {
            countCollidersExits++;

            colliderFinalColliders.Add(ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT);

            GameObject collider_down_go_1 = new GameObject();
            SetLayerAndTagWall(collider_down_go_1);
            collider_down_go_1.transform.position = new Vector3(_x_pos + (((_tilewidth / 5)) - 0.5f), _y_pos);
            collider_down_go_1.transform.SetParent(Room_Go.transform);

            BoxCollider2D down_collider1 = collider_down_go_1.AddComponent<BoxCollider2D>();
            down_collider1.size = new Vector2((_tilewidth / 5) * 2, 1);

            GameObject collider_down_go_2 = new GameObject();
            SetLayerAndTagWall(collider_down_go_2);
            collider_down_go_2.transform.position = new Vector3(_x_pos + ((_tilewidth / 5) * (5 - 1)) - 0.5f, _y_pos);
            collider_down_go_2.transform.SetParent(Room_Go.transform);

            BoxCollider2D down_collider2 = collider_down_go_2.AddComponent<BoxCollider2D>();
            down_collider2.size = new Vector2((_tilewidth / 5) * 2, 1);
        }
        else
        {
            GameObject collider_down_go = new GameObject();
            SetLayerAndTagWall(collider_down_go);
            collider_down_go.transform.position = new Vector3(_x_pos + (_tilewidth - 1) * 0.5f, _y_pos);
            collider_down_go.transform.SetParent(Room_Go.transform);

            BoxCollider2D down_collider = collider_down_go.AddComponent<BoxCollider2D>();
            down_collider.size = new Vector2(_tilewidth, 1);
        }
    }

    void SetLayerAndTagWall(GameObject go)
    {
        go.tag = "wall";
        go.layer = 11;
    }
    void SetLayerAndTagDoor(GameObject go)
    {
        go.tag = "door";
        go.layer = 13;
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

    bool IsColliderExit(ProceduralDungeonGenerator.ExitDirection mydir)
    {
        bool ret = false;
        for (int i = 0; i < colliderFinalColliders.Count; i++)
        {
                if (colliderFinalColliders[i] == mydir)
                {
                    ret = true;
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

    ExitDirectionPoint GetExitInfoUsable(ProceduralDungeonGenerator.ExitDirection mydir)
    {
        ExitDirectionPoint temp = null;

        for (int i = 0; i < usableExits.Count; i++)
        {
            if (usableExits[i] != null)
            {
                if (usableExits[i].dir == mydir)
                {
                    temp = usableExits[i];
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
                        temp = ProceduralDungeonGenerator.mapGenerator.InstantiateWithTile(false, Room_Go.transform);
                        temp.transform.position = tile_pos;
                        break;

                    case ProceduralDungeonGenerator.TileType.LEFTUP_RIGHT_BORDER:
                    case ProceduralDungeonGenerator.TileType.LEFTUP_DOWN_BORDER:
                    case ProceduralDungeonGenerator.TileType.RIGHTUP_LEFT_BORDER:
                    case ProceduralDungeonGenerator.TileType.RIGHTUP_DOWN_BORDER:
                    case ProceduralDungeonGenerator.TileType.LEFTDOWN_UP_BORDER:
                    case ProceduralDungeonGenerator.TileType.LEFTDOWN_RIGHT_BORDER:
                    case ProceduralDungeonGenerator.TileType.RIGHTDOWN_LEFT_BORDER:
                    case ProceduralDungeonGenerator.TileType.RIGHTDOWN_UP_BORDER:
                        temp = ProceduralDungeonGenerator.mapGenerator.InstantiateCornerBorderTile(room[i][j], Room_Go.transform);
                        temp.transform.position = tile_pos;
                        break;

                    case ProceduralDungeonGenerator.TileType.LEFT_DOWN_CORNER:
                    case ProceduralDungeonGenerator.TileType.LEFT_UP_CORNER:
                    case ProceduralDungeonGenerator.TileType.RIGHT_DOWN_CORNER:
                    case ProceduralDungeonGenerator.TileType.RIGHT_UP_CORNER:
                        //temp = ProceduralDungeonGenerator.mapGenerator.InstantiateWithTile(false, true, false, Room_Go.transform);
                        temp = ProceduralDungeonGenerator.mapGenerator.InstantiateCornerTile(room[i][j], Room_Go.transform);
                        temp.transform.position = tile_pos;
                        break;
                    case ProceduralDungeonGenerator.TileType.UP_DOOR_1:
                    case ProceduralDungeonGenerator.TileType.UP_DOOR_2:
                    case ProceduralDungeonGenerator.TileType.UP_DOOR_PREV_0:
                    case ProceduralDungeonGenerator.TileType.UP_DOOR_PREV_3:
                    case ProceduralDungeonGenerator.TileType.DOWN_DOOR_1:
                    case ProceduralDungeonGenerator.TileType.DOWN_DOOR_2:
                    case ProceduralDungeonGenerator.TileType.DOWN_DOOR_PREV_0:
                    case ProceduralDungeonGenerator.TileType.DOWN_DOOR_PREV_3:
                    case ProceduralDungeonGenerator.TileType.RIGHT_DOOR_1:
                    case ProceduralDungeonGenerator.TileType.RIGHT_DOOR_2:
                    case ProceduralDungeonGenerator.TileType.RIGHT_DOOR_PREV_0:
                    case ProceduralDungeonGenerator.TileType.RIGHT_DOOR_PREV_3:
                    case ProceduralDungeonGenerator.TileType.LEFT_DOOR_1:
                    case ProceduralDungeonGenerator.TileType.LEFT_DOOR_2:
                    case ProceduralDungeonGenerator.TileType.LEFT_DOOR_PREV_0:
                    case ProceduralDungeonGenerator.TileType.LEFT_DOOR_PREV_3:
                        temp = ProceduralDungeonGenerator.mapGenerator.InstantiateWithTile(true, Room_Go.transform);
                        temp.transform.position = tile_pos;
                        break;

                    default:
                        temp = ProceduralDungeonGenerator.mapGenerator.InstantiateWallTile(room[i][j], Room_Go.transform);
                        temp.transform.position = tile_pos;
                        break;
                }
            }
        }
    }

    public bool IsInsideRoom(Vector3 point)
    {
        bool ret = false;

        if (point.x >= _x_pos && point.x < _x_pos + _tilewidth && point.y >= _y_pos && point.y < _y_pos + _tileheight)
        {
            ret = true;
        }

        return ret;
    }


}
