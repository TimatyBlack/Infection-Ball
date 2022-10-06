using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitParticle;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            AoE();

            Instantiate(hitParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(collision.gameObject.TryGetComponent<Gates>(out Gates gates))
        {
            Instantiate(hitParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void AoE()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x * 1.5f);
        foreach(Collider c in colliders)
        {
            if (c.TryGetComponent<Obstacle>(out Obstacle obstacle))
            {
                obstacle.Infect();
            }
        }    
    }
}
