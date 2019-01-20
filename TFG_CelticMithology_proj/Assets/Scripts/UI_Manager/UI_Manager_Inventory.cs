using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager_Inventory : MonoBehaviour {

    public Player_Stats player_stats;

    public GameObject[] primery_objects;
    public GameObject mine_items_parent;
    public Dropdown drop_down_item;

    int num_of_materials;
    int num_of_principal_objects;

	// Use this for initialization
	void Start () {

        num_of_materials = mine_items_parent.transform.childCount;
        num_of_principal_objects = mine_items_parent.transform.childCount;

    }
	
	// Update is called once per frame
	void Update () {
		
        //Materials Update
        for(int i=0; i < num_of_materials; i++)
        {
            Transform trans = mine_items_parent.transform.GetChild(i);
            Text num_of_objects = trans.GetChild(0).GetComponent<Text>();
            num_of_objects.text = "x";
            switch (i)
            {
                case (int)Material_InGame.WOOD_MATERIAL:
                    num_of_objects.text += player_stats.Wood_Material.ToString();
                    break;
                case (int)Material_InGame.IRON_MATERIAL:
                    num_of_objects.text += player_stats.Iron_Material.ToString();
                    break;
                case (int)Material_InGame.SILVER_MATERIAL:
                    num_of_objects.text += player_stats.Silver_Material.ToString();
                    break;
                case (int)Material_InGame.DIAMOND_MATERIAL:
                    num_of_objects.text += player_stats.Diamond_Material.ToString();
                    break;
            }
        }

        //Inventory Update

        // 0 - Chest
        if (player_stats.Chest_Object != null)
        {
            GameObject chest_obj = primery_objects[0];
            Button button_chest = chest_obj.GetComponent<Button>();
            button_chest.GetComponentInChildren<Text>().text = player_stats.Chest_Object.name;
        }
        // 1 - Head
        if (player_stats.Head_Object != null)
        {
            GameObject head_obj = primery_objects[1];
            Button button_head= head_obj.GetComponent<Button>();
            button_head.GetComponentInChildren<Text>().text = player_stats.Head_Object.name;
        }
        // 2 - Right Hand
        if (player_stats.Right_Hand_Object != null)
        {
            GameObject right_hand_obj = primery_objects[2];
            Button button_right_hand = right_hand_obj.GetComponent<Button>();
            button_right_hand.GetComponentInChildren<Text>().text = player_stats.Right_Hand_Object.name;
        }
        // 3 - Left Hand
        if (player_stats.Left_Hand_Object != null)
        {
            GameObject left_hand_obj = primery_objects[2];
            Button button_left_hand = left_hand_obj.GetComponent<Button>();
            button_left_hand.GetComponentInChildren<Text>().text = player_stats.Left_Hand_Object.name;
        }

    }
}
