using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using echo17.EndlessBook;


public class custCommand : MonoBehaviour
{
    [SerializeField] TMP_Text journalEntryText;
    [SerializeField][TextArea] string whatToWrite;
    [SerializeField] Material pageMaterial;
    [SerializeField] EndlessBook book;


    [YarnCommand("JournalEntry")]
    public void JournalEntry()
    {
        Debug.Log("journal entry is inserted");

        journalEntryText.text = whatToWrite;

    }

    [YarnCommand("NewPageEntry")]
    public void NewPageEntry()
    {
        Debug.Log("page entry is inserted");
       
       book.AddPageData(pageMaterial);
    }
}
