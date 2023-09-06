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
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> onGameStateChanged;

    [SerializeField] private vThirdPersonInput thirdPersonInput;
    [SerializeField] private EndlessBook endlessBook;
    [SerializeField] private TouchPad touchPad;

    [HideInInspector][SerializeField] public bool isInDialogue;

    //[SerializeField] private GameObject pauseMenu, journal;

    [SerializeField] CameraStacking playerCamera;

    bool isClosing = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Play);
    }

    [HideInInspector] public float ignoreFixedFrame = -1;

    void SetKinematic(bool value)
    {
        ignoreFixedFrame = Time.fixedTime + Time.fixedDeltaTime * 1.5f;
        thirdPersonInput.gameObject.GetComponent<Rigidbody>().isKinematic = value;
    }

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
        Time.timeScale = 0;
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
