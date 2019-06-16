using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImproveManager : MonoBehaviour {

    public EventSystem eventSystem;
    [SerializeField] private int objToImprove = 5;
    public Image improveBar;
    public GameObject buttonSelected;
    [SerializeField] private GameObject panelImprove;
    [HideInInspector]public bool isImproving = false;
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
            isImproving = true;
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

        if (improveBar.fillAmount == 1.0f)
        {
            eventSystem.SetSelectedGameObject(buttonSelected);
        }

    }

    public void ResetBar()
    {
        isImproving = false;
        improveBar.fillAmount = 0.0f;
    }

    public bool CanImprove()
    {
        return improveBar.fillAmount == 1.0f;
    }

}
