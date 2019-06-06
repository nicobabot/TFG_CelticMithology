using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[Serializable]
public class WallsSprites
{
    public Sprite[] tiles;
}

public class ProceduralDungeonGenerator : MonoBehaviour {

    static ProceduralDungeonGenerator _mapgenerator;

    static public ProceduralDungeonGenerator mapGenerator
    {
        get
        {
            if(_mapgenerator == null)
            {
                _mapgenerator = Resources.Load("PrefabsResources/MapGenerator") as ProceduralDungeonGenerator; 
            }

            return _mapgenerator;
        }
    }

    private static Vector2Int StarterRoomCenterPoint = Vector2Int.zero;

    [Header("Map values")]
    public Tilemap grid;
    public int numDungeon;
    [SerializeField] private int tilesWidthMap;
    [SerializeField] private int tilesHeightMap;
    [SerializeField] private int maxim_depth;

    public GameObject Player;

    [Header("Room values")]
    public int tilesWidthRoom;
    public int tilesHeightRoom;

    public BoxCollider2D DetectPlayer;

    public GameObject floorTile;
    public Sprite[] floorTilesSprites;
    public Sprite[] cornersTilesSprites;

    [Header("Tiles wall")]
    public int tileNumUsing = 0;
    [Tooltip("ELEMENT 0 - LEFT WALL \nELEMENT 1 - RIGHT WALL \nELEMENT 3 - UP WALL \nELEMENT 4 - DOWN WALL\n" +
        "ELEMENT 5 - LEFT/RIGHT WALL BORDER\nELEMENT 6 -  UP/DOWN WALL BORDER")]
    public WallsSprites[] wallTilesSprites;


    [Space(10)]
    [Header("Door Prefabs")]
    public GameObject doorPrefabVertical;
    public GameObject doorPrefabHorizontal;
    public Animator doorTest;
    [HideInInspector] public List<GameObject> myDoors;

   /* public Animator doorAnimatorVertical;
    public Animator doorAnimatorHorizontal;
    public AnimatorController doorAnimator;*/

    [Header("Tiles along")]
    public GameObject wallTile;
    public GameObject cornerTile;
    public GameObject doorTile;
    public GameObject borderWallTile;
    public GameObject shadowRoom;

    [Header("Enemy prefabs")]
    public GameObject meleeEnemey;
    public GameObject caorthannach;
    public GameObject bluecaorthannach;
    public GameObject dearDug;
    public GameObject banshee;
    public GameObject macLir;
    public GameObject kelpie;

    [Header("Upgrade")]
    public GameObject UpgradeObject;

    [HideInInspector] public int realDepth = 0;
    int count_rooms = 0;

    private bool _AddBoss = false;
    private int _counterBoss = 0;

    private bool _AddMiniBoss = false;
    private bool _FirstShadowNot = false;
    private int _counterMiniBoss = 0;

    List<Procedural_Room> rooms;

    public enum TileType
    {
        LEFT_WALL, RIGHT_WALL, UP_WALL, DOWN_WALL,
        LEFT_DOWN_CORNER, LEFT_UP_CORNER, RIGHT_DOWN_CORNER, RIGHT_UP_CORNER,

        LEFTUP_RIGHT_BORDER, LEFTUP_DOWN_BORDER,
        RIGHTUP_LEFT_BORDER, RIGHTUP_DOWN_BORDER,
        LEFTDOWN_UP_BORDER, LEFTDOWN_RIGHT_BORDER,
        RIGHTDOWN_LEFT_BORDER, RIGHTDOWN_UP_BORDER,

        LEFT_DOOR_PREV_0, LEFT_DOOR_1, LEFT_DOOR_2, LEFT_DOOR_PREV_3,
        RIGHT_DOOR_PREV_0, RIGHT_DOOR_1, RIGHT_DOOR_2, RIGHT_DOOR_PREV_3,
        UP_DOOR_PREV_0, UP_DOOR_1, UP_DOOR_2, UP_DOOR_PREV_3,
        DOWN_DOOR_PREV_0, DOWN_DOOR_1, DOWN_DOOR_2, DOWN_DOOR_PREV_3,

        FLOOR,
        HOLE,
    }

    public enum ExitDirection
    {
        RIGHT_EXIT,
        LEFT_EXIT,
        UP_EXIT,
        DOWN_EXIT,
        NONE_DIR
    }

    private void Awake()
    {
        _mapgenerator = this;
    }

