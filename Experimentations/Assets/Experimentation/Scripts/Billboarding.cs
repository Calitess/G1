using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    public float rotateAmount;
    [SerializeField] public Camera mainCam;
    public bool noBoard;


    // Update is called once per frame
    void LateUpdate()
    {
        if (!noBoard)
        {
            transform.LookAt(mainCam.transform);

        }
        else
        {
            transform.rotation = mainCam.transform.rotation;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x * rotateAmount, transform.rotation.eulerAngles.y, 0f);

    }
}
