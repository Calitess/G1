using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;
using echo17.EndlessBook.Demo02;
using echo17.EndlessBook;
using static echo17.EndlessBook.EndlessBook;

public class GameManager : MonoBehaviour
{
    //an instance of this game manager
    public static GameManager Instance;

    //shows what game state we are in
    public GameState State;

    //action event of the game state to track changes
    public static event Action<GameState> onGameStateChanged;

    //Third person controller
    [SerializeField] private vThirdPersonInput thirdPersonInput;

    //Journal - endless book asset
    [SerializeField] private EndlessBook endlessBook;

    //The colliders that has the touchpad script to click on pages of the journal
    [SerializeField] private TouchPad touchPad;

    //bool to check if it is dialogue or not
    [HideInInspector][SerializeField] public bool isInDialogue;

    //[SerializeField] private GameObject pauseMenu, journal;

    //camera stacking script to overlay the journal camera on top of the player camera
    [SerializeField] CameraStacking playerCamera;

    //bool to check if the book is closing or not
    bool isClosing = false;

    private void Awake()
    {
        //create an instance of this game manager
        Instance = this;
    }

    private void Start()
    {
        //Make the game state as 'play' at start
        UpdateGameState(GameState.Play);
    }

    //this is to fix the physX after unity 2019
    [HideInInspector] public float ignoreFixedFrame = -1;

    void SetKinematic(bool value)
    {
        ignoreFixedFrame = Time.fixedTime + Time.fixedDeltaTime * 1.5f;
        thirdPersonInput.gameObject.GetComponent<Rigidbody>().isKinematic = value;
    }

    //method to update the game state
    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Pause:
                PauseGame();
                break;
            case GameState.Play:
                PlayGame();
                break;
            case GameState.Journal:
                JournalState();
                break;
            case GameState.Dialogue:
                InDialogue();
                break;
            default:
                break;
        }


        onGameStateChanged?.Invoke(newState); //has anybody subscribed to this function? If so, invoke this function 
    }

    //change the game state according to what is the condition
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && State == GameState.Play)
        {
           GameManager.Instance.UpdateGameState(GameState.Pause);
            
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && State == GameState.Pause)
        {
            GameManager.Instance.UpdateGameState(GameState.Play);
        }

        if (Input.GetKeyDown(KeyCode.Q) && State == GameState.Play)
        {
            isClosing = false;
            GameManager.Instance.UpdateGameState(GameState.Journal);

        }
        else if (State == GameState.Journal)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
            {
                
                //if the book is currently closing, dont allow player to close book in journal state
                if(isClosing == false && endlessBook.IsTurningPages == false)
                {

                    isClosing = true;
                    StartCoroutine(CloseJournal());

                    
                }
                

            }
        }
        
        if(isInDialogue && State == GameState.Play)
        {
            
            GameManager.Instance.UpdateGameState(GameState.Dialogue);
            
        }
        else if(!isInDialogue && State == GameState.Dialogue)
        {
            GameManager.Instance.UpdateGameState(GameState.Play);
        }
    }

    private void JournalState()
    {
        playerCamera.OpenJournal();
        //journal.SetActive(true);

        touchPad.Toggle(TouchPad.PageEnum.Right,true);
        touchPad.Toggle(TouchPad.PageEnum.Left, false);

        thirdPersonInput.ShowCursor(true);
        thirdPersonInput.LockCursor(true);
        Time.timeScale = 0.00001f;
        //Debug.Log("I am now inside the journal");
    }

    IEnumerator CloseJournal()
    {
      thirdPersonInput.ShowCursor(false);
      thirdPersonInput.LockCursor(false);

      endlessBook.SetState(StateEnum.ClosedFront, 0.3f, null, true);
      endlessBook.SetPageNumber(1);

      yield return new WaitForSecondsRealtime(0.8f);


      GameManager.Instance.UpdateGameState(GameState.Play);

    }

    private void PlayGame()
    {
        

        playerCamera.CloseJournal();
        //journal.SetActive(false);
        //pauseMenu.SetActive(false);

        thirdPersonInput.enabled = true;
        thirdPersonInput.ShowCursor(false);
        thirdPersonInput.LockCursor(false);
        SetKinematic(false);


        Time.timeScale = 1;
        Debug.Log("I am now playing");
    }

    private void InDialogue()
    {


        thirdPersonInput.ShowCursor(true);
        thirdPersonInput.LockCursor(true);
        thirdPersonInput.enabled = false;
        thirdPersonInput.gameObject.GetComponent<Animator>().SetFloat("InputMagnitude", 0f);
        SetKinematic(true);
    }

    
    public void OutDialogue()
    {
        thirdPersonInput.ShowCursor(false);
        thirdPersonInput.LockCursor(false);
    }

    private void PauseGame()
    {
        //pauseMenu.SetActive(true);

        thirdPersonInput.ShowCursor(true);
        thirdPersonInput.LockCursor(true);
        Time.timeScale = 0;
        Debug.Log("I am now paused");
    }
}

public enum GameState
{
    Pause,
    Play,
    Journal,
    Dialogue,
}
