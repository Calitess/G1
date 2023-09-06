using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestingNum : MonoBehaviour
{
    [SerializeField] Playtesting_Thanks gameManager;

    private void OnDisable()
    {
        gameManager.CheckInteractionNumber(1);
    }
}
