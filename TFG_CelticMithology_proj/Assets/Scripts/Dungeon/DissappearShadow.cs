using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DissappearShadow : MonoBehaviour {

    public float speedFade=0.25f;
    SpriteRenderer spriteRend;

	// Use this for initialization
	void Start () {
        spriteRend = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRend.DOFade(0.0f, speedFade).OnComplete(() => Destroy(gameObject));
    }

}
