using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkeletonDetection : MonoBehaviour {

    public LayerMask playerLayer;
    private Morrigan_Blackboard morrBB;
    private CircleCollider2D collider;
    private SpriteRenderer spriteRend;
    private Collider2D foundPlayer;

    private float timeDoingAnim = 5.0f;
    private float timer = 0;

	// Use this for initialization
	void Start ()
    {
        collider = GetComponent<CircleCollider2D>();
        spriteRend = gameObject.GetComponentInChildren<SpriteRenderer>();
        foundPlayer = null;
        timer = 0.0f;

        spriteRend.DOFade(1.0f, timeDoingAnim).OnComplete(() => DoAction());

    }
	
	// Update is called once per frame
	void Update () {

        //timer += Time.deltaTime;
        
       /* if (timer >= timeDoingAnim)
        {
            Is_Material_Collided();
            Destroy(gameObject);
        }*/

    }

    void DoAction()
    {
        Is_Material_Collided();
        Destroy(gameObject);
    }

    public void SetMorriganBB(Morrigan_Blackboard myMorrigan)
    {
        morrBB = myMorrigan;
    }

    public void Is_Material_Collided()
    {
        foundPlayer = Physics2D.OverlapCircle(transform.localPosition, collider.radius, playerLayer);

        if (foundPlayer!=null)
        {
            morrBB.SetParameter("invSkelPlayerDet", true);
        }
    }

}
