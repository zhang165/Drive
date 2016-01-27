using UnityEngine;
using System.Collections;

public class WheelController : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(5, 0, 0) * Time.deltaTime);
    }
}
