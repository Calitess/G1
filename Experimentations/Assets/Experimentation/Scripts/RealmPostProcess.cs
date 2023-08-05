using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RealmPostProcess : MonoBehaviour
{
    [SerializeField] Material VoronoiShader;
    [SerializeField] float VoronoiIntensity, targetIntensity =1f, fadeOutTargetValue = 0f;
    [SerializeField] float lerpDuration = 3f;

    void Update()
    {
        VoronoiShader.SetFloat("_VignetteIntensity", VoronoiIntensity);

        

    }

    public void FadeVoronoiShaderIn()
    {
        StartCoroutine(LerpFunction(targetIntensity, lerpDuration, "fading post processing in"));
    }

    public void FadeVoronoiShaderOut()
    {
        StopCoroutine(LerpFunction(targetIntensity, lerpDuration, "fading post processing in"));
        StartCoroutine(LerpFunction(fadeOutTargetValue, lerpDuration, "fading post procesing out"));
    }

    IEnumerator LerpFunction(float endValue, float duration, string comment)
    {
        float time = 0;
        float startValue = VoronoiIntensity;
        while (time < duration)
        {
            VoronoiIntensity = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
            Debug.Log(comment);
        }
        VoronoiIntensity = endValue;
    }


}
