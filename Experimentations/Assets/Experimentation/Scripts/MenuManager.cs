using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu,journal;
    

    private void Awake()
    {
        //subscribing this into the event
        GameManager.onGameStateChanged += GameManager_onGameStateChanged;
        Debug.Log("I am subscribing" + this); 
    }

     private void OnDestroy()
    {                                       
        //unsubscribing this from the event
        GameManager.onGameStateChanged -= GameManager_onGameStateChanged;
        Debug.Log("I am unsubscribing" + this);
    }

    private void GameManager_onGameStateChanged(GameState state)
    {
        
        pauseMenu.SetActive(state == GameState.Pause); //only show the pause menu when in the pause game state, otherwise, hide it.

        journal.SetActive(state == GameState.Journal);




    }



}
