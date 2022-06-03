using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CollisionEffects : MonoBehaviour
{
    public ParticleSystem impactEffect;
    public AudioSource impactSource;
    public AudioClip impactSound;
    public CinemachineVirtualCamera vCam;
 

    private float shakeTimer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Impactable"))
        {
            Instantiate(impactEffect, collision.GetContact(0).point, Quaternion.identity);
            impactSource.PlayOneShot(impactSound);
            ScreenShake(5f, 0.1f);
        }
    }

    public void ScreenShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
