using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    public float rotateAmount;
    Camera mainCam;
    public bool noBoard;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        //rotateAmount = 0f;
    }

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
