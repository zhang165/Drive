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
			if (Physics.Raycast (ray, out hit)) {
				newPosition = hit.point + new Vector3(0,1.2f,0);
				transform.position = newPosition;
			}
		}
	}
}
