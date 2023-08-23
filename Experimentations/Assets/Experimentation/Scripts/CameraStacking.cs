using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraStacking : MonoBehaviour
{
    [SerializeField] Camera cameraPlayer, myOverlayCamera;
    [SerializeField] bool journalOpened;
    

    public void OpenJournal()
    {
        var cameraData = cameraPlayer.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(myOverlayCamera);
        journalOpened = false;
    }

    public void CloseJournal()
    {
        var cameraData = cameraPlayer.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Remove(myOverlayCamera);
        journalOpened = true;
    }
}
