﻿using System.Collections;
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
    [SerializeField] private int numRooms;

    [Header("Room values")]
    public int tilesWidthRoom;
    public int tilesHeightRoom;

    public GameObject floorTile;

    Procedural_Room[] rooms;

    public enum TileType
    {
        LEFT_WALL, RIGHT_WALL, UP_WALL, DOWN_WALL,
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

        rooms = new Procedural_Room[numRooms];

        Procedural_Room room_temp = new Procedural_Room(0,0, tilesWidthRoom, tilesHeightRoom);
        rooms[0] = room_temp;

        room_temp.DrawRoom();

        ExitDirectionPoint point = room_temp.exits[Random.RandomRange(0, room_temp.exits.Length)];

        Procedural_Room room_temp2 = new Procedural_Room(point.nextRoomPos.x, point.nextRoomPos.y, tilesWidthRoom, tilesHeightRoom);
        rooms[1] = room_temp2;

        room_temp2.DrawRoom();

        ExitDirectionPoint point2 = room_temp2.exits[Random.RandomRange(0, room_temp2.exits.Length)];

        Procedural_Room room_temp3 = new Procedural_Room(point2.nextRoomPos.x, point2.nextRoomPos.y, tilesWidthRoom, tilesHeightRoom);
        rooms[2] = room_temp3;
        room_temp3.DrawRoom();


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

    public GameObject InstantiateWithTile()
    {
        return Instantiate(floorTile, transform);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
