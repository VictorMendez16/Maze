using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Health playerHealth;
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public AudioSource footStepsSound;
    public AudioSource jumpSound;
    public AudioSource takeDamageSound;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground check")]
    public bool isOnGround = true;

    public Transform orientation;

    bool readyToJump = true;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerHealth = GetComponent<Health>();
        // Waits 30 seconds and then calls the method to take damage every 1 second
        InvokeRepeating(nameof(playerHealth.takeDamage), 30, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            CancelInvoke(nameof(takeDamage));
        }
        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (!PauseMenu.isPaused)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // When to jump
            if (Input.GetKey(jumpKey) && readyToJump && isOnGround)
            {
                readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // Handle walking effects
        if ((Input.GetAxis("Horizontal") !=0) || (Input.GetAxis("Vertical") != 0) && isOnGround)
        {
            footStepsSound.enabled = true;
        }else
        {
            footStepsSound.enabled = false;
        }

        // Handle drag
        if (isOnGround)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            rb.drag = groundDrag;
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            rb.drag = 0;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        jumpSound.enabled = true;
        isOnGround = false;

        // Velocity to always jump the same height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Win()
    {
        // Load next scene by index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpSound.enabled = false;
            isOnGround = true;
        }
        if (collision.gameObject.CompareTag("SafeArea"))
        {
            Win();
        }
    }

    private void takeDamage(){
        playerHealth.takeDamage();
        takeDamageSound.enabled = true;
        takeDamageSound.Play();
    }
}
