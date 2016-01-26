using UnityEngine;
using System.Collections;
using System;

public class CarController : MonoBehaviour
{
    private float speed;
    public float maxSpeed;
    public float backSpeed;
    public float acceleration;//This is the maximum speed that the object will achieve
    public float turnSpeed;

    private float curPos;
    private float lastPos;
    private Rigidbody rb;
    float moveSpeed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(0, 0, z);
        if (rb.IsSleeping()) speed = 0; // set acceleration
        speed += acceleration;
        if (speed > maxSpeed) speed = maxSpeed;
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * backSpeed);
            speed = 0;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
        {
            transform.Rotate(new Vector3(0, x, 0) * turnSpeed);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        speed = 0;
    }
}
