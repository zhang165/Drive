using UnityEngine;
using System.Collections;

public class SlowSpin : MonoBehaviour {

    public float turnSpeed;
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 15, 0) * Time.deltaTime * turnSpeed);
    }
}
