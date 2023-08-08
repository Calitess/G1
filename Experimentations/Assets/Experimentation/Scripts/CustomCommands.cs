using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CustomCommands : MonoBehaviour
{
    Animator anim;


    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    
    }


[YarnCommand("Talk")]
   
    public void SetTalk(string talkName)
    {
     anim.Play("Base Layer." + talkName, 0);
        Debug.Log("Character switches from idle to talking animation");
      
     // anim.Play("CharacterTalk");
    }
   
}
