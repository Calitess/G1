using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxStrength = 100f;
    [SerializeField] Vector2 parallaxClamp;

    private Vector2 startPosition;
    [SerializeField] private Camera _cam;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        //_cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
        float posX = Mathf.Lerp(transform.position.x, startPosition.x + (mousePos.x *parallaxStrength), 5f * Time.deltaTime);
        float posY = Mathf.Lerp(transform.position.y, startPosition.y + (mousePos.y * parallaxStrength), 5f * Time.deltaTime);

        posX = Mathf.Clamp(posX, startPosition.x - parallaxClamp.x, startPosition.x + parallaxClamp.x);
        posY = Mathf.Clamp(posY, startPosition.y - parallaxClamp.y, startPosition.y + parallaxClamp.y);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}
