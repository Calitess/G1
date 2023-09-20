using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FadingText : MonoBehaviour
{


    [SerializeField] TMP_Text curText;
    [SerializeField] float delayBetweenCharacters = 0.05f, delayBetweenSentences = 1f;
    [SerializeField] [TextArea] string[] allText;
    private string currentText = "";
    private int currentIndex=0;

    
    public void ChangeShownText()
    {
        curText.text = allText[currentIndex];
        ShowText();
        
        
    }

    public void ShowText()
    {
        
        StartCoroutine(FadeTextToFullAlpha(.3f, curText));
        
    }
    public void FinishText()
    {

        StartCoroutine(FadeTextToZeroAlpha(3f, curText));

    }


    public IEnumerator FadeTextToFullAlpha(float t, TMP_Text i)
    {
        
        string textLength = curText.text;

        for (int tw=0;tw< textLength.Length; tw++ )
        {
            
            currentText = textLength.Substring(0, tw);
            i.text = currentText;
            //i.color = new Color(i.color.r, i.color.g, i.color.b, 0);

            //while (i.color.a < 1.0f)
            //{
            //    i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            //    yield return null;
            //}

            yield return new WaitForSecondsRealtime(delayBetweenCharacters); //delay between characters
            

        }

        

        if (currentIndex+1 < allText.Length)
        {
            //FinishText();
            currentIndex++;
            yield return new WaitForSecondsRealtime(delayBetweenSentences); //delay between sentences
            ChangeShownText();
        }
        else
        {
            FinishText();
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }


}