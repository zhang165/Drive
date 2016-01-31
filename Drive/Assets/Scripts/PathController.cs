using UnityEngine;
using System.Collections;

public class PathController : MonoBehaviour {
	Vector3 newPosition;

	void Start(){
		newPosition = transform.position;
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) { // if we click, we want to move an object
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {  // TODO: fix bug with placing object on edge
				newPosition = new Vector3(hit.point.x, 0, hit.point.z) + new Vector3(0,1.2f,0); // zero on hit.point.y so we cannot place target on top of buildings
				transform.position = newPosition;
			}
		}
	}
}
