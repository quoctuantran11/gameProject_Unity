using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCamera : MonoBehaviour
{
    public void Activate()
    {
        GetComponent<Animator>().SetTrigger("Go");
    }

    void ResetCam()
    {
        FindObjectOfType<CameraFollow>().maxXAndY.x = 200;
    }
}
