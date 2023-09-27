using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingImages : MonoBehaviour
{
    private Transform menuImagesToMove;
    [SerializeField] float randomValue, durationForLerp;
    private Vector3 pointsToMove, curPoint, oriPoint;

    

    void Start()
    {
        menuImagesToMove = this.GetComponent<Transform>();
        curPoint = menuImagesToMove.localPosition;
        oriPoint = menuImagesToMove.localPosition;
        StartCoroutine(LerpImages(randomValue, durationForLerp));
        //Debug.Log("image:" + menuImagesToMove + "cur:" + curPoint + "togo:" + pointsToMove);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator LerpImages(float randomVal, float duration)
    {
        pointsToMove = new Vector3(Random.Range(oriPoint.x - randomVal, oriPoint.x+randomVal), Random.Range(oriPoint.y - randomVal, oriPoint.y+randomVal),oriPoint.z);
        float time = 0;
        curPoint = menuImagesToMove.localPosition;
        while (time < duration)
        {
            menuImagesToMove.localPosition = Vector3.Lerp(curPoint, pointsToMove, time / duration);
            time += Time.deltaTime;
            yield return null;

            //Debug.Log("image:" + menuImagesToMove + "cur:" + curPoint + "togo:" + pointsToMove);
        }
        menuImagesToMove.localPosition = pointsToMove;


        yield return new WaitForSecondsRealtime(0);

        StartCoroutine(LerpImages(randomValue, durationForLerp));
    }
}
