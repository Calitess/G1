using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class npcDialogue : MonoBehaviour
{
    public DialogueRunner TalkDialogue, ThoughtDialogue,EavesdropDialogue;


    [SerializeField] bool Talk, Thought, Eavesdrop;

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

        if(Talk)
        {
            ThoughtDialogue.gameObject.SetActive(false);
            EavesdropDialogue.gameObject.SetActive(false);

            TalkDialogue.gameObject.SetActive(true);
            TalkDialogue.StartDialogue(nodename);
        }
        else if (Thought)
        {
           
            TalkDialogue.gameObject.SetActive(false);
            EavesdropDialogue.gameObject.SetActive(false);

            ThoughtDialogue.gameObject.SetActive(true);
            ThoughtDialogue.StartDialogue(nodename);
        }
        else if(Eavesdrop)
        {
            
            ThoughtDialogue.gameObject.SetActive(false);
            TalkDialogue.gameObject.SetActive(false);

            EavesdropDialogue.gameObject.SetActive(true);
            EavesdropDialogue.StartDialogue(nodename);
        }

    }



}