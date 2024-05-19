using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityScale = 5;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;

    [SerializeField] private Transform orientation;

    [Header("KeyBinds")]
    [SerializeField] private KeyCode jumpKey;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public Vector3 moveDirection;

    [HideInInspector] public bool canMove = true;

    private bool grounded;
    private bool redyToJump;

    private float horizontalInput;
    private float verticalInput;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        redyToJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
       if(canMove)
        MovePLayer();

        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && grounded && redyToJump)
        {
            redyToJump = false;

            Jump();

            Invoke("ResetJump", jumpCooldown);
        }
    }

    private void MovePLayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }
    
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3 (limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        redyToJump = true;
    }

}
