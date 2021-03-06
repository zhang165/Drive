﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
    public Transform player;
    public Transform target; 

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    private Node[,] grid;
    private Renderer[,] cubes;

    private float nodeDiameter;
    int gridSizeX, gridSizeY;

    public List<Node> path; // solution path
    public List<Node> prevpath; // previousSolution path


    void Start(){ // initializes a 2d grid for A* search
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid(); // creates the grid
        renderGrid(); // renders the grid
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();
        for (int i = -1; i <= 1; i++) {
            for(int j=-1; j<=1; j++) {
                if (i == 0 && j == 0) continue;

                int checkX = node.gridX + i;
                int checkY = node.gridY + j;
                
                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    void CreateGrid(){
        grid = new Node[gridSizeX, gridSizeY]; // instantiate our 2d matrix
        cubes = new Renderer[gridSizeX, gridSizeY]; // instantiate our renderers
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int i=0; i< gridSizeX; i++){
                for(int j=0; j< gridSizeY; j++){
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
                Node toAdd = new Node(walkable, worldPoint, i, j);
                grid[i,j] = toAdd;
                }
        } 
    }
        
    public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y); // clamps % between 0-1
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

	public int MaxSize{
		get{
			return gridSizeX * gridSizeY;
		}
	}

     void renderGrid() { // renders the grid in real time
        if (grid != null) {
            //Node playerNode = GetNodeFromWorldPoint(new Vector3(player.position.x + 8.0f, 0, player.position.z-2.0f));
            //Node targetNode = GetNodeFromWorldPoint(new Vector3(target.position.x + 8.0f, 0, target.position.z-2.0f));
            for(int i=0; i<grid.GetLength(0); i++) {
                for(int j=0; j<grid.GetLength(1); j++) {
                    Node n = grid[i, j];
                    GameObject cube = (GameObject)Instantiate(Resources.Load("GridCubes"), new Vector3(n.worldPosition.x, n.worldPosition.y-0.93f, n.worldPosition.z), default(Quaternion));
                    //GameObject cube = (GameObject)Instantiate(Resources.Load("Plane"), new Vector3(n.worldPosition.x, n.worldPosition.y+0.93f, n.worldPosition.z), default(Quaternion));
                    cube.transform.localScale = Vector3.one * (nodeDiameter - .1f);
                    Renderer render = cube.GetComponent<Renderer>();
                    cubes[i, j] = render;
                    render.material.color = n.walkable ? new Color(0.5f,0.5f,0.5f,0.3f) : Color.red;
                    render.enabled = false; // all cubes turned off by default
                }
            }
                
        }
    }

    void Update() {
        //Node playerNode = GetNodeFromWorldPoint(new Vector3(player.position.x + 8.0f, 0, player.position.z - 2.0f));
        //Node targetNode = GetNodeFromWorldPoint(new Vector3(target.position.x + 8.0f, 0, target.position.z - 2.0f));
        if (Input.GetKeyDown(KeyCode.X)) { // toggle the colors
            for (int i = 0; i < grid.GetLength(0); i++) {
                for (int j = 0; j < grid.GetLength(1); j++) { 
                    Renderer r = cubes[i, j];
                    r.enabled = !r.enabled;
                }
            }
        }
        if(prevpath != null) {
            foreach (Node n in prevpath) {
                cubes[n.gridX, n.gridY].material.color = new Color(0.5f, 0.5f, 0.5f, 0.3f); // set colors of each previous path to white
            }
        }
        foreach(Node n in path) {
            cubes[n.gridX, n.gridY].material.color = Color.green; // set colors of each cube to green
        }
        prevpath = path;
    }

    //void OnDrawGizmos() { 
    //    Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
    //    if(grid != null){
    //        Node playerNode = GetNodeFromWorldPoint(new Vector3(player.position.x+9.2f,0,player.position.z));
    //        Node targetNode = GetNodeFromWorldPoint(new Vector3(target.position.x+9.2f,0,target.position.z));
    //        foreach (Node n in grid){
    //            Gizmos.color = n.walkable ? Color.white : Color.red;
    //            if(path != null) if (path.Contains(n)) Gizmos.color = Color.green; // calculates the path
    //            if (playerNode == n) Gizmos.color = Color.magenta;
    //            else if(targetNode == n) Gizmos.color = Color.black;
    //            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
    //        }
    //    }
    //}
}
