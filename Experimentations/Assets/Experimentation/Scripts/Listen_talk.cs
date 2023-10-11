using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Listen_talk : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    [SerializeField] string NodeName;
 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (Input.GetKeyDown(KeyCode.E)) ///if you wanna use this, make sure to change to OnTriggerStay
            //{
            dialogueRunner.StartDialogue(NodeName);
            //}
        }
    }

    [YarnCommand("DeleteTrigger")]
    public void DeleteTrigger()
    {
         Destroy(this);
     
        
    }

}