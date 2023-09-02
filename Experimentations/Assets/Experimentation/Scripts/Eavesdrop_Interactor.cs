using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Eavesdrop_Interactor : MonoBehaviour
{
    [SerializeField] Canvas eavesdroppingCanvas;
    [SerializeField] Vector3 offset;
    
    public void assignCinemachineCamera(CinemachineVirtualCamera cinemachineCamera)
    {
        if (cinemachineCamera == null)
        {
            cinemachineCamera = this.GetComponent<CustomCommands>().virtualCamera;
        }
        eavesdroppingCanvas.worldCamera = CinemachineCore.Instance.FindPotentialTargetBrain(cinemachineCamera).GetComponent<Camera>();
        eavesdroppingCanvas.GetComponent<Billboarding>().mainCam = eavesdroppingCanvas.worldCamera;
    }
    public void moveCanvasHere()
    {
        eavesdroppingCanvas.transform.position = transform.position + offset;
    }
}
