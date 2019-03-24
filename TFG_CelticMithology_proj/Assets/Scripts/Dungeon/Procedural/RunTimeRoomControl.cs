using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeRoomControl : MonoBehaviour {

    [HideInInspector]public List<BoxCollider2D> mydoors;
    private BoxCollider2D myComponent;
    private List<RunTimeRoomControl> myneighbourgs;

    private int _x_pos;
    private int _y_pos;
    private int _tilewidth;
    private int _tileheight;

    private int mylevel = 0;
    private int mynumRoom = 0;

    public void InitializeRoomValues(BoxCollider2D detectPlayerValue, List<BoxCollider2D> doors,
    int x, int y, int width, int height, int level, int numRoom)
    {
        _x_pos = x;
        _y_pos = y;
        _tilewidth = width;
        _tileheight = height;


        mylevel = level;

        mynumRoom = numRoom;

        mydoors = doors;

        if (mylevel != 0 || mynumRoom != 0)
        {
            myComponent = gameObject.AddComponent<BoxCollider2D>();
            myComponent.isTrigger = true;
            myComponent.offset = new Vector2(detectPlayerValue.offset.x + _x_pos, detectPlayerValue.offset.y + _y_pos);
            myComponent.size = detectPlayerValue.size;
        }
    }

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ActivateDeactivateDoors(true);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "player_movement_collider")
        {
            //closeDoors
            ActivateDeactivateDoors(false);
        }
    }

    void ActivateDeactivateDoors(bool needTrigger)
    {
        for(int i=0; i<mydoors.Count; i++)
        {
            mydoors[i].isTrigger = needTrigger;
        }
    }

}
