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
using UnityEngine.Events;

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
    [SerializeField] bool hasInteracted;

    [SerializeField] TMP_Text newJournalEntry;
    [SerializeField] TMP_Text newObjective;

    [SerializeField] GameObject sequentialImage;

    vTriggerGenericAction action;
    AudioSource scribbleSource;

    [SerializeField] UnityEvent OnInteractorEnter, OnInteractorStay, OnInteractorExit;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        scribbleSource = FindObjectOfType<vThirdPersonController>().gameObject.GetComponent<AudioSource>();
        //book = FindObjectOfType<EndlessBook>();
        action = gameObject.GetComponent<vTriggerGenericAction>();

    }

    public void ShowMousePrompt(Image mouseButtonPrompt)
    {
        
      if (hasInteracted == false && action.enabled == true && mouseButtonPrompt != null)
      {

          mouseButtonPrompt.gameObject.SetActive(true);

          if (Input.GetMouseButtonDown(0))
          {
              hasInteracted = true;
              mouseButtonPrompt.gameObject.SetActive(false);
          }

      }
      else if (action.enabled == false && mouseButtonPrompt != null)
      {

          mouseButtonPrompt.gameObject.SetActive(false);
      }

    }

    public void HideMousePrompt(Image mouseButtonPrompt)
    {
        if (mouseButtonPrompt != null)
        {
            hasInteracted = false;
            mouseButtonPrompt.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.fixedTime < gameManager.ignoreFixedFrame)
                return;

            OnInteractorEnter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.fixedTime < gameManager.ignoreFixedFrame)
                return;

            OnInteractorStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("Player"))
        {


            if (Time.fixedTime < gameManager.ignoreFixedFrame)
                return;

            OnInteractorExit.Invoke();

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
            if (action != null)
            {
                action.enabled = false;
            }
        }


    }

    [YarnCommand("JournalEntryText")]
    public void JournalEntry()
    {
        //Debug.Log("journal entry is inserted");

        scribbleSource.Play();
        StartCoroutine(NewObjective());
        journalEntryText.text = whatToWrite;

    }

    IEnumerator NewObjective()
    {


        newObjective.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        newObjective.gameObject.SetActive(false);
    }

    [YarnCommand("NewPageEntry")]
    public void NewPageEntry()
    {
        scribbleSource.Play();
        StartCoroutine(NewJournalEntry());
        //Debug.Log("page entry is addedd");
        book.AddPageData(leftPageMaterial);
        book.AddPageData(rightPageMaterial);
    }

    [YarnCommand("InsertEntry")]
    public void InsertEntry()
    {

        scribbleSource.Play();
        StartCoroutine(NewJournalEntry());
        //Debug.Log("page entry is inserted");
        book.InsertPageData((pageNumber - 1), leftPageMaterial);
        book.InsertPageData(pageNumber, rightPageMaterial);
    }

    IEnumerator NewJournalEntry()
    {


        newJournalEntry.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        newJournalEntry.gameObject.SetActive(false);
    }


    [YarnCommand("ActivateRift")]
    public void ActivateRift()
    {
        activateThisRift.gameObject.SetActive(true);
    }

    [YarnCommand("ShowImage")]
    public void ShowImage()
    {
        scribbleSource.Play();
        StartCoroutine(NewJournalEntry());
        sequentialImage.SetActive(true);
    }


}
