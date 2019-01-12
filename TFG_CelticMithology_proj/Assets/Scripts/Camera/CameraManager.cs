using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Animator camera_anim;

    public void Cam_Shake()
    {
        camera_anim.SetTrigger("Shake");
    }

}
