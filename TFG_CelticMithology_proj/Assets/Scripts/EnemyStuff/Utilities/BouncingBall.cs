using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouncingBall : MonoBehaviour {

    public GameObject Player;
    public Rigidbody2D rb;
    public float speed;
    public float numRebaunds = 5;
    public LayerMask mask;
    Vector3 dir = Vector3.zero;
    //Collider2D col;
    RaycastHit2D ray;
    float rebaundsCount = 0;
    Vector3 rightVec;

    public void SetDirection(Vector3 newDir)
    {
        dir = newDir;
        dir = dir.normalized * speed;

       /* Vector3 tempDir = newDir.normalized * speed;
        dir = tempDir - transform.position;*/
    }

    // Use this for initialization
    void Start () {
        Player = ProceduralDungeonGenerator.mapGenerator.Player;
        SelectDirection();
        rebaundsCount = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (rebaundsCount == numRebaunds)
        {
            Destroy(gameObject);
        }
        /*else if(rebaundsCount < numRebaunds)
        {
            transform.position = Vector3.MoveTowards(transform.position, dir, Time.deltaTime * speed);
        }*/

	}

    void SelectDirection()
    {
        /*dir = Player.transform.position - transform.position;

        ray = Physics2D.Raycast(transform.position, dir, mask);

        dir = dir.normalized * speed;*/


        rb.AddForce(dir);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("door"))
        {
            rebaundsCount++;
        }
    }

    /* private void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.CompareTag("wall") || collision.CompareTag("door"))
         {
             if (ray != null)
             {
                 col = collision;
                 dir = Vector2.Reflect(dir, ray.normal);

                 ray = Physics2D.Raycast(transform.position, dir, mask);

                 rebaundsCount++;
             }
         }
}*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, dir);
        /*if (ray != null && col!=null)
        {
            Gizmos.DrawLine(col.transform.position, ray.normal);
        }*/
    }

}
