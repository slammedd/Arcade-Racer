using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheelController : MonoBehaviour
{
    public GameObject[] wheelsToRotate;
    public float rotationSpeed;
    private Animator anim;
    public TrailRenderer[] trails;
    private CarController carCont;
    public ParticleSystem[] wheelSmokeParticleSystems;
    public AudioSource wheelSource;

    private bool isPlaying;

    private void Start()
    {
        anim = GetComponent<Animator>();
        carCont = GetComponent<CarController>();
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

        if (horizontalAxis != 0 && carCont.isGrounded == true && verticalAxis > 0 && carCont.forwardSpeed >= carCont.maxForwardSpeed * 0.5f)
        {
            foreach(TrailRenderer trail in trails)
            {
                trail.emitting = true;
            }

            foreach(ParticleSystem ps in wheelSmokeParticleSystems)
            {
                ps.Play();
            }

            if (!isPlaying)
            {
                wheelSource.DOFade(0.1f, 1);
                isPlaying = true;
            }
        }

        else
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = false;
            }

            foreach (ParticleSystem ps in wheelSmokeParticleSystems)
            {
                ps.Stop();
            }

            if (isPlaying && horizontalAxis == 0)
            {
                wheelSource.DOFade(0, 0.25f);
                isPlaying = false;
            }
        }
    }
}
