using UnityEngine;

[RequireComponent(typeof(Player_Manager))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player_PushBack : MonoBehaviour
{
    public float pushback_force;
    public float time_doing_pushback = 0.1f;
    [HideInInspector]public Transform enemy_pos;
    private float timer_pushback = 0.0f;
    private Player_Manager player_manager_sct;
    private Rigidbody2D rb;

    // Use this for initialization
    private void Start()
    {
        player_manager_sct = GetComponent<Player_Manager>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void PushBack_Update()
    {
        if (enemy_pos == null)
        {
            Reset_Values();
            return;
        }

        Vector3 dir_push = transform.position - enemy_pos.position;
        dir_push = dir_push.normalized * pushback_force;

        timer_pushback += Time.deltaTime;

        rb.velocity = dir_push;

        if (timer_pushback > time_doing_pushback)
        {
            Reset_Values();
        }
    }

    void Reset_Values()
    {
        if (player_manager_sct.noNeedInvulnerable)
        {
            player_manager_sct.noNeedInvulnerable = false;
            player_manager_sct.is_invulnerable = false;
        }

        rb.velocity = Vector2.zero;
        timer_pushback = 0;
        enemy_pos = null;
        player_manager_sct.current_state = Player_Manager.Player_States.IDLE_PLAYER;
    }

}