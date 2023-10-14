using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eavesdrop_Whispers : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
            audioSource.Pause();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
            audioSource.Play();
        }
    }
}
