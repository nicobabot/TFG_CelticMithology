using UnityEngine.UI;
using UnityEngine;

public enum Material_InGame
{
    WOOD_MATERIAL=0,
    IRON_MATERIAL,
    SILVER_MATERIAL,
    DIAMOND_MATERIAL
}
public class Player_Stats: MonoBehaviour
{

    uint p_wood_material = 0;
    public uint Wood_Material
    {
        get { return p_wood_material; }
        set { p_wood_material = value; }
    }

    uint p_iron_material = 0;
    public uint Iron_Material
    {
        get { return p_iron_material; }
        set { p_iron_material = value; }
    }

    uint p_silver_material = 0;
    public uint Silver_Material
    {
        get { return p_silver_material; }
        set { p_silver_material = value; }
    }

    uint p_diamond_material = 0;
    public uint Diamond_Material
    {
        get { return p_diamond_material; }
        set { p_diamond_material = value; }
    }

    Object_InGame item_Chest_Object;
    public Object_InGame Chest_Object
    {
        get { return item_Chest_Object; }
        set { item_Chest_Object = value; }
    }

    Object_InGame item_Head_Object;
    public Object_InGame Head_Object
    {
        get { return item_Head_Object; }
        set { item_Head_Object = value; }
    }

    Object_InGame item_Right_Hand_Object;
    public Object_InGame Right_Hand_Object
    {
        get { return item_Right_Hand_Object; }
        set { item_Right_Hand_Object = value; }
    }

    Object_InGame item_Left_Hand_Object;
    public Object_InGame Left_Hand_Object
    {
        get { return item_Left_Hand_Object; }
        set { item_Left_Hand_Object = value; }
    }

    Object_InGame Right_Hand_Object_Secondary_1;
    Object_InGame Right_Hand_Object_Secondary_2;
    Object_InGame Left_Hand_Object_Secondary_1;
    Object_InGame Left_Hand_Object_Secondary_2;

    Object_InGame[] Player_Inventory_Objects;
    public Object_InGame[] All_Game_Objects;

    private void Start()
    {
        Player_Inventory_Objects = new Object_InGame[8];
        //Pushback to all objects of the game

        Right_Hand_Object = All_Game_Objects[0];

    }

}

public enum ObjectType
{
    CHEST_OBJECT,
    RIGHT_HAND_OBJECT,
    LEFT_HAND_OBJECT,
    HEAD_OBJECT,
}

[System.Serializable]
public class Object_InGame
{
    public string name;
    public Sprite object_image;
    public ObjectType type;
    public int damage;
    public int duration;

    Object_InGame(string name_n, Sprite sprite_object, ObjectType n_type, int n_damage, int n_duration)
    {
        name = name_n;
        object_image = sprite_object;
        type = n_type;
        damage = n_damage;
        duration = n_duration;
    }
}