    // Use this for initialization
    void Start () {
        //DrawMapWithTiles();
        realDepth = maxim_depth / 3;

        rooms = new List<Procedural_Room>();
        myDoors = new List<GameObject>();

        Procedural_Room room_temp = new Procedural_Room(0, 0, tilesWidthRoom, tilesHeightRoom, count_rooms, ExitDirection.NONE_DIR, 0);
        rooms.Add(room_temp);
        //room_temp.DrawRoom();

        GenerateRoomLastChildFirst(room_temp,0);

        foreach (Procedural_Room testRoom in rooms)
        {
            if(testRoom != null)
            {
                testRoom.SetColliders();
                testRoom.SetDoors();

                testRoom.InstantiateDoorPrefabs();

                //InitializeRoomsRunTimeValues
                testRoom.InitializeRoomRunTimeValues();

                testRoom.DrawRoom();

                if (testRoom.GetLevelDepth() > 0 || _FirstShadowNot)
                {
                    Vector3 posShadow = testRoom.GetStarterRoom();
                    GameObject goShadow = Instantiate(shadowRoom, testRoom.Room_Go.transform);
                    goShadow.transform.localPosition = posShadow + goShadow.transform.localPosition;
                }
                else _FirstShadowNot = true;

            }
        }
    }

    void GenerateRoomLastChildFirst(Procedural_Room room, int level)
    {

        Procedural_Room creation_room;
        ExitDirectionPoint creation_point = new ExitDirectionPoint();
        count_rooms++;

        if (level + 1 == realDepth && !_AddBoss && _counterBoss == 0)
        {
            _AddBoss = true;
        }
        else if (level == realDepth*0.5 && !_AddMiniBoss && _counterMiniBoss == 0)
        {
            _AddMiniBoss = true;
        }


        if (level == realDepth)
        {

            Vector3 posUpgrade = room.GetMiddlePositionRoom();
            GameObject go = Instantiate(UpgradeObject);
            go.transform.position = posUpgrade;

            int it = FindInRooms(room);
            if(rooms[it].usableExits.Count>0)
            rooms[it].usableExits[0].isUsed = true;
            return;
        }

        for (int i = 0; i < room.usableExits.Count; i++)
        {
            creation_point = room.usableExits[i];

            bool wantToCreate = creation_point.needRoomCreation;
            if (!wantToCreate) {
                int it = FindInRooms(room);
                rooms[it].usableExits[i].isUsed = true;
            }

            if (creation_point.needRoomCreation && !PointIsInsideAnyRoom(creation_point.nextRoomPos))
            {
                int it = FindInRooms(room);
                rooms[it].usableExits[i].isUsed = true;

                if (_AddBoss)
                    _counterBoss++;
                else if (_AddMiniBoss)
                    _counterMiniBoss++;

                creation_room = new Procedural_Room(creation_point.nextRoomPos.x, creation_point.nextRoomPos.y, tilesWidthRoom, tilesHeightRoom, count_rooms, OppositeDirection(creation_point.dir), level, _AddBoss, _AddMiniBoss);

                _AddBoss = false;
                _AddMiniBoss = false;

                if (creation_room.wantToDraw)
                {
                    rooms.Add(creation_room);
                    GenerateRoomLastChildFirst(creation_room, level + 1);
                }
            }
        }
    }

    ExitDirection OppositeDirection(ExitDirection dir)
    {
        ExitDirection oppositeDir = ExitDirection.NONE_DIR;

        switch (dir)
        {
            case ExitDirection.RIGHT_EXIT:
                oppositeDir = ExitDirection.LEFT_EXIT;
                break;
            case ExitDirection.LEFT_EXIT:
                oppositeDir = ExitDirection.RIGHT_EXIT;
                break;
            case ExitDirection.UP_EXIT:
                oppositeDir = ExitDirection.DOWN_EXIT;
                break;
            case ExitDirection.DOWN_EXIT:
                oppositeDir = ExitDirection.UP_EXIT;
                break;
        }

        return oppositeDir;
    }

    public void ActivateDeactivateDoors(bool openDoor)
    {
        foreach(GameObject go in myDoors)
        {
            if (go == null) continue;

            Animator anim = go.GetComponent<Animator>();

            if (anim == null) continue;

            anim.SetBool("OpenDoor", openDoor);
        }
    }

    void DrawMapWithTiles()
    {
        Vector2Int _startingPos = new Vector2Int((int)(StarterRoomCenterPoint.x - (tilesWidthRoom * 0.5f)), (int)(StarterRoomCenterPoint.y - (tilesHeightRoom * 0.5f)));

        for (int i=0; i< tilesWidthMap; i++)
        {
            for (int j = 0; j < tilesHeightMap; j++)
            {
                GameObject temp = Instantiate(floorTile, transform);
                temp.transform.position = grid.CellToWorld(new Vector3Int(_startingPos.x + i, _startingPos.y + j, 0));
            }
        }
    }

