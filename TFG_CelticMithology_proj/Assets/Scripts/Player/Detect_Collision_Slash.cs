using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Detect_Collision_Slash : MonoBehaviour
{
    public LayerMask enemy_layer;
    public LayerMask material_layer;
    private GameObject enemy_collided = null;
    private BoxCollider2D collider;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public Collider2D[] Is_Enemy_Collided()
    {
        collider = GetComponent<BoxCollider2D>();
        Collider2D[] enemies_found = Physics2D.OverlapBoxAll(transform.position, collider.size, 0.0f, enemy_layer);

        return enemies_found;
    }

    public Collider2D Is_Material_Collided()
    {
        collider = GetComponent<BoxCollider2D>();
        Collider2D material_found = Physics2D.OverlapBox(transform.position, collider.size, 0.0f, material_layer);

        return material_found;
    }
}