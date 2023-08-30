using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDistortion : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float activationDistance = 5, distancePadding = 2, normalizedDist = 0;
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
        float distance = Vector3.Distance(target.position, transform.position);
        normalizedDist = 1 - Mathf.Clamp01((distance - distancePadding) / activationDistance);
        distortionMaterial.SetFloat("_NormalizeAmount", normalizedDist);
    }

   
}
