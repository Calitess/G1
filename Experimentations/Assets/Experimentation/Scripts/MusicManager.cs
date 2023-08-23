using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [SerializeField] public AudioSpeaker curSpeaker;

    // Update is called once per frame
    public void ChangePositon()
    {
        this.transform.localPosition = curSpeaker.transform.localPosition;
    }
}
