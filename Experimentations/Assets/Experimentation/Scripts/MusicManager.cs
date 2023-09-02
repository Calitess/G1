using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] public AudioSpeaker curSpeaker;

    [SerializeField] List<AudioSpeaker> audioSpeakers = new List<AudioSpeaker>();


    public float dist, shortestDist;



    private void Update()
    {

        for (int i = 0; i < audioSpeakers.Count; i++)
        {


            Vector3 speakerPos = audioSpeakers[i].transform.position;

            dist = Vector3.Distance(player.transform.position, speakerPos);

            if (dist < shortestDist)
            {
                curSpeaker = audioSpeakers[i];
                ChangePositon();
            }
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
                Gizmos.color = Color.white;
                Gizmos.DrawLine(player.transform.position, audioSpeakers[i].transform.position);
            }
        }

    }

     

}
