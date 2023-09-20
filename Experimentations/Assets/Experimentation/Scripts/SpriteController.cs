using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class SpriteController : MonoBehaviour
{
    private SpriteController spriteController;
    public Sprite[] charSprites;
    public Sprite emptySprite;
    public Image spriteL, spriteR;

    private void Awake()
    {
        spriteController = this; //references this script

    }

    //clears the sprite containers after the dialogue is done (this will be called from custom command)
    public void ClearSpriteContainers()
    {
        spriteController.spriteL.sprite = emptySprite;
        spriteController.spriteL.color = Color.white;
        spriteController.spriteR.sprite = emptySprite;
        spriteController.spriteR.color = Color.white;

        //charName.gameObject.GetComponentInChildren<DialogueCharacterNameView>()
    }

    [YarnCommand("changeSprL")]
    public void ChangeSprL(string spriteName)
    {

        Sprite[] spriteArray = spriteController.charSprites;
        Sprite theSprite = emptySprite;
        // Color spriteCol = theSprite.GetComponent<Color>();
        for (int i = 0; i < spriteArray.Length; i++)
        {
            if (spriteArray[i].name == spriteName) //if the name of the sprite is equal to the string input
            {
                theSprite = spriteArray[i]; //set to that sprite
                break;
            }

        }
        spriteController.spriteL.sprite = theSprite;
        Debug.Log(theSprite.name);
    }


    //Changes the sprite on the right side
    [YarnCommand("changeSprR")]
    public void ChangeSprR(string spriteName)
    {
        Debug.Log(spriteName);
        Sprite[] spriteArray = spriteController.charSprites;
        Sprite theSprite = emptySprite;
        // Color colour = theSprite.GetComponent<Color>();
        for (int i = 0; i < spriteArray.Length; i++)
        {
            if (spriteArray[i].name == spriteName)
            {
                theSprite = spriteArray[i]; //find the sprite index number
                break;
            }
            //Debug.Log(spriteArray[i]);

        }
        spriteController.spriteR.sprite = theSprite;

    }

    //Changes the colour of who is talking to white, and not talking to grey
    [YarnCommand("sprTalking")]
    public void SprTalking(char whichSprite)
    {
        if(whichSprite == 'R' || whichSprite == 'r')
        {
            spriteController.spriteR.color = Color.white;
            spriteController.spriteL.color = Color.grey;
        }

        else if (whichSprite == 'L' || whichSprite == 'l')
        {
            spriteController.spriteR.color = Color.grey;
            spriteController.spriteL.color = Color.white;
        }

    }



}