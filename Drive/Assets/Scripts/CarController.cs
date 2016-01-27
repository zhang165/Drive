using UnityEngine;
using System.Collections;
using System;

public class CarController : MonoBehaviour
{
    private float speed;
    public float maxSpeed;
    public float backSpeed;
	public float autopilotSpeed;
    public float acceleration;//This is the maximum speed that the object will achieve
    public float turnSpeed;

    private float curPos;
    private float lastPos;
    private Rigidbody rb;
    float moveSpeed;
	public float collisionThreshold;
	private bool autopilot;

	public Transform target;

	private Quaternion lookRotation;
	private Vector3 direction;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += new Vector3(0, 0, 1);
		autopilot = false;
    }

	void Update(){
		if (Input.GetKeyDown (KeyCode.C)) {
			autopilot = !autopilot;
		}
		bool isFar = (Vector3.Distance (transform.position, target.position) > collisionThreshold);
		// always move towards our object in autopilot mode
		if (autopilot && isFar) {
			transform.position = Vector3.MoveTowards (transform.position, target.position, autopilotSpeed);
		}
		// rotate towards our object in autopilot mode
		if (autopilot && isFar) {
			direction = (target.position - transform.position).normalized;
			lookRotation = Quaternion.LookRotation (direction);
			transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
		}
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        
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
