using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Interactor : MonoBehaviour
{
    [SerializeField] float radius;

    [SerializeField] float lerpDuration = 3f;
    [SerializeField] float targetValue = 4.5f;

    [SerializeField] float outTargetValue = 0f;

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_Interactor_Position", transform.position);
        Shader.SetGlobalFloat("_Interactor_Radius", radius);


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseRealm();
        }
    }


    public void OpenRealm()
    {

        StartCoroutine(LerpFunction(targetValue, lerpDuration, "opening"));
    }


    public void CloseRealm()
    {
        StopAllCoroutines();
        StartCoroutine(LerpFunction(outTargetValue, lerpDuration, "closing"));
    }

    IEnumerator LerpFunction(float endValue, float duration, string comment)
    {
        float time = 0;
        float startValue = radius;
        while (time < duration)
        {
            radius = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
            Debug.Log(comment);
        }
        radius = endValue;
    }



}