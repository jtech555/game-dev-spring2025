using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform childObject; 

    float rotateSpeed = 90f;
    float forwardSpeed = 12.5f;
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
    }

    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 amountToRotate = new Vector3(0, 0, 0);
        amountToRotate.y = hAxis * rotateSpeed * Time.deltaTime;
        transform.Rotate(amountToRotate, Space.Self);

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
            if (cc.isGrounded)
            {
                jumpCount = 0;
                yVelocity = -5;
                isStomping = false;
                if (Input.GetKeyDown(KeyCode.Space))
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
                    if (yVelocity < 0)yVelocity += gravity / 2 * Time.deltaTime;
                    else yVelocity += gravity * Time.deltaTime;
                }

                if (Input.GetKeyUp(KeyCode.Space) && yVelocity > 0)
                    yVelocity = 0;

              
            }
            if (inWater == true)
            {

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    forwardSpeed = 6;
                    childObject.localRotation = Quaternion.Euler(99f, childObject.localRotation.eulerAngles.y, childObject.localRotation.eulerAngles.z);
                    childObject.localPosition = new Vector3(childObject.localPosition.x, 1.04f, -2.73f);

                }
           
           
                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    forwardSpeed = 1;
                    childObject.localPosition = Vector3.zero; 
                    childObject.localRotation = Quaternion.identity; 
                }
            }
            Vector3 amountToMove = transform.forward * forwardSpeed * vAxis;
            amountToMove.y = yVelocity;
            amountToMove *= Time.deltaTime;

        
            cc.Move(amountToMove);
        }
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
            forwardSpeed = 8f;
            inWater = false;
        }
        else if (other.CompareTag("Ladder"))
        {
            isGoingUpLadder = false;
        }
    }
}