using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridManager : MonoBehaviour
{
    // Square GameObject
    public GameObject gridSquare;
	public Material defaultMat;
	public Material selectedMat;
	
    
    public int gridWidthInSquares = 10;
    public int gridHeightInSquares = 10;
 
    private float squareWidth;
    private float squareHeight;
	// Not sure if these are needed
	public float offsetX;
	public float offsetY;
	public float offsetZ;
	
	private List<GameObject> nodes;
 
    // Initialise Square width and height
    void setSizes(){
        squareWidth = gridSquare.GetComponent<Renderer>().bounds.size.x;
        squareHeight = gridSquare.GetComponent<Renderer>().bounds.size.z;
    }
 
    //Method to calculate the position of the first tile
    //The center of the grid is (0,0,0)
    Vector3 calcInitPos(){
        Vector3 initPos;
        //the initial position will be in the left upper corner
        initPos = new Vector3(-squareWidth * gridWidthInSquares / 2f + squareWidth / 2, 0,
            gridHeightInSquares / 2f * squareHeight - squareHeight / 2);
 
        return initPos;
    }
 
    // Convert grid coordinates to game world coordinates
    public Vector3 calcWorldCoord(Vector2 gridPos){
        // First tile
        Vector3 initPos = calcInitPos();

        float x =  initPos.x  + gridPos.x * squareWidth;
        float z = initPos.z - gridPos.y * squareHeight;
		
        return new Vector3(x, 0, z);
    }
 
    // Initialise and position all the tiles
    void createGrid(){
        // Parent of all the tiles
        GameObject squareGridGO = new GameObject("SquareGrid");
 
        for (float y = 0; y < gridHeightInSquares; y++)
        {
            for (float x = 0; x < gridWidthInSquares; x++)
            {
                // Clone the Square object
                GameObject square = (GameObject)Instantiate(gridSquare);
                // Current position in grid
                Vector2 gridPos = new Vector2(x, y);
                square.transform.position = calcWorldCoord(gridPos);
                square.transform.parent = squareGridGO.transform;
				// Add grid square to list of squares
				nodes.Add(square);
            }
        }
    }
 
    void Start(){
		nodes = new List<GameObject>();
        setSizes();
        createGrid();
    }
	
	public GameObject NodeFromWorldPosition(Vector3 worldPosition){
		float worldX = worldPosition.x;
		float worldY = worldPosition.y;
		float worldZ = worldPosition.z;
		
		worldX /= offsetX;
		worldY /= offsetY;
		worldZ /= offsetZ;
		
		int x = Mathf.RoundToInt(worldX);
		int y = Mathf.RoundToInt(worldY);
		int z = Mathf.RoundToInt(worldZ);
		
		// Keep the values within the full Grid size
		/*
		if(x > gridWidthInSquares - 1)
			x = gridWidthInSquares - 1;
		else if(x < 0)
			x = 0;
		// if(y > maxY - 1)
		if(z > gridHeightInSquares - 1)
			z = gridHeightInSquares - 1;
		else if(z < 0)
			z = 0;
		*/
		
		// Now, Look for the grid/node
		for(int i = 0; i < nodes.Count; i++){
			if(nodes[i].transform.position.x == x){
				if(nodes[i].transform.position.z == z)
					return nodes[i];
			}
		}
		// If we get here, there is no node
		return null;
	}
	
	// Check if mouse has selected a square
	void ClickedNode(Vector3 mousePos){
		// First, get the position of mouse click
		GameObject clickedSquare = NodeFromWorldPosition(mousePos);
		if(clickedSquare != null){
			Debug.Log("Node: x = " + clickedSquare.transform.position.x + 
				" y = " + clickedSquare.transform.position.y);
		}
		/*
		// Index the list for the grid square
		for(int i = 0; i < nodes.Count; i++){
			
		}	*/
	}
	
	void Update(){
		if(Input.GetButtonDown("Fire1")){
			Vector3 mousePos = Input.mousePosition;
			Debug.Log("X = " + mousePos.x);
			Debug.Log("Y = " + mousePos.y);
			Debug.Log("Z = " + mousePos.z);	// For some reason this is always 0
			ClickedNode(mousePos);
		}
	}
}
