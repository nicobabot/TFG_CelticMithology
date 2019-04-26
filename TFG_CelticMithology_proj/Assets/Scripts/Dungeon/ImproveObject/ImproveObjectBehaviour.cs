using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImproveObjectBehaviour : MonoBehaviour {

    [SerializeField] private ImproveManager improvePlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player_combat_collider"))
        {
            ImproveManager improveManager = collision.transform.parent.GetComponent<ImproveManager>();
            improveManager.AddSliceBar();
            Destroy(gameObject);
        }
    }

}
