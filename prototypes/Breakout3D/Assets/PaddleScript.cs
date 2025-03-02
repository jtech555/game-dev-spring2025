using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections; //used to allow IEnumerator ResetScale()


public class PaddleScript : MonoBehaviour
{
    public Transform target; // Center of the sphere
    public Transform cameraTransform; // Camera transform
    public float radius = 5f; // Radius of the sphere for the object
    public float cameraRadius = 7f; // Radius of the sphere for the camera
    public float speed = 1f; // Speed of rotation
    private float angle = 0f; // Horizontal angle
    private float verticalAngle = 0f; // Vertical angle


    // The scale increase factors
    public Vector3 scaleIncrease = new Vector3(2f, 2f, 1f); // Example: Double the X and Y scale
    private Vector3 originalScale;
    

    private void Start()
    {
        // Store the original scale of the cube
        originalScale = transform.localScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Increase the scale of the cube when collision happens
        transform.localScale = originalScale + scaleIncrease;

        // Start the coroutine to reset the scale after 0.5 seconds
        StartCoroutine(ResetScale());
    }

    IEnumerator ResetScale()
    {
        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.2f);

        // Reset the scale back to original
        transform.localScale = originalScale;
    }

    void Update()
    {
        // Calculate the new position for the player-controlled object
        float objectX = target.position.x + Mathf.Cos(angle) * Mathf.Cos(verticalAngle) * radius;
        float objectY = target.position.y + Mathf.Sin(verticalAngle) * radius;
        float objectZ = target.position.z + Mathf.Sin(angle) * Mathf.Cos(verticalAngle) * radius;


        // Ensure objectY doesn't exceed 4.6
        if (objectY >= 4.6f)
        {
            objectY = 4.6f;
        }
        if (objectY <= -4.6f)
        {
            objectY = -4.6f;
        }

        transform.position = new Vector3(objectX, objectY, objectZ);

 


    // Make the object face the target
    transform.LookAt(target);

        

        // Update angles based on input
        if (Input.GetKey(KeyCode.A)) // Left
        {
            angle -= speed * Time.deltaTime; // Move around horizontally (left)
        }
        if (Input.GetKey(KeyCode.D)) // Right
        {
            angle += speed * Time.deltaTime; // Move around horizontally (right)
        }
        if (Input.GetKey(KeyCode.W)) // Up
        {
            verticalAngle += speed * Time.deltaTime; // Move up vertically
        }
        if (Input.GetKey(KeyCode.S)) // Down
        {
            verticalAngle -= speed * Time.deltaTime; // Move down vertically
        }

        // Wrap angles to prevent overflow
        angle = Mathf.Repeat(angle, Mathf.PI * 2); // Keep horizontal angle within 0 to 2π
        verticalAngle = Mathf.Repeat(verticalAngle, Mathf.PI * 2); // Keep vertical angle within 0 to 2π

        // Update the camera's position and rotation
        if (cameraTransform != null)
        {
            // Calculate the new position for the camera
            float cameraX = target.position.x + Mathf.Cos(angle) * Mathf.Cos(verticalAngle) * cameraRadius;
            float cameraY = target.position.y + Mathf.Sin(verticalAngle) * cameraRadius;
            float cameraZ = target.position.z + Mathf.Sin(angle) * Mathf.Cos(verticalAngle) * cameraRadius;

            cameraTransform.position = new Vector3(cameraX, cameraY, cameraZ);

            // Make the camera face the target
            cameraTransform.LookAt(target);
        }
    }
}
