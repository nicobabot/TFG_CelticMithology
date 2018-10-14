using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour {


    protected List<Parameter> list;

    // Use this for initialization
    virtual public void Start () {
        list = new List<Parameter>();


    }


    public object GetParameter(string parameter_name)
    {
        for(int i=0;i<list.Count;i++)
        {
            if(list[i].myName.Equals(parameter_name))
            {
                return list[i].GetValue();
            }
        }
        return null;
    }

    public void SetParameter(string parameter_name,object value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].myName.Equals(parameter_name))
            {
                list[i].SetValue(value);
            }
        }
    }
}
