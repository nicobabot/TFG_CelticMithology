using UnityEngine;

public class Hole_Collision : MonoBehaviour
{
    public Player_Manager player_scr;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player_falling_collider")
        {
            player_scr.current_state = Player_Manager.Player_States.FALLING_PLAYER;
        }
    }
}