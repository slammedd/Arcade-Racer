using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffects : MonoBehaviour
{
    public ParticleSystem impactEffect;
    public AudioSource impactSource;
    public AudioClip impactSound;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(impactEffect, collision.GetContact(0).point, Quaternion.identity);
        impactSource.PlayOneShot(impactSound);
    }
}
