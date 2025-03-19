using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform childObject;
    public Transform cameraTransform;
    public float cameraDistance = 5f; // How far the camera is from the player
    public float cameraHeight = 2f;   // How high the camera is from the ground level

    float rotateSpeed = 110f;
    float forwardSpeed = 18.5f;
    float jumpForce = 20f;
    float gravity = -19.8f;

    float yVelocity = 0;

    public CharacterController cc;
    public Animator animator;

    private int jumpCount = 0;
    private const int maxJumps = 2;

    public float climbSpeed = 3f;
    private bool isGoingUpLadder = false;

    public float stompSpeed = -80f;
    private bool isStomping = false;

    Boolean inWater = false;

    void Start()
    {
        // Set the camera position initially
        UpdateCameraPosition();
    }

    void Update()
    {
        // Get horizontal (left/right) and vertical (forward/back) axis inputs
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        // Handle ladder climbing
        if (isGoingUpLadder)
        {
            Vector3 climbMovement = new Vector3(0, vAxis * climbSpeed, 0);
            cc.Move(climbMovement * Time.deltaTime);

            if (Input.GetKey(KeyCode.Q))
            {
                Vector3 leftMovement = -transform.right * forwardSpeed * Time.deltaTime;
                cc.Move(leftMovement);
            }
            if (Input.GetKey(KeyCode.E))
            {
                Vector3 rightMovement = transform.right * forwardSpeed * Time.deltaTime;
                cc.Move(rightMovement);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGoingUpLadder = false;
                yVelocity = jumpForce;
                Vector3 jumpDirection = -transform.forward + transform.up;
                cc.Move(jumpDirection * jumpForce * Time.deltaTime);
            }
        }
        else
        {
            // Jumping logic when grounded
            if (cc.isGrounded)
            {
                jumpCount = 0;
                yVelocity = -5;
                isStomping = false;
                if (Input.GetKeyDown(KeyCode.Space))  // Jump only on pressing Space
                {
                    yVelocity = jumpForce;
                    jumpCount++;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
                {
                    yVelocity = jumpForce;
                    jumpCount++;
                    childObject.localPosition = Vector3.zero;
                    childObject.localRotation = Quaternion.identity;
                }

                if (jumpCount == maxJumps && Input.GetKeyDown(KeyCode.S))
                {
                    isStomping = true;
                }

                if (isStomping)
                {
                    yVelocity = stompSpeed;
                }
                else
                {
                    if (yVelocity < 0) yVelocity += gravity / 2 * Time.deltaTime;
                    else yVelocity += gravity * Time.deltaTime;
                }

                if (Input.GetKeyUp(KeyCode.Space) && yVelocity > 0)
                    yVelocity = 0;
            }

            // Handle movement and rotation
            // Only horizontal axis should affect rotation (left/right turning)
            if (hAxis != 0)
            {
                float rotationAngle = hAxis * rotateSpeed * Time.deltaTime;
                transform.Rotate(0, rotationAngle, 0);
            }

            // Forward/backward movement
            Vector3 forwardMovement = transform.forward * forwardSpeed * vAxis;
            forwardMovement.y = yVelocity;  // Set the vertical component (jumping/falling)
            cc.Move(forwardMovement * Time.deltaTime);
        }

        // Update camera position to always stay behind the player
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // Position the camera behind the player based on the player's rotation
        Vector3 desiredPosition = transform.position - transform.forward * cameraDistance + Vector3.up * cameraHeight;
        cameraTransform.position = desiredPosition;

        // Ensure the camera is always looking at the back of the player
        cameraTransform.LookAt(transform.position + Vector3.up * cameraHeight);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("water"))
        {
            inWater = true;
            forwardSpeed = 1;
        }
        else if (other.CompareTag("Ladder"))
        {
            isGoingUpLadder = true;
            yVelocity = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("water"))
        {
            forwardSpeed = 18.5f;
            inWater = false;
        }
        else if (other.CompareTag("Ladder"))
        {
            isGoingUpLadder = false;
        }
    }
}
