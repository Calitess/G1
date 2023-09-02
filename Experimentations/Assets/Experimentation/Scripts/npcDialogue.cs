using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class npcDialogue : MonoBehaviour
{
    public DialogueRunner dialogueRunner, disableThisDialogueRunner;


    //void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (Input.GetMouseButtonDown(0)) ///if you wanna use this, make sure to change to OnTriggerStay
    //        {
    //        dialogueRunner.StartDialogue("Start");
    //        }
    //    }
    //}

    public void dialogue(string nodename)
    {
        disableThisDialogueRunner.gameObject.SetActive(false);
        dialogueRunner.gameObject.SetActive(true);
        dialogueRunner.StartDialogue(nodename);
    }



}