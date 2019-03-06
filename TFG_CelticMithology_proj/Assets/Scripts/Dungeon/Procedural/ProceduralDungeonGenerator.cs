using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

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
    [SerializeField] private int tilesWidthMap;
    [SerializeField] private int tilesHeightMap;
    [SerializeField] private int maxim_depth;

    [Header("Room values")]
    public int tilesWidthRoom;
    public int tilesHeightRoom;

    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject cornerTile;
    int count_rooms = 0;
    private int realDepth = 0;

    List<Procedural_Room> rooms;

    public enum TileType
    {
        LEFT_WALL, RIGHT_WALL, UP_WALL, DOWN_WALL,
        LEFT_DOWN_CORNER, LEFT_UP_CORNER, RIGHT_DOWN_CORNER, RIGHT_UP_CORNER,
        FLOOR,
        HOLE,
    }

    public enum ExitDirection
    {
        RIGHT_EXIT,
        LEFT_EXIT,
        UP_EXIT,
        DOWN_EXIT
    }

    private void Awake()
    {
        _mapgenerator = this;
    }

    // Use this for initialization
    void Start () {
        //DrawMapWithTiles();

        rooms = new List<Procedural_Room>();

        //int count_rooms = 0;

        //Procedural_Room room_temp = new Procedural_Room(0,0, tilesWidthRoom, tilesHeightRoom, count_rooms);
        //rooms[0] = room_temp;

        //room_temp.DrawRoom();

        //ExitDirectionPoint point = room_temp.exits[Random.RandomRange(0, room_temp.exits.Length)];

        //count_rooms++;
        //Procedural_Room room_temp2 = new Procedural_Room(point.nextRoomPos.x, point.nextRoomPos.y, tilesWidthRoom, tilesHeightRoom, count_rooms);
        //rooms[1] = room_temp2;

        //room_temp2.DrawRoom();

        //ExitDirectionPoint point2 = room_temp2.exits[Random.RandomRange(0, room_temp2.exits.Length)];

        //count_rooms++;
        //Procedural_Room room_temp3 = new Procedural_Room(point2.nextRoomPos.x, point2.nextRoomPos.y, tilesWidthRoom, tilesHeightRoom, count_rooms);
        //rooms[2] = room_temp3;
        //room_temp3.DrawRoom();

        //----------------------------------------------------------------------
        Procedural_Room room_temp = new Procedural_Room(0, 0, tilesWidthRoom, tilesHeightRoom, count_rooms);
        rooms.Add(room_temp);
        room_temp.DrawRoom();

        realDepth = maxim_depth / room_temp.usableExits.Count;

        GenerateRoomLastChildFirst(room_temp,0);

    }

    void GenerateRoomLastChildFirst(Procedural_Room room, int level)
    {

        Procedural_Room creation_room;
        ExitDirectionPoint creation_point = new ExitDirectionPoint();
        count_rooms++;

        if (level == realDepth)
        {
            return;
        }

        for (int i = 0; i < room.usableExits.Count; i++)
        {
            creation_point = room.usableExits[i];

            creation_room = new Procedural_Room(creation_point.nextRoomPos.x, creation_point.nextRoomPos.y, tilesWidthRoom, tilesHeightRoom, count_rooms);
            rooms.Add(creation_room);
            creation_room.DrawRoom();

            GenerateRoomLastChildFirst(creation_room, level + 1);
        }
    }

    void GenerateRoomFirstChildFirst(Procedural_Room room, int level)
    {

        Procedural_Room creation_room;
        ExitDirectionPoint creation_point = new ExitDirectionPoint();
        
        if(level == maxim_depth)
        {
            return;
        }

        for(int i= room.usableExits.Count-1; i >= 0; i--)
        {
            creation_point = room.usableExits[i];
            creation_room = new Procedural_Room(creation_point.nextRoomPos.x, creation_point.nextRoomPos.y, tilesWidthRoom, tilesHeightRoom, count_rooms);
            rooms.Add(creation_room);
            count_rooms++;
            creation_room.DrawRoom();
        }

        for (int j = count_rooms; j > count_rooms - room.usableExits.Count-1; j--)
        {
            GenerateRoomFirstChildFirst(rooms[j], level + 1);
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
            if(room_temp!=null)
            ret = room_temp.IsInsideRoom(point);

            if (ret == true) break;
        }


        return ret;

    }

    public GameObject InstantiateWithTile(bool is_wall = false, bool is_corner=false, Transform parent = null)
    {
        if (parent != null)
        {
            if (!is_wall && !is_corner)
                return Instantiate(floorTile, parent);
            else if (is_corner) return Instantiate(cornerTile, parent);
            else return Instantiate(wallTile, parent);
        }
        return null;
    }

    public GameObject InstantiateGO(GameObject go)
    {
        return Instantiate(go);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
