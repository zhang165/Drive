using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathController : MonoBehaviour {

    public Transform seeker, target;
    Grid grid; 
	
    void Awake() {
        grid = GetComponent<Grid>();
    }

    void Update() {
        FindPath(seeker.position, target.position);
    }

	void FindPath(Vector3 startPos, Vector3 targetPos) {  // performs A* search on the grid to find a path
        startPos = new Vector3(startPos.x + 8.0f, 0, startPos.z - 2.0f); // offsets
        targetPos = new Vector3(targetPos.x + 8.0f, 0, targetPos.z - 2.0f); // offsets

        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        if (targetNode.walkable == false) return; // don't try to path find if we're on an unwalkable area

        List<Node> openSet = new List<Node>(); 
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);
        while(openSet.Count > 0) { // we still have nodes 
            Node currentNode = openSet[0];
            for(int i=1; i<openSet.Count; i++) { // TODO: inefficient, implement a heap for this
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode) { // we've found exit
                RetracePath(startNode, targetNode);
                return;
            }
            foreach(Node n in grid.GetNeighbours(currentNode)) {
                if (!n.walkable || closedSet.Contains(n)) continue;
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, n);
                if(newMovementCostToNeighbour < n.gCost || !openSet.Contains(n)) {
                    n.gCost = newMovementCostToNeighbour;
                    n.hCost = GetDistance(n, targetNode);
                    n.parent = currentNode;

                    if (!openSet.Contains(n)) openSet.Add(n); // add our neighbour into open set
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

    int GetDistance(Node a, Node b) {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);  // pythagorean approx
        return 14 * dstX + 10 * (dstY - dstX);
        
    }
}
