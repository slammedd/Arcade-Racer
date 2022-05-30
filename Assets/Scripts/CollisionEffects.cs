using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffects : MonoBehaviour
{
    public ParticleSystem impactEffect;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(impactEffect, collision.GetContact(0).point, Quaternion.identity);
    }
}
