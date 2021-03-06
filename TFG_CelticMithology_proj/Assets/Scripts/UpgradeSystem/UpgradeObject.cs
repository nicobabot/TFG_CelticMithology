﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpgradeObject : MonoBehaviour {

    [SerializeField] private GameObject UpgradeCanvas;
    [SerializeField] private GameObject myCanvas;
    [SerializeField] private EventSystem myeventSystem;
    [SerializeField] private GameObject nextGameObjectSelected;

    [Header("Parent Error")]
    [SerializeField] private TextMeshProUGUI ErrorText;
    [SerializeField] private Image ArrowGO;

    [SerializeField] private ImproveManager playerImprove;
    private bool playerInteract = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (playerImprove == null) return;

        if (playerInteract && playerImprove.CanImprove())
        {
            myeventSystem.SetSelectedGameObject(nextGameObjectSelected);
            UpgradeCanvas.SetActive(true);
            myCanvas.SetActive(false);
            playerInteract = false;
        }
        else if(playerInteract && !playerImprove.CanImprove())
        {
            StartCoroutine(ShowErrorWhileTryingToUpgrade());
            playerInteract = false;
        }
	}

    IEnumerator ShowErrorWhileTryingToUpgrade()
    {
        ErrorText.enabled = true;
        ArrowGO.enabled = true;

        yield return new WaitForSecondsRealtime(0.1f);

        ErrorText.DOFade(0.0f, 0.5f);
        ArrowGO.DOFade(0.0f, 0.5f);

        yield return new WaitForSecondsRealtime(0.75f);

        ErrorText.enabled = false;
        ArrowGO.enabled = false;

        ErrorText.color = new Color(ErrorText.color.r, ErrorText.color.g, ErrorText.color.b, 1.0f);
        ArrowGO.color = new Color(ArrowGO.color.r, ArrowGO.color.g, ArrowGO.color.b, 1.0f); ;

    }

    public void WantToInteract()
    {
        playerInteract = true;
    }

}
