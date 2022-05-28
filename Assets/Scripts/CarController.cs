using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    
    private float moveInput;
    private float turnInput;
    [HideInInspector] public bool isGrounded;
    private float normalDrag;

    public Rigidbody sphereRB;
    public Rigidbody carRB;
    public float modifiedDrag;
    public float gravityScale;
    public float alignToGroundTime;
    public float forwardSpeed;
    public float reverseSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;
    

    private void Start()
    {
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;

        normalDrag = sphereRB.drag;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        transform.Rotate(0,newRotation, 0, Space.World);

        transform.position = sphereRB.transform.position;

        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f , groundLayer);

        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);

        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;

        sphereRB.drag = isGrounded ? normalDrag : modifiedDrag;
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
