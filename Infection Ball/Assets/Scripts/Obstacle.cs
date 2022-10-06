using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public Material infectionMaterial;
    public GameObject explosion;
   
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        
    }

    public void Infect()
    {
        meshRenderer.material = infectionMaterial;
        StartCoroutine(destruction());
    }

    IEnumerator destruction()
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
