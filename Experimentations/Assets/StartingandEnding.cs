using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class StartingandEnding : MonoBehaviour
{
    [SerializeField] vThirdPersonInput thirdPersonInput;
    private void Start()
    {
        fadeFromBlack.gameObject.SetActive(true);
        fadeFromBlack.color = Color.black;
        FadingBlack(new Color(0, 0, 0, 0), 3);
    }

    [YarnCommand("EndGame")]
    public void EndGame()
    {
        fadeFromBlack.gameObject.SetActive(true);
        FadingBlack(new Color(0, 0, 0, 1), 3);
    }

    [YarnCommand("ChangeScene")]
    public void ChangeScene()
    {
        StartCoroutine(ChangeToEndingScene());
    }
    IEnumerator ChangeToEndingScene()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1.0f;
        thirdPersonInput.ShowCursor(true);
        thirdPersonInput.LockCursor(true);
        GoToScene(3);
    }

    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    [SerializeField] private Image fadeFromBlack;


    IEnumerator FadeBlack(Color amountToFadeTo, float duration)
    {
        float time = 0;
        Color startValue = fadeFromBlack.color;
        while (time < duration)
        {
            fadeFromBlack.color = Color.Lerp(startValue, amountToFadeTo, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        fadeFromBlack.color = amountToFadeTo;

        if(amountToFadeTo == new Color(0, 0, 0, 0))
        {

            fadeFromBlack.gameObject.SetActive(false);
        }


    }

    public void FadingBlack(Color amountToFadeTo, float duration)
    {
        StartCoroutine(FadeBlack(amountToFadeTo, duration));
    }
}
