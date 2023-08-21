using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using echo17.EndlessBook;
using TMPro;
using Unity.VisualScripting;

public class CustomCommands : MonoBehaviour
{
    Animator anim;
    GameManager gameManager;

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject playerCam;
    [SerializeField] TMP_Text journalEntryText;
    [SerializeField][TextArea] string whatToWrite;
    [SerializeField] Material blankMaterial, pageMaterial;
    [SerializeField] EndlessBook book;


    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    
    }


    [YarnCommand("Talk")]
   
    public void SetTalk(string talkName)
    {
     anim.Play("Base Layer." + talkName, 0);
        Debug.Log("Character switches from idle to talking animation");
      
     // anim.Play("CharacterTalk");
    }

    [YarnCommand("InDialogue")]

    public void CurInDialogue()
    {
       gameManager.isInDialogue = true;

        if (virtualCamera != null)
        {
            virtualCamera.Priority = 0;
            playerCam.SetActive(false);
        }

    }

    [YarnCommand("OutDialogue")]

    public void CurOutDialogue()
    {
        gameManager.isInDialogue = false;

        if (virtualCamera != null)
        {
            virtualCamera.Priority = 100;
            playerCam.SetActive(true);
        }


    }

    [YarnCommand("JournalEntryText")]
    public void JournalEntry()
    {
        Debug.Log("journal entry is inserted");

        journalEntryText.text = whatToWrite;

    }

    [YarnCommand("NewPageEntry")]
    public void NewPageEntry()
    {
        Debug.Log("page entry is addedd");
        book.AddPageData(blankMaterial);
        book.AddPageData(pageMaterial);
    }

    [YarnCommand("InsertEntry")]
    public void InsertEntry()
    {
        Debug.Log("page entry is inserted");

        book.InsertPageData(book.pages.Count - 1, pageMaterial);
    }


}
