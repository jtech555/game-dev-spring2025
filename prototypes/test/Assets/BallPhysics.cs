using System.Data;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Vector3 velocity;
    Vector3 acceleration;

    [SerializeField]
    Camera cam; //create reference to camera - we drag Main Camera into here

    Vector3 gravity = new Vector3(9.8f, 0, 0);

void Start()
    {
        acceleration = Vector3.zero; //makes 0
        velocity = new Vector3(0, 0, 0);
        //velocity.Normalize() //maintain the direction but scale it down to 0
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            acceleration += gravity;
        }

        // red is mouse world
        //blue is 3d world
        Vector3 ballInScreenCoordinateSpace = cam.WorldToScreenPoint(transform.position);
        Vector3 directionToMouse = Input.mousePosition - ballInScreenCoordinateSpace;
        directionToMouse = directionToMouse.normalized;
        directionToMouse.z = 0;// takes away call coming towards player
        acceleration += directionToMouse* 9.8f; //makes as strong as gravity
        velocity = Vector3.ClampMagnitude(velocity, 10); //limits how fast we are moving

        //length of line is magnitude of vector
        Debug.DrawRay(transform.position, acceleration/5, Color.green);

        /*
         * 
         *  ball+??=mouse
         *  mouse-ball=?
         *
         * 
         */

        //make force go to mouse pos
        //Input.mousePosition //mouse exists on a screen.. 0,0 might be bottom left

       // acceleration +=gravity;
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        acceleration= Vector3.zero; 
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided with" + collision.gameObject.name); //sees if ball hit something
        if (collision.gameObject.CompareTag("paddle"))
        {
            velocity.y *= 1; //reverse y
        }
    }
}