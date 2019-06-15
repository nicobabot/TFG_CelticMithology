using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImproveManager : MonoBehaviour {

    [SerializeField] private int objToImprove = 5;
    public Image improveBar;
    [SerializeField] private GameObject panelImprove;
    private Player_Manager playerManager;
    private float sliceBar=0;

	// Use this for initialization
	void Start () {

        playerManager = gameObject.GetComponent<Player_Manager>();

        sliceBar = 1.0f / objToImprove;
    }

    private void Update()
    {
        if(improveBar.fillAmount == 1)
        {
            playerManager.SetPlayerInPause();
            panelImprove.SetActive(true);
        }
    }

    public void AddSliceBar()
    {
        if (improveBar.fillAmount < 1.0f)
        {
            improveBar.fillAmount += sliceBar;
        }
    }

    public void ResetBar()
    {
        improveBar.fillAmount = 0.0f;
    }

    public bool CanImprove()
    {
        return improveBar.fillAmount == 1.0f;
    }

}
