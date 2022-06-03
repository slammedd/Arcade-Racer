using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class WheelController : MonoBehaviour
{
    public GameObject[] wheelsToRotate;
    public float rotationSpeed;
    private Animator anim;
    public TrailRenderer[] trails;
    private CarController carCont;
    public ParticleSystem[] wheelSmokeParticleSystems;
    public AudioSource wheelSource;
    public ParticleSystem[] offTrackSmokeParticleSystems;
    
    public CinemachineVirtualCamera vCam;

    private bool isPlaying;
    private bool isOffTrack;

    private void Start()
    {
        anim = GetComponent<Animator>();
        carCont = GetComponent<CarController>();

        foreach (ParticleSystem ps in wheelSmokeParticleSystems)
        {
            ps.Stop();
        }

        foreach (ParticleSystem ps in offTrackSmokeParticleSystems)
        {
            ps.Stop();
        }
    }

    private void Update()
    {
        float verticalAxis = Input.GetAxisRaw("Vertical");
        float horizontalAxis = Input.GetAxisRaw("Horizontal");

        foreach (GameObject wheel in wheelsToRotate)
        {
            wheel.transform.Rotate(Time.deltaTime * verticalAxis * rotationSpeed, 0, 0, Space.Self);
        }

        if(horizontalAxis > 0)
        {
            anim.SetBool("left", false);
            anim.SetBool("right", true);
        }

        else if(horizontalAxis < 0)
        {
            anim.SetBool("right", false);
            anim.SetBool("left", true);
        }

        else
        {
            anim.SetBool("right", false);
            anim.SetBool("left", false);
        }

        if (horizontalAxis != 0 && carCont.isGrounded == true && verticalAxis != 0 && carCont.forwardSpeed >= carCont.maxForwardSpeed * 0.5f && !isOffTrack)
        {
            foreach(TrailRenderer trail in trails)
            {
                trail.emitting = true;
            }

            if (!isPlaying)
            {
                wheelSource.DOFade(0.1f, 1);
                isPlaying = true;
                
                foreach (ParticleSystem ps in wheelSmokeParticleSystems)
                {
                    ps.Play();
                }
            }
        }

        else
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = false;
            }

         
                wheelSource.DOFade(0, 0.25f);
                isPlaying = false;

                foreach (ParticleSystem ps in wheelSmokeParticleSystems)
                {
                    ps.Stop();
                }
           
        }

        if (carCont.offTrack == false)
        {

            isOffTrack = false;

            foreach (ParticleSystem ps in offTrackSmokeParticleSystems)
            {
                ps.Stop();
            }
        }

        else if (carCont.offTrack)
        {

            isOffTrack = true;
           
            if(carCont.forwardSpeed > 0)
            {
                foreach (ParticleSystem ps in offTrackSmokeParticleSystems)
                {
                    ps.Play();
                }

                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0.5f;
            }
            
            else
            {
                foreach (ParticleSystem ps in offTrackSmokeParticleSystems)
                {
                    ps.Stop();
                }

                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
            
        }

        print(horizontalAxis);
    }
}
