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
    public int maxEnemiesOfType;

    public EnemiesRoom(Enemy_type newMyEnemyType, GameObject newMyEnemy, int difPoints, int newMaxEnemiesOfType=0)
    {
        myEnemyType = newMyEnemyType;
        myEnemy = newMyEnemy;
        mydificultyPoints = difPoints;
        maxEnemiesOfType = newMaxEnemiesOfType;
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

    private int[] numEnemyTypes;

    private int _x_pos;
    private int _y_pos;
    private int _tilewidth;
    private int _tileheight;

    private int _mylevel = 0;
    private int _mynumRoom = 0;

    private int _enemiesDead = 0;
    private int _enemiesInRoomCount = 0;

    private int _maximumPoints = 20;
    private int _counterPoints = 0;

    private bool _continueSpawning = true;
    private bool _wantBoss = true;
    private bool _wantMiniBoss = true;

    public void InitializeRoomValues(BoxCollider2D detectPlayerValue, Dictionary<BoxCollider2D, ProceduralDungeonGenerator.ExitDirection> doors,
    int x, int y, int width, int height, int level, int numRoom, bool newWantBoss = false, bool newWantMiniBoss = false)
    {
        _x_pos = x;
        _y_pos = y;
        _tilewidth = width;
        _tileheight = height;

        _wantBoss = newWantBoss;
        _wantMiniBoss = newWantMiniBoss;

        _mylevel = level;

        _mynumRoom = numRoom;

        mydoors = doors;

        _enemiesInRoom = new List<EnemiesRoom>();
        _possibleEnemies = new List<EnemiesRoom>();


        if (_mylevel != 0 || _mynumRoom != 0)
        {

            if (!_wantBoss && !_wantMiniBoss)
            {
                SetPosiblePointsEnemies();

                SetEnemies();

                numEnemyTypes = new int[_possibleEnemies.Count];

                ClearEnemyTypeArray();

                //SpawnEnemies();
                SpawnEnemiesIterativeMode();
            }
            else if(_wantBoss  && !_wantMiniBoss)
            {
                _enemiesInRoomCount++;

                _enemyPositions = new List<EnemyPos>();

                float _middleDownPosition = ((_tileheight) / 6) * 2;
                _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + _middleDownPosition, 0), EnemyPositionEnum.MIDDLE_DOWNMIDDLE_POS));

                _possibleEnemies.Add(new EnemiesRoom(Enemy_type.MACLIR_ENEMY, ProceduralDungeonGenerator.mapGenerator.macLir, 5, 4));

                GameObject go = Instantiate(_possibleEnemies[0].myEnemy);
                go.transform.position = _enemyPositions[0]._position;
                _enemiesInRoom.Add(new EnemiesRoom(_possibleEnemies[0].myEnemyType, go, _possibleEnemies[0].mydificultyPoints));
            }
            else if (!_wantBoss && _wantMiniBoss)
            {
                _enemiesInRoomCount++;
                _enemyPositions = new List<EnemyPos>();

                float _middleDownPosition = ((_tileheight) / 6) * 2;
                _enemyPositions.Add(new EnemyPos(new Vector3(_x_pos + _tilewidth * 0.5f, _y_pos + _middleDownPosition, 0), EnemyPositionEnum.MIDDLE_DOWNMIDDLE_POS));

                _possibleEnemies.Add(new EnemiesRoom(Enemy_type.KELPIE_ENEMY, ProceduralDungeonGenerator.mapGenerator.kelpie, 5, 4));

                GameObject go = Instantiate(_possibleEnemies[0].myEnemy);
                go.transform.position = _enemyPositions[0]._position;
                _enemiesInRoom.Add(new EnemiesRoom(_possibleEnemies[0].myEnemyType, go, _possibleEnemies[0].mydificultyPoints));
            }

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

        if (ProceduralDungeonGenerator.mapGenerator.numDungeon == 0 ||
            ProceduralDungeonGenerator.mapGenerator.numDungeon == 1)
        {
            _possibleEnemies.Add(new EnemiesRoom(Enemy_type.MEELE_ENEMY, ProceduralDungeonGenerator.mapGenerator.meleeEnemey, 5, 6));
            _possibleEnemies.Add(new EnemiesRoom(Enemy_type.CARTONACH_ENEMY, ProceduralDungeonGenerator.mapGenerator.caorthannach, 5, 4));
            _possibleEnemies.Add(new EnemiesRoom(Enemy_type.CARTONACH_ENEMY, ProceduralDungeonGenerator.mapGenerator.bluecaorthannach, 5, 4));
            _possibleEnemies.Add(new EnemiesRoom(Enemy_type.DEARDUG_ENEMY, ProceduralDungeonGenerator.mapGenerator.dearDug, 5, 3));
        }
        //Need to add something to detect if is the dungeon you want
        if (ProceduralDungeonGenerator.mapGenerator.numDungeon == 1)
        {
            _possibleEnemies.Add(new EnemiesRoom(Enemy_type.BANSHEE_ENEMY, ProceduralDungeonGenerator.mapGenerator.banshee, 7, 2));
        }

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
                    _counterPoints += enemy.mydificultyPoints;
                }
            }

            if (_counterPoints == _maximumPoints)
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

    void SpawnEnemiesIterativeMode()
    {

        int counterType = 0;
        int counterTotal = 0;

        while (counterTotal != _possibleEnemies.Count) {

            do
            {
                counterType = UnityEngine.Random.Range(0, _possibleEnemies.Count);
            } while (IsEnemyTypeUsed(counterType));

            numEnemyTypes[counterTotal] = counterType;

            counterTotal++;

            EnemiesRoom enemy = _possibleEnemies[counterType];


            if (_counterPoints == _maximumPoints)
                break;

            int numOfEnemyType = UnityEngine.Random.Range(0, enemy.maxEnemiesOfType + 1);
            for (int i = 1; i < numOfEnemyType; i++)
            {

                EnemyPos enemyPos = GetPositionToSpawnNotUsed();

                if (enemyPos == null && _counterPoints + enemy.mydificultyPoints > _maximumPoints)
                    break;

                GameObject go = Instantiate(enemy.myEnemy);
                go.transform.position = enemyPos._position;
                _enemiesInRoom.Add(new EnemiesRoom(enemy.myEnemyType, go, enemy.mydificultyPoints));
                _counterPoints += enemy.mydificultyPoints;

                if (_counterPoints >= _maximumPoints)
                    break;

            }
            if (_counterPoints >= _maximumPoints)
                break;
        }


        while(_enemiesInRoom.Count == 0)
        {
            ClearEnemyTypeArray();
            SpawnEnemiesIterativeMode();
        }

        _enemiesInRoomCount = _enemiesInRoom.Count;

    }

    bool IsEnemyTypeUsed(int it)
    {
        bool ret = false;
        for(int i=0; i< numEnemyTypes.Length; i++)
        {
            if(numEnemyTypes[i] == it)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    void ClearEnemyTypeArray()
    {
        for (int i = 0; i < numEnemyTypes.Length; i++)
        {
            numEnemyTypes[i] = -1;
        }
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
                ProceduralDungeonGenerator.mapGenerator.ActivateDeactivateDoors(true);
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

        if (AreAllPositionsUsed())
            return null;

        do
        {
            randPos = UnityEngine.Random.Range(0, _enemyPositions.Count);
        } while (_enemyPositions[randPos]._alreadyUsed);

        _enemyPositions[randPos]._alreadyUsed = true;
        ret = _enemyPositions[randPos];

        return ret;
    }

    bool AreAllPositionsUsed()
    {
        int ret = 0;

        foreach (EnemyPos pos in _enemyPositions)
        {
           if(pos._alreadyUsed)
            {
                ret++;
            }
        }

        return ret == _enemyPositions.Count;
    }

    void DetectEnemyLives()
    {
        Blackboard bb;



        for(int i=0; i< _enemiesInRoom.Count; i++)
        {

            if (_enemiesInRoom[i] == null)
                continue;

            EnemiesRoom enemiesRoom = _enemiesInRoom[i];


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
                case Enemy_type.KELPIE_ENEMY:
                    bb = enemiesRoom.myEnemy.transform.GetChild(0).GetComponent<Kelpi_Blackboard>();
                    if (bb != null)
                    {
                        if (((Kelpi_Blackboard)bb).life.myValue <= 0)
                        {
                            _enemiesInRoom.Remove(enemiesRoom);
                            _enemiesDead++;
                        }
                    }
                    break;
                case Enemy_type.MACLIR_ENEMY:
                    bb = enemiesRoom.myEnemy.transform.GetChild(0).GetComponent<MacLir_Blackboard>();
                    if (bb != null)
                    {
                        if (((MacLir_Blackboard)bb).life.myValue <= 0)
                        {
                            _enemiesInRoom.Remove(enemiesRoom);
                            _enemiesDead++;
                        }
                    }
                    break;
                case Enemy_type.DEARDUG_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponent<DearDug_Blackboard>();
                    if (bb != null)
                    {
                        if (((DearDug_Blackboard)bb).life.myValue <= 0)
                        {
                            _enemiesInRoom.Remove(enemiesRoom);
                            _enemiesDead++;
                        }
                    }
                    break;
                case Enemy_type.BANSHEE_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponent<Banshee_Blackboard>();
                    if (bb != null)
                    {
                        if (((Banshee_Blackboard)bb).life.myValue <= 0)
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
                case Enemy_type.KELPIE_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponentInChildren<Kelpi_Blackboard>();
                    if (bb != null)
                    {
                        ((Kelpi_Blackboard)bb).playerIsInsideRoom.myValue = true;
                    }
                    break;
                case Enemy_type.MACLIR_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponentInChildren<MacLir_Blackboard>();
                    if (bb != null)
                    {
                        ((MacLir_Blackboard)bb).playerIsInsideRoom.myValue = true;
                    }
                    break;
                case Enemy_type.DEARDUG_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponentInChildren<DearDug_Blackboard>();
                    if (bb != null)
                    {
                        ((DearDug_Blackboard)bb).playerIsInsideRoom.myValue = true;
                    }
                    break;
                case Enemy_type.BANSHEE_ENEMY:
                    bb = enemiesRoom.myEnemy.GetComponentInChildren<Banshee_Blackboard>();
                    if (bb != null)
                    {
                        ((Banshee_Blackboard)bb).playerIsInsideRoom.myValue = true;
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
                ProceduralDungeonGenerator.mapGenerator.ActivateDeactivateDoors(false);
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
