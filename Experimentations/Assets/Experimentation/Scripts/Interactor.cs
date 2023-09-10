using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;

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
    [SerializeField] AudioClip RealmMusicClip;
    [SerializeField] bool isWhooshSoundPlaying = false;

    [SerializeField] ParticleSystem realmSmoke;

    GameManager gameManager;

    [SerializeField] UnityEvent OnInteractorEnter, OnInteractorStay, OnInteractorExit;



    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_Interactor_Position", transform.position);
        Shader.SetGlobalFloat("_Interactor_Radius", radius);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.fixedTime < gameManager.ignoreFixedFrame)
                return;

            OnInteractorEnter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.fixedTime < gameManager.ignoreFixedFrame)
                return;

            OnInteractorStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            

            if (Time.fixedTime < gameManager.ignoreFixedFrame)
                return;

            OnInteractorExit.Invoke();

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

            new WaitUntil(() => RealmWhoosh.isPlaying);

            RealmWhoosh.PlayOneShot(RealmMusicClip);

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
            RealmWhoosh.Stop();
            PlayWhooshSound();

        }

        realmSmoke.playbackSpeed = 8;
        realmSmoke.Stop();
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

    public void ResetParticleSpeed(float speed)
    {
        realmSmoke.playbackSpeed = speed;
    }


}