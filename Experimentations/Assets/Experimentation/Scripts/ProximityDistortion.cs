using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDistortion : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float activationDistance = 5, distancePadding = 2, normalizedDist = 0, timeToCloseRift = 0.5f;
    public bool enableProximityDistort = true;
    private Material distortionMaterial;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        distortionMaterial = meshRenderer.material;

    }

    // Update is called once per frame
    void Update()
    {
        if (enableProximityDistort)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            normalizedDist = 1 - Mathf.Clamp01((distance - distancePadding) / activationDistance);
            distortionMaterial.SetFloat("_NormalizeAmount", normalizedDist);
        }
    }

    public void disableProximityDistort()
    {
        enableProximityDistort = false;

        StartCoroutine(closeRift());
    }

    public void EnableProximityDistort()
    {
        enableProximityDistort = true;

    }

    IEnumerator closeRift()
    {
        yield return new WaitForSeconds(timeToCloseRift);
        StartCoroutine(LerpFunction(0, 2));

    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = normalizedDist;
        while (time < duration)
        {
            normalizedDist = Mathf.Lerp(startValue, endValue, time / duration);


            distortionMaterial.SetFloat("_NormalizeAmount", normalizedDist);
            time += Time.deltaTime;
            yield return null;
        }
        normalizedDist = endValue;

        distortionMaterial.SetFloat("_NormalizeAmount", normalizedDist);

        this.gameObject.SetActive(false);
    }


}
