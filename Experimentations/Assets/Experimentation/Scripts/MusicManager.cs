
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] public AudioSpeaker curSpeaker;

    [SerializeField] List<AudioSpeaker> audioSpeakers = new List<AudioSpeaker>();


    public float dist, shortestDist;



    private void Update()
    {
        shortestDist = float.MaxValue;
        var newSpeaker = curSpeaker;

        for (int i = 0; i < audioSpeakers.Count; i++)
        {
            Vector3 speakerPos = audioSpeakers[i].transform.position;
            dist = Vector3.Distance(player.transform.position, speakerPos);

            float distY = Mathf.Abs(player.transform.position.y - speakerPos.y);

            if (dist < shortestDist && distY < 1f)
            {
                newSpeaker = audioSpeakers[i];
                shortestDist = dist;
            }
        }

        if (newSpeaker != curSpeaker)
        {
            curSpeaker = newSpeaker;
            ChangePositon();

        }

    }

    // Update is called once per frame
    public void ChangePositon()
    {
        this.transform.localPosition = curSpeaker.transform.localPosition;
    }

    void OnDrawGizmos()
    {

        for (int i = 0; i < audioSpeakers.Count; i++)
        {
            if (curSpeaker == audioSpeakers[i])
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(player.transform.position, audioSpeakers[i].transform.position);
            }
            else
            {
                //Gizmos.color = Color.white;
                //Gizmos.DrawLine(player.transform.position, audioSpeakers[i].transform.position);
            }
        }

    }

     

}
