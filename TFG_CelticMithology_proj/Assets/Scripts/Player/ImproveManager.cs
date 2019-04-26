using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImproveManager : MonoBehaviour {

    [SerializeField] private int objToImprove = 5;
    [SerializeField] private Image improveBar;
    private float sliceBar=0;

	// Use this for initialization
	void Start () {

        sliceBar = 1.0f / objToImprove;

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
