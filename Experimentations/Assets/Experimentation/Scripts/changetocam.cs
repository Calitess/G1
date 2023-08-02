using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changetocam : MonoBehaviour
{
    [SerializeField] GameObject CineCamera, PlayerCam;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CineCamera.SetActive(true);
            PlayerCam.SetActive(false);


        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CineCamera.SetActive(false);
            PlayerCam.SetActive(true);


        }
    }
}
