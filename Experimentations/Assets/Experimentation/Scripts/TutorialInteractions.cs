using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteractions : MonoBehaviour
{
    [SerializeField] public List<CustomCommands> Interactables;
    RiftManager riftManager;

    public void EvaluateInteractions(CustomCommands interactable)
    {
        riftManager = FindObjectOfType<RiftManager>();
        Interactables.Remove(interactable);

        if (Interactables.Count == 0)
        {
            riftManager.ActivateThisRift(0);
        }

    }
}
