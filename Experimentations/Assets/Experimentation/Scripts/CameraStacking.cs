using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraStacking : MonoBehaviour
{
    [SerializeField] Camera camera, myOverlayCamera;
    [SerializeField] bool journalOpened;
    



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && journalOpened)
        {
            var cameraData = camera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(myOverlayCamera);
            journalOpened = false;
        }

        else if (Input.GetKeyDown(KeyCode.Q) && !journalOpened)
        {
            var cameraData = camera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Remove(myOverlayCamera);
            journalOpened = true;

        }
    }
}
