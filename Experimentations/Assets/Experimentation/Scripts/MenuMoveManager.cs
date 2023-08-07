using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMoveManager : MonoBehaviour
{
    private Image menuImagesToMove;
    [SerializeField] float randomValue, durationForLerp;
    private Vector2 pointsToMove, curPoint, oriPoint;

    // Start is called before the first frame update
    void Start()
    {
        menuImagesToMove = this.GetComponent<Image>();
        curPoint = menuImagesToMove.rectTransform.localPosition;
        oriPoint = menuImagesToMove.rectTransform.localPosition;
        StartCoroutine(LerpImages(randomValue, durationForLerp));
        //Debug.Log("image:" + menuImagesToMove + "cur:" + curPoint + "togo:" + pointsToMove);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LerpImages(float randomVal, float duration)
    {
            pointsToMove = new Vector2(Random.Range(oriPoint.x, randomVal), Random.Range(oriPoint.y, randomVal));
            float time = 0;
            curPoint = menuImagesToMove.rectTransform.localPosition;
            while (time < duration)
            {
                menuImagesToMove.rectTransform.localPosition = Vector2.Lerp(curPoint, pointsToMove, time / duration);
                time += Time.deltaTime;
                yield return null;

            //Debug.Log("image:" + menuImagesToMove + "cur:" + curPoint + "togo:" + pointsToMove);
            }
            menuImagesToMove.rectTransform.localPosition = pointsToMove;
        

        yield return new WaitForSecondsRealtime(0);

        StartCoroutine(LerpImages(randomValue, durationForLerp));
    }
}
