using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour {

    [SerializeField] private bool is_interruptible;
    private BT_Entity my_bt;

    private bool is_finish = false;

    public string name = "action";

    public bool isInterruptible
        {
        get { return is_interruptible; }
        }
    public BT_Entity myBT
    {
        get { return my_bt; }
        set { my_bt = value; }
    }
    public bool isFinish
    {
        get { return is_finish; }
        set { if(!isInterruptible)is_finish =value; }

    }

    private void Start()
    {
        myBT = GetComponent<BT_Entity>();
    }

    virtual public BT_Status StartAction()
    {
        return BT_Status.RUNNING;
    }

    virtual public BT_Status UpdateAction()
    {
        return BT_Status.RUNNING;
    }
    virtual public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}
