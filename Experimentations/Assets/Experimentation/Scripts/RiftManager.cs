using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftManager : MonoBehaviour
{
    public Interactor curInteractor;
    public List<Interactor> riftInteractors = new List<Interactor>();

    
    public void ActivateThisRift(int elementIndexOfRiftList)
    {
        for(int i=0; i<riftInteractors.Count; i++)
        {
            if(i!=elementIndexOfRiftList)
            {
                riftInteractors[i].gameObject.SetActive(false);
            }
            else if(i == elementIndexOfRiftList)
            {
                riftInteractors[i].gameObject.SetActive(true);
                curInteractor = riftInteractors[i];
            }
        }
    }
}
