using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using echo17.EndlessBook;
using TMPro;
using Unity.VisualScripting;
using Invector.vCharacterController.vActions;
using UnityEngine.UI;

public class CustomCommands : MonoBehaviour
{
    Animator anim;
    GameManager gameManager;

    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject playerCam;
    [SerializeField] TMP_Text journalEntryText;
    [SerializeField][TextArea] string whatToWrite;
    [SerializeField] Material leftPageMaterial, rightPageMaterial;
    [SerializeField] EndlessBook book;
    [SerializeField] public int pageNumber;
    [SerializeField] bool deleteTriggeAfterDialogue;
    [SerializeField] Interactor activateThisRift;
    [SerializeField] Image mouseButtonPrompt;

    vTriggerGenericAction action;
    AudioSource scribbleSource;



    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        scribbleSource = FindObjectOfType<vThirdPersonController>().gameObject.GetComponent<AudioSource>();
        //book = FindObjectOfType<EndlessBook>();
        action = gameObject.GetComponent<vTriggerGenericAction>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (action.enabled == true && mouseButtonPrompt!=null)
            {

                mouseButtonPrompt.gameObject.SetActive(true);
            }
            else
            {
                mouseButtonPrompt.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && mouseButtonPrompt != null)
        {
            mouseButtonPrompt.gameObject.SetActive(false);
        }
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
        if (virtualCamera != null)
        {
            var vcamBrain = CinemachineCore.Instance.FindPotentialTargetBrain(virtualCamera);
            vcamBrain.gameObject.GetComponent<AudioListener>().enabled = true;
            virtualCamera.Priority = 100;
            playerCam.SetActive(false);
        }

        gameManager.isInDialogue = true;


    }

    [YarnCommand("OutDialogue")]

    public void CurOutDialogue()
    {
        if (virtualCamera != null)
        {
            var vcamBrain = CinemachineCore.Instance.FindPotentialTargetBrain(virtualCamera);
            vcamBrain.gameObject.GetComponent<AudioListener>().enabled = false;
            virtualCamera.Priority = 0;
            playerCam.SetActive(true);
        }


        gameManager.isInDialogue = false;


        if (deleteTriggeAfterDialogue)
        {
            action.enabled = false;
        }


    }

    [YarnCommand("JournalEntryText")]
    public void JournalEntry()
    {
        Debug.Log("journal entry is inserted");

        scribbleSource.Play();

        journalEntryText.text = whatToWrite;

    }

    [YarnCommand("NewPageEntry")]
    public void NewPageEntry()
    {
        scribbleSource.Play();

        Debug.Log("page entry is addedd");
        book.AddPageData(leftPageMaterial);
        book.AddPageData(rightPageMaterial);
    }

    [YarnCommand("InsertEntry")]
    public void InsertEntry()
    {

        scribbleSource.Play();

        Debug.Log("page entry is inserted");
        book.InsertPageData((pageNumber - 1), leftPageMaterial);
        book.InsertPageData(pageNumber, rightPageMaterial);
    }


    [YarnCommand("ActivateRift")]
    public void ActivateRift()
    {
        activateThisRift.gameObject.SetActive(true);
    }




}
