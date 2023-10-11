using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SwitchSnapshots : MonoBehaviour
{

    [SerializeField] private AudioMixerSnapshot outsideSnapshot, insideSnapshot;
    [SerializeField] private float transitionTime = 1f;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Time.fixedTime < gameManager.ignoreFixedFrame)
                return;
            insideSnapshot.TransitionTo(transitionTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Time.fixedTime < gameManager.ignoreFixedFrame)
                return;
            outsideSnapshot.TransitionTo(transitionTime);
        }
    }
}
