using UnityEngine;
using UnityEngine.UI;

public class Action_PushBack : ActionBase
{
    public float push_distance = 1.0f;
    public float time_doing_pushback = 0.5f;
    public float min_distance = 0.1f;
    public LayerMask layer_ray;
    public Color damaged_color;

    private float timer_pushback;

    private GameObject player;

    private Color regular_color;

    private Vector3 pushback_dir = Vector3.zero;
    private Vector3 temp_position;
    private Vector3 temp_position_player;
    private Vector3 hitpoint_wall;
    private Vector3 pushback_point = Vector3.zero;

    private Rigidbody2D rb;

    private SpriteRenderer sprite_rend;


    override public BT_Status StartAction()
    {
        player = (GameObject)myBT.myBB.GetParameter("player");
        if (player == null)
        {
            Debug.Log("<color=red> Player not found!_Action_PushBack");
        }

        rb = gameObject.GetComponent<Rigidbody2D>();

        //Reverse direction to make the pushback
        sprite_rend = GetComponent<SpriteRenderer>();
        if (sprite_rend == null)
        {
            Debug.Log("Sprite renderer null _Action_PushBack");
        }

        regular_color = sprite_rend.color;

        float size_addition = (sprite_rend.bounds.size.y * 0.5f);
        temp_position = transform.position;
        temp_position.y += size_addition;

        SpriteRenderer sprite_rend_player = player.GetComponent<SpriteRenderer>();
        if (sprite_rend_player == null)
        {
            Debug.Log("Sprite renderer null _Action_PushBack");
        }

        float size_addition_player = (sprite_rend_player.bounds.size.y * 0.5f);
        temp_position_player = player.transform.position;
        //temp_position_player.y += size_addition_player;

        pushback_dir = temp_position - temp_position_player;

        if ((Direction)myBT.myBB.GetParameter("direction") == Direction.UP)
        {
            pushback_dir = -pushback_dir;
        }

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, pushback_dir.normalized, push_distance, layer_ray);
        if (hit != null)
        {
            foreach (RaycastHit2D hit_t in hit)
                if (hit_t.transform.CompareTag("wall"))
                {
                    hitpoint_wall = hit_t.point;
                    break;
                }
        }

        pushback_dir = pushback_dir.normalized * push_distance;
        pushback_point = transform.position + pushback_dir;

        timer_pushback = 0.0f;

        return BT_Status.RUNNING;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + pushback_dir);
        Gizmos.DrawWireSphere(hitpoint_wall, min_distance);
        Gizmos.DrawWireSphere(temp_position, 0.1f);
        Gizmos.DrawWireSphere(temp_position_player, 0.1f);
        /*Gizmos.DrawWireSphere(temp_position, 0.1f);
        Gizmos.DrawWireSphere(temp_position_player, 0.1f);*/
    }

    override public BT_Status UpdateAction()
    {

        sprite_rend.color = damaged_color;

        Vector3 direction = transform.position - hitpoint_wall;

        if (direction.magnitude < min_distance)
        {
            sprite_rend.color = regular_color;
            rb.velocity = Vector2.zero;
            myBT.myBB.SetParameter("is_enemy_hit", false);
            isFinish = true;
            return BT_Status.RUNNING;
        }

        float step = Time.deltaTime * push_distance;

        transform.position = Vector3.MoveTowards(transform.position, pushback_point, step);

        timer_pushback += Time.deltaTime;

        if (timer_pushback > time_doing_pushback)
        {
            sprite_rend.color = regular_color;
            myBT.myBB.SetParameter("is_enemy_hit", false);
            isFinish = true;
        }

        return BT_Status.RUNNING;
    }

    override public BT_Status EndAction()
    {
        return BT_Status.SUCCESS;
    }
}