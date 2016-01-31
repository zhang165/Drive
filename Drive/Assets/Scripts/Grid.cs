using UnityEngine;
using System.Collections;

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

    void Start(){ // initializes a 2d grid for A* search
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid(); // creates the grid
        renderGrid(); // renders the grid
    }

    void CreateGrid(){
        grid = new Node[gridSizeX, gridSizeY]; // instantiate our 2d matrix
        cubes = new Renderer[gridSizeX, gridSizeY]; // instantiate our renderers

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int i=0; i< gridSizeX; i++){
                for(int j=0; j< gridSizeY; j++){
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
                Node toAdd = new Node(walkable, worldPoint);
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

     void renderGrid() {
        if (grid != null) {
            Node playerNode = GetNodeFromWorldPoint(new Vector3(player.position.x + 8.0f, 0, player.position.z-2.0f));
            Node targetNode = GetNodeFromWorldPoint(new Vector3(target.position.x + 8.0f, 0, target.position.z-2.0f));
            for(int i=0; i<grid.GetLength(0); i++) {
                for(int j=0; j<grid.GetLength(1); j++) {
                    Node n = grid[i, j];
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    GameObject cube = (GameObject)Instantiate(Resources.Load("GridCubes"), n.worldPosition, default(Quaternion));
                    cube.transform.localScale = Vector3.one * (nodeDiameter - .1f);
                    //Destroy(cube.GetComponent<Collider>());
                    Renderer render = cube.GetComponent<Renderer>();
                    cubes[i, j] = render;
                    render.material.color = n.walkable ? Color.white : Color.red;
                    //if (playerNode == n) render.material.color = Color.magenta;                // TODO: fix color generation
                    //else if (targetNode == n) render.material.color = Color.green;
                    render.enabled = false; // all cubes turned off by default
                }
            }
                
        }
    }

    void Update() {
        Node playerNode = GetNodeFromWorldPoint(new Vector3(player.position.x + 8.0f, 0, player.position.z - 2.0f));
        Node targetNode = GetNodeFromWorldPoint(new Vector3(target.position.x + 8.0f, 0, target.position.z - 2.0f));
        if (Input.GetKeyDown(KeyCode.X)) { // toggle the colors
            for (int i = 0; i < grid.GetLength(0); i++) {
                for (int j = 0; j < grid.GetLength(1); j++) { // TODO: update cube colors on move
                    Renderer r = cubes[i, j];
                    r.enabled = !r.enabled;
                }
            }
        }
    }

    void OnDrawGizmos() { 
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(grid != null){
            Node playerNode = GetNodeFromWorldPoint(new Vector3(player.position.x+9.2f,0,player.position.z));
            Node targetNode = GetNodeFromWorldPoint(new Vector3(target.position.x+9.2f,0,target.position.z));
            foreach (Node n in grid){
                Gizmos.color = n.walkable ? Color.white : Color.red;
                if (playerNode == n) Gizmos.color = Color.magenta;
                else if(targetNode == n) Gizmos.color = Color.green;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
