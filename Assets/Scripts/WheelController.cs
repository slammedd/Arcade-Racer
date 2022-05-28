using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public GameObject[] wheelsToRotate;
    public float rotationSpeed;
    private Animator anim;
    public TrailRenderer[] trails;
    private CarController carCont;

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

        if(horizontalAxis != 0 && carCont.isGrounded == true)
        {
            foreach(TrailRenderer trail in trails)
            {
                trail.emitting = true;
            }
        }

        else
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = false;
            }
        }
    }
}
