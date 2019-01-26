using UnityEngine;

public enum BT_Status
{
    WAITING,
    RUNNING,
    SUCCESS,
    FAIL,
    ERROR,
}

public class BT_Entity : MonoBehaviour
{
    private ActionBase current_action = null;
    [SerializeField] private Blackboard blackboard = null;
    public string Action_name = "";
    public Pathfinder pathfinder_scr;

    private BT_Status status = BT_Status.WAITING;

    public ActionBase currentAction
    {
        get { return current_action; }
        set
        {
            if (currentAction != null)
            {
                currentAction.isFinish = false;
            }

            current_action = value;

            if (currentAction != null)
            {
                Action_name = currentAction.name;
            }
        }
    }

    public BT_Status myStatus
    {
        get { return status; }
        set
        {
            if (status != value)
            {
                status = value;
                StatusChange();
            }
        }
    }

    public Blackboard myBB
    {
        get { return blackboard; }
    }

    // Use this for initialization
    virtual public void Start()
    {
    }

    // Update is called once per frame
    virtual public void Update()
    {
        BT_Status current_status = myStatus;

        if (current_action == null)
        {
            if (MakeDecision())
            {
                current_status = current_action.StartAction();
                if (current_action.isFinish)
                {
                    current_status = EndAction();
                }
            }
            EndUpdate(current_status);
            return;
        }

        if (myStatus == BT_Status.WAITING)
        {
            if (MakeDecision())
            {
                current_status = current_action.StartAction();
                if (current_action.isFinish)
                {
                    current_status = EndAction();
                }
            }
            EndUpdate(current_status);
            return;
        }
        if (current_action.isInterruptible)
        {
            if (MakeDecision())
            {
                current_status = current_action.StartAction();
                if (current_action.isFinish)
                {
                    current_status = EndAction();
                }
                EndUpdate(current_status);
                return;
            }
        }
        if (current_action.isFinish)
        {
            current_status = EndAction();
            EndUpdate(current_status);
            return;
        }
        current_status = current_action.UpdateAction();
        EndUpdate(current_status);
    }

    private void EndUpdate(BT_Status current_status)
    {
        myStatus = current_status;
    }

    private void StatusChange()
    {
        switch (myStatus)
        {
            case BT_Status.WAITING:
                //Debug.Log("Waiting for action");
                break;

            case BT_Status.RUNNING:
                //Debug.Log("Action start running");
                break;

            case BT_Status.SUCCESS:
                //Debug.Log("Action Succes change to waitinh");
                ResetAction();
                break;

            case BT_Status.FAIL:
                //Debug.Log("Action Fail change to waiting");
                ResetAction();
                break;

            case BT_Status.ERROR:
                //Debug.Log("Action Error something bad happend :(((");
                ResetAction();
                break;
        }
    }

    private BT_Status EndAction()
    {
        if (currentAction == null)
            return BT_Status.ERROR;
        return currentAction.EndAction();
    }

    private void ResetAction()
    {
        currentAction = null;
        status = BT_Status.WAITING;
    }

    virtual public bool MakeDecision()
    {
        return true;
    }

    virtual public void Enemy_Live_Modification(int num)
    {
        int live = (int)blackboard.GetParameter("live");
        live += num;
        blackboard.SetParameter("live", live);
        blackboard.SetParameter("is_enemy_hit", false);
    }

}