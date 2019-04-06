using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Parameter : System.Object
{
    [SerializeField] private string my_name;

    public string myName
    {
        get { return my_name; }
        set { my_name = value; }
    }

    virtual public object GetValue()
    {
        return null;
    }

    virtual public void SetValue(object value)
    {
    }
}

[System.Serializable]
public class ParameterInt : Parameter
{
    [SerializeField] private int my_value;

    public int myValue
    {
        get { return my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (int)value;
    }
}

[System.Serializable]
public class ParameterFloat : Parameter
{
    [SerializeField] private float my_value;

    public float myValue
    {
        get { return my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (float)value;
    }
}

[System.Serializable]
public class ParameterBool : Parameter
{
    [SerializeField] private bool my_value;

    public bool myValue
    {
        get { return my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (bool)value;
    }
}

[System.Serializable]
public class ParameterTransform : Parameter
{
    [SerializeField] private Transform my_value = null;

    public Transform myValue
    {
        get { return (my_value == null) ? null : my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (Transform)value;
    }
}

[System.Serializable]
public class ParameterBoxCollider2D : Parameter
{
    [SerializeField] private BoxCollider2D my_value;

    public BoxCollider2D myValue
    {
        get { return my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (BoxCollider2D)value;
    }
}

[System.Serializable]
public class ParameterGameObject : Parameter
{
    [SerializeField] private GameObject my_value;

    public GameObject myValue
    {
        get { return my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (GameObject)value;
    }
}

[System.Serializable]
public class ParameterSpriteRend : Parameter
{
    [SerializeField] private SpriteRenderer my_value;

    public SpriteRenderer myValue
    {
        get { return my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (SpriteRenderer)value;
    }
}

[System.Serializable]
public class ParameterAnimator: Parameter
{
    [SerializeField] private Animator my_value;

    public Animator myValue
    {
        get { return my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (Animator)value;
    }
}

public enum Direction
{
    RIGHT,
    LEFT,
    UP,
    DOWN,
    NEUTRAL
}

[System.Serializable]
public class ParameterEnumDirection : Parameter
{
    [SerializeField] private Direction my_value;

    public Direction myValue
    {
        get { return my_value; }
        set { my_value = value; }
    }

    override public object GetValue()
    {
        return myValue;
    }

    override public void SetValue(object value)
    {
        myValue = (Direction)value;
    }
}