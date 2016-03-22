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

    PathController pathcontroller;
    Vector3[] path; // path to follow in autopilot mode
    int targetIndex = 0;
        
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += new Vector3(0, 0, 1);
		autopilot = false;
        pathcontroller = GetComponent<PathController>();
    }

	void Update(){
		if (Input.GetKeyDown (KeyCode.C)) {
			autopilot = !autopilot;
		}
        
		bool isFar = (Vector3.Distance (transform.position, target.position) > collisionThreshold);
        // always move towards our object in autopilot mode
        path = pathcontroller.getPath();
        if (autopilot && isFar) {
            path = pathcontroller.getPath();
            StopCoroutine("FollowPath"); // stop previous enumeration
            StartCoroutine("FollowPath"); // start new enumeration
        }
	}

    IEnumerator FollowPath() { // use IEnumerator to move car towards path
        Vector3 currentWaypoint = path[0];
        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, autopilotSpeed * Time.deltaTime);
            direction = (currentWaypoint - transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);  // remove Y axis rotation
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            yield return null;
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        if (rb.IsSleeping()) speed = 0; // set acceleration
        speed += acceleration;
        if (speed > maxSpeed) speed = maxSpeed;
        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(-Vector3.forward * backSpeed * Time.deltaTime);
            speed = 0;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(0, x, 0) * turnSpeed);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        speed = 0;
    }

    public bool getAutopilot()
    {
        return autopilot;
    }
}
