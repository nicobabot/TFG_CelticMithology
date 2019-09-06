using System.Collections.Generic;
using UnityEngine;

public class Action_FollowPlayer : ActionBase
{
    public bool use_pathfinding = true;
    public float time_to_change_cell = 1.0f;
    public float speed = 5.0f;
    private float timer_changing = 0.0f;
    private int cells_changed = 0;
    private List<PathNode> tiles_list;

    //Values calculate path
    private Vector3 destiny_pos = Vector3.zero;

    private Vector3Int cell_destiny_pos = Vector3Int.zero;
    private Vector3Int cell_pos = Vector3Int.zero;
    private PathNode actual_node;
    private GameObject player;

    private SpriteRenderer mySpriteRend;
    private Animator myAnimator;

    private float timeWaiting = 0.65f;
    private float timerWait = 0.0f;
    private bool waitOnce = false;
    private bool canMove = true;


    bool can_reach = true;

    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_FollowPlayer");
        }

        mySpriteRend = (SpriteRenderer)myBT.myBB.GetParameter("mySpriteRend");

        myAnimator = (Animator)myBT.myBB.GetParameter("myAnimator");

        timerWait = 0.0f;

        if (myAnimator != null && myBT.enemy_type != Enemy_type.KELPIE_ENEMY)
        {
            myAnimator.SetBool("enemy_startwalking", true);
            myAnimator.SetFloat("offsetAnimation", Random.Range(0.0f, 1.0f));
        }

        can_reach = Recalculate_Path();
        return BT_Status.RUNNING;
    }

    override public BT_Status UpdateAction()
    {


        if (use_pathfinding)
        {
            Vector3 new_position = Vector3.zero;


            if (myBT.pathfinder_scr.walkability.LocalToCell(player.transform.position) != cell_destiny_pos)
            {
                can_reach = Recalculate_Path();
            }

            if (cells_changed < tiles_list.Count)
            {
                actual_node = tiles_list[cells_changed];
                int x_tile = actual_node.GetTileX();
                int y_tile = actual_node.GetTileY();
                new_position = myBT.pathfinder_scr.walkability.CellToLocal(new Vector3Int(x_tile, y_tile, 0));

                transform.position = Vector3.MoveTowards(transform.position, new_position, speed * Time.deltaTime);

                if (transform.position == new_position)
                {
                    cells_changed++;
                }
            }
            else
            {

                if (myBT.pathfinder_scr.walkability.LocalToCell(player.transform.position) != cell_destiny_pos)
                {
                    can_reach = Recalculate_Path();
                }

                if (can_reach)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }
            }

            DetectDirection(transform.position, new_position);

        }
        else
        {

            //can_reach = Recalculate_Path();
            //if (myBT.pathfinder_scr.walkability.LocalToCell(player.transform.position) != cell_destiny_pos)
            //{
            //    can_reach = Recalculate_Path();
            //}

            //if (can_reach)
            if ((myBT.enemy_type == Enemy_type.MEELE_ENEMY || myBT.enemy_type == Enemy_type.MORRIGAN_ENEMY) && !waitOnce)
            {
               
                canMove = WaitFirstAnimationFinished();

                if(canMove)
                    waitOnce = true;
            }

            if (canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

                Direction mydir = DetectDirection(transform.position, player.transform.position);

                if(myBT.enemy_type == Enemy_type.MACLIR_ENEMY)
                {                 
                    myAnimator.SetFloat("direction", (float)mydir);
                }

                if(mySpriteRend == null)
                    Debug.Log("Rend = null PLAYUer");

                if (mydir == Direction.RIGHT && mySpriteRend != null)
                {
                    if(myBT.enemy_type == Enemy_type.BANSHEE_ENEMY)
                        mySpriteRend.flipX = true;
                    else mySpriteRend.flipX = false;
                }
                else if (mydir == Direction.LEFT && mySpriteRend != null)
                {
                    if (myBT.enemy_type == Enemy_type.BANSHEE_ENEMY)
                        mySpriteRend.flipX = false;
                    else mySpriteRend.flipX = true;
                }
            }

        }

        

        return BT_Status.RUNNING;
    }

    bool WaitFirstAnimationFinished()
    {
        bool ret = false;

        timerWait += Time.deltaTime;

        if (timerWait>= timeWaiting)
        {
            ret = true;
            //timerWait = 0.0f;
        }

        return ret;
    }

    public bool Recalculate_Path()
    {
        bool can_reach = true;

        if (use_pathfinding)
        {
            can_reach = false;

            if (tiles_list != null)
                tiles_list.Clear();

            timer_changing = 0;
            cells_changed = 0;

            destiny_pos = player.transform.position;

            cell_destiny_pos = myBT.pathfinder_scr.walkability.LocalToCell(destiny_pos);

            cell_pos = myBT.pathfinder_scr.walkability.LocalToCell(transform.position);

            tiles_list = myBT.pathfinder_scr.CalculatePath(new PathNode(cell_pos.x, cell_pos.y), new PathNode(cell_destiny_pos.x, cell_destiny_pos.y), out can_reach);

        }
        return can_reach;
    }

    public Direction DetectDirection(Vector3 my_position, Vector3 new_position)
    {
        Direction dir_x = Direction.NEUTRAL;
        Direction dir_y = Direction.NEUTRAL;

        float x = my_position.x;
        float y = my_position.y;

        if (x < new_position.x) dir_x = Direction.RIGHT;
        else dir_x = Direction.LEFT;

        if (y < new_position.y) dir_y = Direction.UP;
        else dir_y = Direction.DOWN;

        float dif_x = Mathf.Abs(x - new_position.x);
        float dif_y = Mathf.Abs(y - new_position.y);

        if (dif_x > dif_y)
        {
            dir_y = Direction.NEUTRAL;
            //Debug.Log("Going " + dir_x.ToString());
            myBT.myBB.SetParameter("direction", dir_x);
            return dir_x;
        }
        else
        {
            dir_x = Direction.NEUTRAL;
            //Debug.Log("Going " + dir_y.ToString());
            myBT.myBB.SetParameter("direction", dir_y);
            return dir_y;
        }
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}