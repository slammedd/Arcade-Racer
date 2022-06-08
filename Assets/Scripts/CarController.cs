using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private float moveInput;
    private float turnInput;
    [HideInInspector] public bool isGrounded;
    private float normalDrag;
    private float enginePitch;
    [HideInInspector] public bool offTrack;
    private float initialMForwardSpeed;
    private float initialMReverseSpeed;

    [Header("Assignables")]
    public Rigidbody sphereRB;
    public Rigidbody carRB;
    public LayerMask groundLayer;
    public LayerMask offTrackLayer;
    public Material breakLightMaterial;
    public AudioSource engineSource;
    public AudioClip engineSound;
    public ParticleSystem speedLines;

    [Header("Physics Controls")]
    public float modifiedDrag;
    public float gravityScale;
    public float alignToGroundTime;
    [HideInInspector] public float forwardSpeed;
    public float maxForwardSpeed;
    public float maxForwardSpeedOffTrack;
    public float forwardAcceleration;
    public float stoppingAcceleration;
    public float reverseAcceleration;
    [HideInInspector] public float reverseSpeed;
    public float maxReverseSpeed;
    public float maxReverseSpeedOffTrack;
    public float turnSpeed;

    [Header("Misc")]
    public float breakLightIntensity;
    public float reverseLightIntensity;

    private void Start()
    {
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;

        normalDrag = sphereRB.drag;

        engineSource.clip = engineSound;

        initialMForwardSpeed = maxForwardSpeed;
        initialMReverseSpeed = maxReverseSpeed;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            if (forwardSpeed < maxForwardSpeed)

            {
                forwardSpeed += Time.deltaTime * forwardAcceleration;
            }
            else
            {
                forwardSpeed = maxForwardSpeed;
            }

            breakLightMaterial.SetColor("_EmissionColor", new Color(191, 0, 0) * 0);
            breakLightMaterial.SetColor("_BaseColor", new Color(1, 0.76f, 0.36f));
        }

        else
        {
            if (forwardSpeed > 0)
            {
                forwardSpeed -= Time.deltaTime * stoppingAcceleration;
            }

            breakLightMaterial.SetColor("_EmissionColor", new Color(191, 0, 0) * breakLightIntensity);
            breakLightMaterial.SetColor("_BaseColor", new Color(0.95f, 0.26f, 0.26f));

        }

        if (moveInput < 0)
        {
            if (reverseSpeed < maxReverseSpeed)

            {
                reverseSpeed += Time.deltaTime * reverseAcceleration;
            }
            else
            {
                reverseSpeed = maxReverseSpeed;
            }

            breakLightMaterial.SetColor("_BaseColor", new Color(1, 1, 1));
            breakLightMaterial.SetColor("_EmissionColor", new Color(255, 255, 255) * reverseLightIntensity);
        }

        else
        {
            if(reverseSpeed > 0)
            {
                reverseSpeed -= Time.deltaTime * stoppingAcceleration;
            }
        }

        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        transform.Rotate(0, newRotation, 0, Space.World);

        transform.position = sphereRB.transform.position;

        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);

        RaycastHit hit1;
        offTrack = Physics.Raycast(transform.position, -transform.up, out hit1, 2f, offTrackLayer);

        if (offTrack)
        {
            maxForwardSpeed = maxForwardSpeedOffTrack;
            maxReverseSpeed = maxReverseSpeedOffTrack;
        }

        else if (!offTrack)
        {
            maxForwardSpeed = initialMForwardSpeed;
            maxReverseSpeed = initialMReverseSpeed;
        }

        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;

        sphereRB.drag = isGrounded ? normalDrag : modifiedDrag;

        if (moveInput > 0)
        {
            engineSource.pitch = 0.5f + forwardSpeed / maxForwardSpeed;
        }
        else engineSource.pitch = 0.5f + reverseSpeed / maxReverseSpeed;

        if(forwardSpeed == maxForwardSpeed)
        {
            speedLines.Play();
        }

        else
        {
            speedLines.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }

        else
        {
            sphereRB.AddForce(transform.up * gravityScale);
        }

        carRB.MoveRotation(transform.rotation);
    }
}
