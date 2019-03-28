using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoom
{
    public GameObject myEnemy;
    public Enemy_type myEnemyType;

    public EnemiesRoom(Enemy_type newMyEnemyType, GameObject newMyEnemy)
    {
        myEnemyType = newMyEnemyType;
        myEnemy = newMyEnemy;
    }
}

public enum EnemyPositionEnum
{
    LEFT_TOP_CORNER, MIDDLE_TOP_POS, RIGHT_TOP_CORNER,

    LEFT_UPPERMIDDLE_POS, MIDDLE_UPPERMIDDLE_POS, RIGHT_UPPERMIDDLE_POS,

    LEFT_DOWNMIDDLE_POS, MIDDLE_DOWNMIDDLE_POS, RIGHT_DOWNMIDDLE_POS,

    LEFT_BOT_CORNER, MIDDLE_BOT_POS, RIGHT_BOT_CORNER
}

public class EnemyPos
{
    public Vector3 _position;
    public bool _alreadyUsed=false;
    public EnemyPositionEnum _positionType;

    public EnemyPos(Vector3 newpos, EnemyPositionEnum typePos)
    {
        _position = newpos;
        _positionType = typePos;
    }

}

public class RunTimeRoomControl : MonoBehaviour {

    [HideInInspector]public List<BoxCollider2D> mydoors;
    private BoxCollider2D _myComponent;

    private List<RunTimeRoomControl> _myneighbourgs;

    private List<EnemyPos> _enemyPositions;

    private List<EnemiesRoom> _enemiesInRoom;
    private List<EnemiesRoom> _possibleEnemies;

    private bool _playerInsideRoom = false;

    private int _x_pos;
    private int _y_pos;
    private int _tilewidth;
    private int _tileheight;

    private int mylevel = 0;
    private int mynumRoom = 0;

    private int enemiesDead = 0;

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

        _enemiesInRoom = new List<EnemiesRoom>();
        _possibleEnemies = new List<EnemiesRoom>();

        SetPosiblePointsEnemies();

        SetEnemies();

        SpawnEnemies();

        if (mylevel != 0 || mynumRoom != 0)
        {
            _myComponent = gameObject.AddComponent<BoxCollider2D>();
            _myComponent.isTrigger = true;
            _myComponent.offset = new Vector2(detectPlayerValue.offset.x + _x_pos, detectPlayerValue.offset.y + _y_pos);
            _myComponent.size = detectPlayerValue.size;
        }
    }

    void SetPosiblePointsEnemies()
    {
        _enemyPositions = new List<EnemyPos>();

        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + 1, _y_pos + 1, 0), EnemyPositionEnum.LEFT_BOT_CORNER));
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + 1, 0), EnemyPositionEnum.MIDDLE_BOT_POS));
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth - 2, _y_pos + 1, 0), EnemyPositionEnum.RIGHT_BOT_CORNER));

        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + 1, _y_pos + _tileheight - 2, 0), EnemyPositionEnum.LEFT_TOP_CORNER));
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + _tileheight - 2, 0), EnemyPositionEnum.MIDDLE_TOP_POS));
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth - 2, _y_pos + _tileheight - 2, 0), EnemyPositionEnum.RIGHT_TOP_CORNER));

        float _middleDownPosition = ((_tileheight) / 6) * 2;

        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + 1, _y_pos + _middleDownPosition, 0), EnemyPositionEnum.LEFT_DOWNMIDDLE_POS));
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + _middleDownPosition, 0), EnemyPositionEnum.MIDDLE_DOWNMIDDLE_POS));
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth - 2, _y_pos + _middleDownPosition, 0), EnemyPositionEnum.RIGHT_DOWNMIDDLE_POS));

        float _middleUpperPosition = ((_tileheight) / 6) * 4;

        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + 1, _y_pos + _middleUpperPosition, 0), EnemyPositionEnum.LEFT_DOWNMIDDLE_POS));
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + _middleUpperPosition, 0), EnemyPositionEnum.MIDDLE_DOWNMIDDLE_POS));
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth - 2, _y_pos + _middleUpperPosition, 0), EnemyPositionEnum.RIGHT_DOWNMIDDLE_POS));
    }

    public void SetEnemies()
    {
        _possibleEnemies.Add(new EnemiesRoom(Enemy_type.MEELE_ENEMY, ProceduralDungeonGenerator.mapGenerator.meleeEnemey));
        _possibleEnemies.Add(new EnemiesRoom(Enemy_type.CARTONACH_ENEMY, ProceduralDungeonGenerator.mapGenerator.caorthannach));
    }

    void SpawnEnemies()
    {
        //First test to spawn

        _enemiesInRoom.Add(new EnemiesRoom(_possibleEnemies[0].myEnemyType, Instantiate(_possibleEnemies[0].myEnemy)));
        _enemiesInRoom[0].myEnemy.transform.position = _enemyPositions[4]._position;
        _enemyPositions[4]._alreadyUsed = true;


        _enemiesInRoom.Add(new EnemiesRoom(_possibleEnemies[1].myEnemyType, Instantiate(_possibleEnemies[1].myEnemy)));
        _enemiesInRoom[1].myEnemy.transform.position = _enemyPositions[6]._position;
        _enemyPositions[6]._alreadyUsed = true;
    }

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        if (_playerInsideRoom)
        {
            if (enemiesDead == _enemiesInRoom.Count)
            {
                ActivateDeactivateDoors(true);
            }
        }
	}


    void DetectEnemyLives()
    {
        Blackboard bb;

        foreach (EnemiesRoom enemiesRoom in _enemiesInRoom)
        {
            switch (enemiesRoom.myEnemyType)
            {
                case Enemy_type.MEELE_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponent<Soldier_Blackboard>();
                    if (bb != null)
                    {
                        if (((Soldier_Blackboard)bb).life.myValue <= 0)
                        {
                            enemiesDead++;
                        }
                    }
                    break;
                case Enemy_type.CARTONACH_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponent<Caorthannach_Blackboard>();
                    if (bb != null)
                    {
                        if (((Caorthannach_Blackboard)bb).life.myValue <= 0)
                        {
                            enemiesDead++;
                        }
                    }
                    break;
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "player_movement_collider")
        {
            //closeDoors
            _playerInsideRoom = true;
            ActivateDeactivateDoors(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player_movement_collider")
        {
            _playerInsideRoom = false;
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