    public bool PointIsInsideAnyRoom(Vector3 point)
    {
        bool ret = false;

        foreach(Procedural_Room room_temp in rooms)
        {
            if (room_temp != null)
            {
                ret = room_temp.IsInsideRoom(point);
            }

            if (ret == true) break;
        }
        return ret;
    }

    public Procedural_Room GetRoomByPoint(Vector3 point)
    {
        Procedural_Room roomRet = null;
        bool ret = false;

        foreach (Procedural_Room room_temp in rooms)
        {
            if (room_temp != null)
            {
                ret = room_temp.IsInsideRoom(point);
                if (ret)
                {
                    roomRet = room_temp;
                    break;
                }
            }
        }
        return roomRet;
    }

    int FindInRooms(Procedural_Room myRoom)
    {
        int it = -1;

        for (int i=0; i< rooms.Count; i++)
        {
            if (rooms[i].Compare(myRoom))
            {
                it = i;
                break;
            }
        }

        return it;
    }

    public GameObject InstantiateWithTile(bool is_door = false, Transform parent = null)
    {
        if (parent != null)
        {
            if (!is_door)
            {
                GameObject go = new GameObject();
                go.layer = 15;
                go.transform.parent = parent;
                SpriteRenderer rend = go.AddComponent<SpriteRenderer>();

                int normalOrSpecial = UnityEngine.Random.Range(0, 10);
                int randTile = 0;
                Sprite tileSprite;

                if (normalOrSpecial < 9)
                {
                    //floor tile
                    tileSprite = floorTilesSprites[0];
                }
                else
                {
                    int specialOrClover = UnityEngine.Random.Range(0, 20);
                    if (specialOrClover < 18)
                    {
                        randTile = UnityEngine.Random.Range(1, floorTilesSprites.Length - 2);
                        tileSprite = floorTilesSprites[randTile];
                    }
                    else
                    {
                        tileSprite = floorTilesSprites[floorTilesSprites.Length - 1];
                    }
                }

                rend.sprite = tileSprite;

                return go;
            }
            else if (is_door) return Instantiate(doorTile, parent);
        }
        return null;
    }

    public GameObject InstantiateCornerTile(TileType type, Transform parent = null)
    {

        GameObject go = new GameObject();
        go.layer = 15;
        go.transform.parent = parent;
        SpriteRenderer rend = go.AddComponent<SpriteRenderer>();

        switch (type)
        {
            case TileType.LEFT_UP_CORNER:
                rend.sprite = cornersTilesSprites[0];
                break;
            case TileType.RIGHT_UP_CORNER:
                rend.sprite = cornersTilesSprites[1];
                break;
            case TileType.LEFT_DOWN_CORNER:
                rend.sprite = cornersTilesSprites[2];
                break;
            case TileType.RIGHT_DOWN_CORNER:
                rend.sprite = cornersTilesSprites[3];
                break;
        }

        return go;
    }

    public GameObject InstantiateWallTile(TileType type, Transform parent = null)
    {

        GameObject go = new GameObject();
        go.layer = 15;
        go.transform.parent = parent;
        SpriteRenderer rend = go.AddComponent<SpriteRenderer>();

        switch (type)
        {
            case TileType.LEFT_WALL:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[0];
                rend.flipX = true;
                break;
            case TileType.RIGHT_WALL:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[1];
                break;
            case TileType.UP_WALL:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[2];
                break;
            case TileType.DOWN_WALL:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[3];
                rend.flipY = true;
                break;

            #region BordersTiles

            case TileType.LEFTDOWN_RIGHT_BORDER:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[5];
                rend.flipX = true;
                rend.flipY = true;
                break;
            case TileType.LEFTDOWN_UP_BORDER:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[4];
                rend.flipX = true;
                break;

            case TileType.LEFTUP_RIGHT_BORDER:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[5];
                rend.flipX = true;
                break;
            case TileType.LEFTUP_DOWN_BORDER:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[4];
                rend.flipX = true;
                rend.flipY = true;
                break;

            case TileType.RIGHTUP_LEFT_BORDER:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[5];
                break;
            case TileType.RIGHTUP_DOWN_BORDER:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[4];
                rend.flipY = true;
                break;

            case TileType.RIGHTDOWN_LEFT_BORDER:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[5];
                rend.flipY = true;
                break;
            case TileType.RIGHTDOWN_UP_BORDER:
                rend.sprite = wallTilesSprites[tileNumUsing].tiles[4];
                break;
                #endregion
        }

        return go;
    }

    public GameObject InstantiateCornerBorderTile(TileType type, Transform parent = null)
    {
        GameObject go = Instantiate(borderWallTile, parent);
        return go;
    }

    public GameObject InstantiateGO(GameObject go, Transform trans = null)
    {
        return Instantiate(go);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
