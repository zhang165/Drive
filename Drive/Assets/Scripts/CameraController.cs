using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Camera camera1;
	public Camera camera2;
	public bool active;
    public GameObject player;

    private Vector3 offset;

    float cameraDistanceMax = 20f;
    float cameraDistanceMin = 5f;
    float cameraDistance = 10f;
    float scrollSpeed = 0.5f;

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
				active = !active;
			} else {
				camera1.gameObject.SetActive (true);
				camera2.gameObject.SetActive (false);
				active = !active;
			}
        }
        cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
    }

	void LateUpdate () {
			transform.position = player.transform.position + offset;
            
	}
}
