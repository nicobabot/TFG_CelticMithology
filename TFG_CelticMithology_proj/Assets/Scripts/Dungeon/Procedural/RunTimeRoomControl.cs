using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
//using Random.range

public class EnemiesRoom
{
    public GameObject myEnemy;
    public Enemy_type myEnemyType;
    public int mydificultyPoints;

    public EnemiesRoom(Enemy_type newMyEnemyType, GameObject newMyEnemy, int difPoints)
    {
        myEnemyType = newMyEnemyType;
        myEnemy = newMyEnemy;
        mydificultyPoints = difPoints;
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

    
    [HideInInspector]public Dictionary<BoxCollider2D, ProceduralDungeonGenerator.ExitDirection> mydoors;
    private BoxCollider2D _myComponent;

    private List<RunTimeRoomControl> _myneighbourgs;

    private List<EnemyPos> _enemyPositions;

    private List<EnemiesRoom> _enemiesInRoom;
    private List<EnemiesRoom> _possibleEnemies;

    private bool _playerInsideRoom = false;
    private bool _roomFinished = false;

    private int _x_pos;
    private int _y_pos;
    private int _tilewidth;
    private int _tileheight;

    private int _mylevel = 0;
    private int _mynumRoom = 0;

    private int _enemiesDead = 0;
    private int _enemiesInRoomCount = 0;

    private int _maximumPoints = 20;
    private int _counterPints = 0;

    private bool _continueSpawning = true;

    public void InitializeRoomValues(BoxCollider2D detectPlayerValue, Dictionary<BoxCollider2D, ProceduralDungeonGenerator.ExitDirection> doors,
    int x, int y, int width, int height, int level, int numRoom)
    {
        _x_pos = x;
        _y_pos = y;
        _tilewidth = width;
        _tileheight = height;


        _mylevel = level;

        _mynumRoom = numRoom;

        mydoors = doors;

        _enemiesInRoom = new List<EnemiesRoom>();
        _possibleEnemies = new List<EnemiesRoom>();


        if (_mylevel != 0 || _mynumRoom != 0)
        {

            SetPosiblePointsEnemies();

            SetEnemies();

            SpawnEnemies();

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

        if(!mydoors.ContainsValue(ProceduralDungeonGenerator.ExitDirection.DOWN_EXIT))
        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + 1, 0), EnemyPositionEnum.MIDDLE_BOT_POS));

        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth - 2, _y_pos + 1, 0), EnemyPositionEnum.RIGHT_BOT_CORNER));
        //-------------------------


        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + 1, _y_pos + _tileheight - 2, 0), EnemyPositionEnum.LEFT_TOP_CORNER));

        if (!mydoors.ContainsValue(ProceduralDungeonGenerator.ExitDirection.UP_EXIT))
            _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + _tileheight - 2, 0), EnemyPositionEnum.MIDDLE_TOP_POS));

        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth - 2, _y_pos + _tileheight - 2, 0), EnemyPositionEnum.RIGHT_TOP_CORNER));
        //-----------------------

        float _middleDownPosition = ((_tileheight) / 6) * 2;

        if (!mydoors.ContainsValue(ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT))
            _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + 1, _y_pos + _middleDownPosition, 0), EnemyPositionEnum.LEFT_DOWNMIDDLE_POS));

        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + _middleDownPosition, 0), EnemyPositionEnum.MIDDLE_DOWNMIDDLE_POS));

        if (!mydoors.ContainsValue(ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT))
            _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth - 2, _y_pos + _middleDownPosition, 0), EnemyPositionEnum.RIGHT_DOWNMIDDLE_POS));
        //---------------------------

        float _middleUpperPosition = ((_tileheight) / 6) * 4;

        if (!mydoors.ContainsValue(ProceduralDungeonGenerator.ExitDirection.LEFT_EXIT))
            _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + 1, _y_pos + _middleUpperPosition, 0), EnemyPositionEnum.LEFT_DOWNMIDDLE_POS));

        _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + _middleUpperPosition, 0), EnemyPositionEnum.MIDDLE_DOWNMIDDLE_POS));

        if (!mydoors.ContainsValue(ProceduralDungeonGenerator.ExitDirection.RIGHT_EXIT))
            _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth - 2, _y_pos + _middleUpperPosition, 0), EnemyPositionEnum.RIGHT_DOWNMIDDLE_POS));
    }

    public void SetEnemies()
    {
        _possibleEnemies.Add(new EnemiesRoom(Enemy_type.MEELE_ENEMY, ProceduralDungeonGenerator.mapGenerator.meleeEnemey, 5));
        _possibleEnemies.Add(new EnemiesRoom(Enemy_type.CARTONACH_ENEMY, ProceduralDungeonGenerator.mapGenerator.caorthannach, 5));
    }

    void SpawnEnemies()
    {
        //First test to spawn

        //_enemiesInRoom.Add(new EnemiesRoom(_possibleEnemies[0].myEnemyType, Instantiate(_possibleEnemies[0].myEnemy), _possibleEnemies[0].mydificultyPoints));
        //_enemiesInRoom[0].myEnemy.transform.position = _enemyPositions[4]._position;
        //_enemyPositions[4]._alreadyUsed = true;


        //_enemiesInRoom.Add(new EnemiesRoom(_possibleEnemies[1].myEnemyType, Instantiate(_possibleEnemies[1].myEnemy), _possibleEnemies[1].mydificultyPoints));
        //_enemiesInRoom[1].myEnemy.transform.position = _enemyPositions[6]._position;
        //_enemyPositions[6]._alreadyUsed = true;

        if (_continueSpawning)
        {
            foreach (EnemiesRoom enemy in _possibleEnemies)
            {
                if(NumberEnemiesOfType(enemy.myEnemyType) <= 2)
                {
                    EnemyPos enemyPos = GetPositionToSpawnNotUsed();

                    GameObject go = Instantiate(enemy.myEnemy);
                    go.transform.position = enemyPos._position;
                    _enemiesInRoom.Add(new EnemiesRoom(enemy.myEnemyType, go, enemy.mydificultyPoints));
                    _counterPints += enemy.mydificultyPoints;
                }
            }

            if (_counterPints == _maximumPoints)
            {
                _continueSpawning = true;
            }
            else
            {
                SpawnEnemies();
            }
        }

        _enemiesInRoomCount = _enemiesInRoom.Count;
    }

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        if (_playerInsideRoom && !_roomFinished)
        {
            DetectEnemyLives();

            if (_enemiesDead >= _enemiesInRoomCount)
            {
                _playerInsideRoom = false;
                _roomFinished = true;
                ActivateDeactivateDoors(true);
            }
        }
	}

    EnemyPos GetPositionToSpawnNotUsed()
    {
        EnemyPos ret;
        int randPos = 0;

        do
        {
            randPos = UnityEngine.Random.Range(0, _enemyPositions.Count);
        } while (_enemyPositions[randPos]._alreadyUsed);

        _enemyPositions[randPos]._alreadyUsed = true;
        ret = _enemyPositions[randPos];

        return ret;
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
                            _enemiesInRoom.Remove(enemiesRoom);
                            _enemiesDead++;
                        }
                    }
                    break;
                case Enemy_type.CARTONACH_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponent<Caorthannach_Blackboard>();
                    if (bb != null)
                    {
                        if (((Caorthannach_Blackboard)bb).life.myValue <= 0)
                        {
                            _enemiesInRoom.Remove(enemiesRoom);
                            _enemiesDead++;
                        }
                    }
                    break;
            }
        }
    }

    void ActivateEnemies()
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
                        ((Soldier_Blackboard)bb).playerIsInsideRoom.myValue = true;
                    }
                    break;
                case Enemy_type.CARTONACH_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponent<Caorthannach_Blackboard>();
                    if (bb != null)
                    {
                        ((Caorthannach_Blackboard)bb).playerIsInsideRoom.myValue = true;
                    }
                    break;
            }
        }
    }

    int NumberEnemiesOfType(Enemy_type type)
    {
        int retNum = 0;
        foreach (EnemiesRoom enemy in _enemiesInRoom)
        {
            if(enemy.myEnemyType == type)
            {
                retNum++;
            }
        }
        return retNum;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "player_movement_collider")
        {
            //closeDoors
            _playerInsideRoom = true;
            if (!_roomFinished)
            {
                ActivateDeactivateDoors(false);
                ActivateEnemies();
            }
        }
    }

    void ActivateDeactivateDoors(bool needTrigger)
    {
        foreach (KeyValuePair<BoxCollider2D, ProceduralDungeonGenerator.ExitDirection> door in mydoors)
        {
            door.Key.isTrigger = needTrigger;
        }
    }

}
