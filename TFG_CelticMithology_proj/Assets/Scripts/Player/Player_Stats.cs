using UnityEngine.UI;
using UnityEngine;

public class Player_Stats: MonoBehaviour
{

    int wood_material = 0;
    int iron_material = 0;
    int silver_material = 0;
    int diamond_material = 0;

    Object_InGame Chest_Object;
    Object_InGame Head_Object;
    Object_InGame Right_Hand_Object;
    Object_InGame Left_Hand_Object;

    Object_InGame Right_Hand_Object_Secondary_1;
    Object_InGame Right_Hand_Object_Secondary_2;
    Object_InGame Left_Hand_Object_Secondary_1;
    Object_InGame Left_Hand_Object_Secondary_2;

    Object_InGame[] Player_Inventory_Objects;
    public Object_InGame[] All_Game_Objects;

    public void StartStats()
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
