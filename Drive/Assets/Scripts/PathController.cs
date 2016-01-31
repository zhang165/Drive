using UnityEngine;
using System.Collections;

public class PathController : MonoBehaviour {

    Grid grid;
	// Use this for initialization
    void Awake() {
        grid = GetComponent<Grid>();
    }

	void FindPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);
    }
}
