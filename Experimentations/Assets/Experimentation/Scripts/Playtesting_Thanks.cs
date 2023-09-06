using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Playtesting_Thanks : MonoBehaviour
{
    [SerializeField] int maxInteractionNum;
    [SerializeField] TMP_Text thanksText;

    int curInteractionNum = 0;

    private void Start()
    {
        CheckInteractionNumber(0);
    }
    public void CheckInteractionNumber(int howManyInteracted)
    {
        

        int newNum = howManyInteracted + curInteractionNum;
        curInteractionNum = newNum;

        if (curInteractionNum == maxInteractionNum) 
        {
            thanksText.text = "You have interacted with all interactions. Thank you for playing!";
        }
        else if (curInteractionNum < maxInteractionNum)
        {
            thanksText.text = $"You have interacted with {curInteractionNum} out of {maxInteractionNum}";
        }

    }
}
