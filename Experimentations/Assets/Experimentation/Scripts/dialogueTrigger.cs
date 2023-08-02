using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class dialogueTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (Input.GetKeyDown(KeyCode.E)) ///if you wanna use this, make sure to change to OnTriggerStay
           //{
           dialogueRunner.StartDialogue("Start");
           //}
        }
    }


}
