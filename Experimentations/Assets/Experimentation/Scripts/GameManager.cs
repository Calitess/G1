using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> onGameStateChanged;

    [SerializeField] private vThirdPersonInput thirdPersonInput;

    [SerializeField] public bool isInDialogue;

    [SerializeField] private GameObject pauseMenu, journal;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Play);
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
        if (Input.GetKeyDown(KeyCode.Q) && State == GameState.Play)
        {
           GameManager.Instance.UpdateGameState(GameState.Pause);
            
        }
        else if(Input.GetKeyDown(KeyCode.Q) && State == GameState.Pause)
        {
            GameManager.Instance.UpdateGameState(GameState.Play);
        }

        if (Input.GetKeyDown(KeyCode.J) && State == GameState.Play)
        {
            GameManager.Instance.UpdateGameState(GameState.Journal);
        }
        else if (Input.GetKeyDown(KeyCode.J) && State == GameState.Journal)
        {
            GameManager.Instance.UpdateGameState(GameState.Play);
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
        

        thirdPersonInput.ShowCursor(true);
        thirdPersonInput.LockCursor(true);
        Time.timeScale = 0;
        //Debug.Log("I am now inside the journal");
    }

    private void PlayGame()
    {

        thirdPersonInput.enabled = true;
        thirdPersonInput.ShowCursor(false);
        thirdPersonInput.LockCursor(false);
        thirdPersonInput.gameObject.GetComponent<Rigidbody>().isKinematic = false;


        Time.timeScale = 1;
        Debug.Log("I am now playing");
    }

    private void InDialogue()
    {
        
        thirdPersonInput.ShowCursor(true);
        thirdPersonInput.LockCursor(true);
        thirdPersonInput.enabled = false;
        thirdPersonInput.gameObject.GetComponent<Animator>().SetFloat("InputMagnitude", 0f);
        thirdPersonInput.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    
    public void OutDialogue()
    {
        thirdPersonInput.ShowCursor(false);
        thirdPersonInput.LockCursor(false);
    }

    private void PauseGame()
    {
        

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
