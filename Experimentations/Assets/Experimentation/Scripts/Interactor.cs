using Cinemachine;
using Invector.vCharacterController.vActions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
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
    [HideInInspector][SerializeField] bool isWhooshSoundPlaying = false;

    [SerializeField] ParticleSystem realmSmoke, riftSmoke, riftTrail1, riftTrail2;
    [SerializeField] ProximityDistortion proximityDistortion;

    GameManager gameManager;
    vTriggerGenericAction action;

    [SerializeField] UnityEvent OnRealmOpen,OnInteractorEnter, OnInteractorStay, OnInteractorExit;

    [HideInInspector]public bool realmInteractionsFinished = false;
    [SerializeField] private List<CustomCommands> Interactables;
    [SerializeField] private vTriggerGenericAction dialogueToTriggerAfterInteractionsFinish;

    [SerializeField] bool realmOpened = false;
    [SerializeField] VisualEffect[] manaiaVfx;


    [SerializeField] bool deactivateInvisibleWall;
    [SerializeField] GameObject invisibleWall;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        action = this.GetComponent<vTriggerGenericAction>();

        if(dialogueToTriggerAfterInteractionsFinish !=null)
        {

            dialogueToTriggerAfterInteractionsFinish.enabled = false;
            dialogueToTriggerAfterInteractionsFinish.gameObject.GetComponent<SphereCollider>().enabled = false;
        }
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

        OnRealmOpen.Invoke();

        foreach (CustomCommands interactable in Interactables)
        {
            interactable.gameObject.SetActive(true);
        }

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

            RealmWhoosh.clip = RealmMusicClip;
            RealmWhoosh.Play();
            RealmWhoosh.loop = true;

        }

        realmOpened = true;
        
        for(int i =0;i<manaiaVfx.Length;i++)
        {
            manaiaVfx[i].Play();
        }
    }

    public void PlayWhooshSound()
    {
        RealmWhoosh.PlayOneShot(RealmSoundClips[Random.Range(0,RealmSoundClips.Length)]);

    }

    public void CloseRealm()
    {
        for (int i = 0; i < manaiaVfx.Length; i++)
        {
            manaiaVfx[i].Stop();
        }

        if (Interactables != null && Interactables.Count != 0)
        {
            foreach (CustomCommands interactable in Interactables.ToList())
            {
                interactable.gameObject.SetActive(false);

                if (interactable.hasInteracted && realmOpened == true)
                {
                    Debug.Log($"{interactable.name} has been interacted and removed from the list.");
                    Interactables.Remove(interactable);
                    
                }

                

            }
        }
        

        EvaluateInteractions();

        if (realmInteractionsFinished && realmOpened == true)
        {
            DeactivateInteractor();
        }
        else if (realmOpened == true)
        {
            StopAllCoroutines();
            StartCoroutine(LerpFunction(3f, lerpDuration, "closing"));

            if (isWhooshSoundPlaying == true)
            {
                isWhooshSoundPlaying = false;
                action.enabled = true;
                RealmWhoosh.Stop();
                RealmWhoosh.loop = false;
                PlayWhooshSound();

            }

            realmSmoke.playbackSpeed = 8;
            realmSmoke.Stop();
            realmOpened = false;

            ActivateRealmEffects();
        }
    }

    public void DeactivateInteractor()
    {
        StopAllCoroutines();
        StartCoroutine(LerpFunction(outTargetValue, lerpDuration, "closing"));

        if (isWhooshSoundPlaying == true)
        {
            isWhooshSoundPlaying = false;
            action.enabled = false;
            RealmWhoosh.Stop();
            RealmWhoosh.loop = false;
            PlayWhooshSound();

        }

        realmSmoke.playbackSpeed = 8;
        realmSmoke.Stop();

        this.GetComponent<SphereCollider>().enabled = false;
        realmOpened = false;
    }

    void ActivateRealmEffects()
    {
        riftSmoke.Play();
        riftTrail1.Play();
        riftTrail2.Play();
        proximityDistortion.gameObject.SetActive(true);
        proximityDistortion.EnableProximityDistort();

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

    public void EvaluateInteractions()
    {

        if(Interactables.Count == 0)
        {
            realmInteractionsFinished = true;
            ActivateDialogue();

            if (deactivateInvisibleWall)
            {
                if (invisibleWall != null)
                {

                    invisibleWall.SetActive(false);
                }
            }
        }

        
    }

    void ActivateDialogue()
    {
        //this is the dialogue to be activated after interactions in rift is finished
        if(dialogueToTriggerAfterInteractionsFinish !=null)
        {

            dialogueToTriggerAfterInteractionsFinish.enabled = true;
            dialogueToTriggerAfterInteractionsFinish.gameObject.GetComponent<SphereCollider>().enabled = true;
        }

    }

}