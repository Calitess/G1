using Invector.vCharacterController.vActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInteractable : MonoBehaviour
{
    vTriggerGenericAction vAction;
    [SerializeField] public GameObject InteractableIcon;
    [HideInInspector] public bool inDialogue;

    private void Start()
    {
        vAction = this.GetComponent<vTriggerGenericAction>();
    }

    private void Update()
    {
        if(vAction!=null)
        {
            if (vAction.enabled == false || inDialogue == true)
            {
                InteractableIcon.SetActive(false);
            }
            else if (vAction.enabled == true)
            {

                InteractableIcon.SetActive(true);
            }

        }
    }
}
