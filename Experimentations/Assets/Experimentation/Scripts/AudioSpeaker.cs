using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioSpeaker : MonoBehaviour
{
    MusicManager musicManager;

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            musicManager.curSpeaker = this;
            musicManager.ChangePositon();
        }

    }
}
