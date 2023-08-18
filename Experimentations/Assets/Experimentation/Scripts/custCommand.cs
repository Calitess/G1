using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using echo17.EndlessBook;
using Unity.VisualScripting;

public class custCommand : MonoBehaviour
{
    [SerializeField] TMP_Text journalEntryText;
    [SerializeField][TextArea] string whatToWrite;
    [SerializeField] Material blankMaterial, pageMaterial;
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
        Debug.Log("page entry is addedd");
        book.AddPageData(blankMaterial);
        book.AddPageData(pageMaterial);
    }

    [YarnCommand("InsertEntry")]
    public void InsertEntry()
    {
        Debug.Log("page entry is inserted");

        book.InsertPageData(book.pages.Count-1,pageMaterial);
    }


}
