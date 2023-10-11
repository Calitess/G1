using Invector.vCharacterController;
using System.Collections;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using echo17.EndlessBook;
using TMPro;
using Invector.vCharacterController.vActions;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Rendering;

public class CustomCommands : MonoBehaviour
{
    Animator anim;
    GameManager gameManager;

    [SerializeField] bool thisIsPartOfTutorial;
    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject playerCam;
    [Space]
    [SerializeField] RectTransform journalObjectives;
    [SerializeField][TextArea] string whatToWrite;
    [Space]
    [SerializeField] EndlessBook book;
    [SerializeField] Material leftPageMaterial, rightPageMaterial;
    [SerializeField] public int pageNumber;
    [Space]
    [SerializeField] bool deleteTriggeAfterDialogue;
    [Space]
    [SerializeField] Image mouseButtonPrompt;
    [SerializeField] public bool hasInteracted;
    [Space]
    [SerializeField] TMP_Text newJournalEntry;
    [SerializeField] TMP_Text newObjective;
    [Space]
    [SerializeField] GameObject sequentialImage;


    vTriggerGenericAction action;
    AudioSource scribbleSource;
    SpriteController spriteController;
    RiftManager riftManager;
    TutorialInteractions tutorialInteractions;
    ShowInteractable InteractionIcon;
    npcDialogue npcDialogue;

    [SerializeField] private vTriggerGenericAction dialogueToTriggerAfterInteractionsFinish;
    [SerializeField] UnityEvent OnInteractorEnter, OnInteractorStay, OnInteractorExit;


    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        journalObjectives = gameManager.objectiveContent;
        scribbleSource = FindObjectOfType<vThirdPersonController>().gameObject.GetComponent<AudioSource>();
        //book = FindObjectOfType<EndlessBook>();
        action = this.gameObject.GetComponent<vTriggerGenericAction>();
        spriteController = FindObjectOfType<SpriteController>();
        riftManager = FindObjectOfType<RiftManager>();
        tutorialInteractions = FindObjectOfType<TutorialInteractions>();
        InteractionIcon = GetComponent<ShowInteractable>();
        npcDialogue = GetComponent<npcDialogue>();

        if(thisIsPartOfTutorial)
        {
            tutorialInteractions.Interactables.Add(this);   
        }
        if (dialogueToTriggerAfterInteractionsFinish != null)
        {

            dialogueToTriggerAfterInteractionsFinish.enabled = false;
            dialogueToTriggerAfterInteractionsFinish.gameObject.GetComponent<SphereCollider>().enabled = false;
        }

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
            //hasInteracted = false;
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


        if (InteractionIcon !=null && InteractionIcon.InteractableIcon != null)
        {
            InteractionIcon.inDialogue = true;
            InteractionIcon.InteractableIcon.SetActive(false);
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
            SphereCollider col = this.GetComponent<SphereCollider>();
            if (action != null)
            {
                action.enabled = false;
                
            }
            if (col != null)
            {
                col.enabled = false;

            }



        }

        if (thisIsPartOfTutorial)
        {

            //this looks at the index of TutorialInteraction and then removes it
            tutorialInteractions.EvaluateInteractions(this);
        }

        if (InteractionIcon != null && InteractionIcon.InteractableIcon != null)
        {
            InteractionIcon.inDialogue = false;
        }

        if (dialogueToTriggerAfterInteractionsFinish != null)
        {

            dialogueToTriggerAfterInteractionsFinish.enabled = true;
            dialogueToTriggerAfterInteractionsFinish.gameObject.GetComponent<SphereCollider>().enabled = true;
        }

        spriteController.ClearSpriteContainers();

    }



    [YarnCommand("JournalEntryText")]
    public void JournalEntry()
    {
        //Debug.Log("journal entry is inserted");

        scribbleSource.Play();
        StartCoroutine(NewObjective());
        GameObject newObjective = Instantiate(new GameObject(), journalObjectives.transform);
        TMP_Text journalObjectiveText = newObjective.AddComponent<TextMeshProUGUI>();
        journalObjectiveText.name = "Objective";
        journalObjectiveText.fontSize = 20;
        journalObjectiveText.color = Color.black;
        journalObjectiveText.text = whatToWrite;

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
    public void ActivateRift(int indexOfRiftManager)
    {

        riftManager.ActivateThisRift(indexOfRiftManager);
    }

    

    [YarnCommand("ShowImage")]
    public void ShowImage()
    {
        scribbleSource.Play();
        StartCoroutine(NewJournalEntry());
        sequentialImage.SetActive(true);
    }

    [YarnCommand("Cinematic")]
    public void Cinematic(CinemachineVirtualCamera cameraFrom, CinemachineVirtualCamera cameraTo)
    {
        cameraFrom.Priority = 0;
        cameraTo.Priority = 100;
    }

    [YarnCommand("AutoInput")]
    public void AutoInput()
    {
        action.inputType = vTriggerGenericAction.InputType.AutoAction;
    }

    [YarnCommand("ButtonInput")]
    public void ButtonInput()
    {
        action.inputType = vTriggerGenericAction.InputType.GetButtonDown;
    }

    [YarnCommand("FollowPlayer")]
    public void FollowPlayer()
    {
        
        CompanionStateMachine companion = GetComponent<CompanionStateMachine>();

        if(companion !=null)
        {

            companion.SetState(companion.FollowState);
        }
    }
}
