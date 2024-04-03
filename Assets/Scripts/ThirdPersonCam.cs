using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    // [Header("References")]
    // public Transform orientation;
    // public Transform player;
    // public Transform playerObj;
    

    [Header("Movement")]
    public float moveSpeed;
    // public float rotationSpeed;
    // public float jumpForce = 2.0f;
    // public bool isOnGround = true;

    // public float groundDrag;
    // [Header("Ground Check")]
    // public float playerHeight;
    // public LayerMask whatIsGround;
    // bool grounded;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public Rigidbody rb;

    private void Start()
    {
        // Hide cursor
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        // Freeze the rotation of the bodie to prevent it from falling
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // Player jump
        // if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        // {
        //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //     isOnGround = false;
        // }

        // Handle drag
        // if (isOnGround){
        //     rb.drag = groundDrag;
        // }
        // else {
        //     rb.drag = 0;
        // }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocitu if needed
        if (flatVel.magnitude >moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Update()
    {
        MyInput();
        // SpeedControl();

        // Rotate orientation
        // Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        // orientation.forward = viewDir.normalized;

        // Rotate player object
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");
        // Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // if (inputDir != Vector3.zero){
        //     playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        // }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground")){
    //        isOnGround = true;
    //    }
    //}
}
