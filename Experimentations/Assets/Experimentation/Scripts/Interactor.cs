using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
[ExecuteInEditMode]
public class Interactor : MonoBehaviour
{
    
    
    [SerializeField] float radius;
    [SerializeField] float lerpDuration = 3f;
    [SerializeField] float targetValue = 4.5f;
    [SerializeField] float outTargetValue = 0f;

    [SerializeField] RealmPostProcess VoronoiShaderManager;
    [SerializeField] AudioSource RealmWhoosh;
    [SerializeField] AudioClip[] RealmSoundClips;
    [SerializeField] bool isWhooshSoundPlaying = false;


    

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

            if(VoronoiShaderManager == null)
            {
                VoronoiShaderManager = GameObject.Find("VoronoiShaderManager").GetComponent<RealmPostProcess>();
            }

            VoronoiShaderManager.FadeVoronoiShaderOut();
        }
    }


    public void OpenRealm()
    {

        StartCoroutine(LerpFunction(targetValue, lerpDuration, "opening"));
        
        if(RealmWhoosh == null)
        {
            RealmWhoosh = this.GetComponent<AudioSource>();
        }

        if(isWhooshSoundPlaying == false)
        {
            isWhooshSoundPlaying = true;
            PlayWhooshSound();

        }
    }

    public void PlayWhooshSound()
    {
        RealmWhoosh.PlayOneShot(RealmSoundClips[Random.Range(0,RealmSoundClips.Length)]);
    }

    public void CloseRealm()
    {
        StopAllCoroutines();
        StartCoroutine(LerpFunction(outTargetValue, lerpDuration, "closing"));

        if (isWhooshSoundPlaying == true)
        {
            isWhooshSoundPlaying = false;
            PlayWhooshSound();

        }
    }

    public void DeactivateInteractor()
    {

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