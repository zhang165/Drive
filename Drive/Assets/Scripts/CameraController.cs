using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Camera camera1;
	public Camera camera2;
	public bool active;
    public GameObject player;

    private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
		camera1.gameObject.SetActive (true);
		camera2.gameObject.SetActive (false);
		active = true;
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.C)) {
			if (active) {
				camera2.gameObject.SetActive (true);
				camera1.gameObject.SetActive (false);
				active = false;
			} else {
				camera1.gameObject.SetActive (true);
				camera2.gameObject.SetActive (false);
				active = true;
			}	
		}
	}
	// Update is called once per frame
	void LateUpdate () {
				transform.position = player.transform.position + offset;
	}
}
