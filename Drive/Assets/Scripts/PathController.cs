using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics; // check performance gain
using System;

public class PathController : MonoBehaviour {

    public Transform seeker, target;
    private Vector3[] path;
    Grid grid; 
	
    void Awake() {
        grid = GetComponent<Grid>();
    }

    void Update() {
        FindPath(seeker.position, target.position);
    }

    public Vector3[] getPath() { // getter
        return path;
    }

	void FindPath(Vector3 startPos, Vector3 targetPos) {  // performs A* search on the grid to find a path
        startPos = new Vector3(startPos.x + 8.0f, 0, startPos.z - 2.0f); // offsets
        targetPos = new Vector3(targetPos.x + 8.0f, 0, targetPos.z - 2.0f); // offsets

        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        if (targetNode.walkable == false) return; // don't try to path find if we're on an unwalkable area

		Heap<Node> openSet = new Heap<Node>(grid.MaxSize); 
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);
        while(openSet.Count > 0) { // we still have nodes 
			Node currentNode = openSet.pop();
            closedSet.Add(currentNode);
            if(currentNode == targetNode) { // we've found exit
                RetracePath(startNode, targetNode);
                path = backTrackPath(startNode, targetNode);
                return;
            }
            foreach(Node n in grid.GetNeighbours(currentNode)) {
                if (!n.walkable || closedSet.Contains(n)) continue;
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, n);
                if(newMovementCostToNeighbour < n.gCost || !openSet.contains(n)) {
                    n.gCost = newMovementCostToNeighbour;
                    n.hCost = GetDistance(n, targetNode);
                    n.parent = currentNode;

                    if (!openSet.contains(n)) openSet.Add(n); // add our neighbour into open set
					else openSet.UpdateItem(n);
                }
            }
        }
    }

    void RetracePath(Node start, Node end) {
        List<Node> path = new List<Node>();
        Node current = end;
        while(current != start) {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse(); // TODO: optimize
        grid.path = path;
    }

    Vector3[] backTrackPath(Node start, Node end) {
        List<Node> path = new List<Node>();
        Node current = end;
        while (current != start) {
            path.Add(current);
            current = current.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld) {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node a, Node b) {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);  // pythagorean approx
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
