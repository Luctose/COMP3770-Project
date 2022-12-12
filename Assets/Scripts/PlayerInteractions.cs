using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// NameSpace
using GridMaster;
using PathFinding;
using UnitControl;

namespace PI{
	public class PlayerInteractions : MonoBehaviour{
		// The Unit that the player is currently controlling
		public UnitController activeUnit;		// Set in Inspector (for now)
		public GameObject[] playerUnits = null;	// Array of player units
		public Camera camera;
		
		public bool hasPath;
		public bool holdPath;
		
		PathfindMaster pathFinder;
		GridBase grid;
		Node prevNode;
		// This is for displaying the path using a Line
		public bool visualizePath;
		public GameObject lineGO;
		public LineRenderer line;	// Attached to this same object
		
		// Start is called before the first frame update
		void Start(){
			grid = GridBase.GetInstance();
			pathFinder = PathfindMaster.GetInstance();
			// playerUnits = GameObject.FindGameObjectsWithTag("Player");
		}

		// Update is called once per frame
		void Update(){
			// Check if a unit is selected and does not have a path already
			// Active Unit is updated by GameSystem*
			if(activeUnit){
				if(!hasPath){
					// Store node the unit is currently on
					Node startNode = grid.NodeFromWorldPosition(activeUnit.transform.position);
					// Get the node that the mouse is currently hovering over
					Node targetNode = FindNodeFromMousePosition();
					// If target node has changed and it's not a previous node
					if(targetNode != null && startNode != null){
						if(prevNode != targetNode && !holdPath){
							// Update the path
							pathFinder.RequestPathfind(startNode, targetNode, PopulatePathOfActiveUnit);
							prevNode = targetNode;
							hasPath = true;	// Update flag
						}
					}
				}
				// Right Mouse-click selects the node for movement
				// This will stop tracing mouse movements above
				if(Input.GetMouseButtonUp(1) && !activeUnit.GetComponent<UnitController>().IsMoving())
					holdPath = !holdPath;
				// If the node selected is the unit's node
				if(activeUnit.shortPath.Count < 1)
					holdPath = false;
				// Toggle path line in Inspector
				if(visualizePath){
					if(line == null){
						// Instantiate the line object
						GameObject go = Instantiate(lineGO, transform.position, Quaternion.identity) as GameObject;
						line = go.GetComponent<LineRenderer>();
					}
					else {	// Render the line
						// line.SetVertexCount(activeUnit.shortPath.Count);
						line.positionCount = activeUnit.shortPath.Count;
						for(int i = 0; i < activeUnit.shortPath.Count; i++){
							line.SetPosition(i, activeUnit.shortPath[i].worldObject.transform.position);
						}
					}
				}
			}
		}
		
		// Use a Raycast to get the mouse-hovered node
		Node FindNodeFromMousePosition(){
			Node retVal = null;
			// Based on active Camera's perspective
			// Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// If the cast collides with something
			if(Physics.Raycast(ray, out hit, 200)){
				// Check if that is a node position
				retVal = grid.NodeFromWorldPosition(hit.point);
			}
			// Return that node, or nothing
			return retVal;
		}
		
		// Add the nodes between 
		void PopulatePathOfActiveUnit(List<Node> nodes){
			// First, clear both lists
			activeUnit.currentPath.Clear();
			activeUnit.shortPath.Clear();
			// Add the unit's current node to the path
			activeUnit.currentPath.Add(grid.NodeFromWorldPosition(activeUnit.transform.position));
			// Add the list of nodes to the path
			for(int i = 0; i < nodes.Count; i++){
				activeUnit.currentPath.Add(nodes[i]);
			}
			// Let UnitController decide if this path is best
			activeUnit.EvaluatePath();
			activeUnit.ResetMovingVariables();
			hasPath = false;	// Reset flag
		}
	
		// Return the unit that the player has selected
		public GameObject GetPlayerUnit(){
			// Based on active Camera's perspective
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, 200)){
				Debug.Log("Ray hit: " + hit.point.ToString());
				// Save positions in local context
				float worldX = hit.point.x;
				float worldY = hit.point.y;
				float worldZ = hit.point.z;
				// Round to int (for easier calculation, finding node)
				int x = Mathf.RoundToInt(worldX);
				int y = Mathf.RoundToInt(worldY) + 1;
				int z = Mathf.RoundToInt(worldZ);
				
				Debug.Log("Mouse: " + x + " " + y + " " + z);
				
				for(int i = 0; i < playerUnits.Length; i++){
					Debug.Log("Unit Position: " + playerUnits[i].transform.position.ToString());
					if(Vector3.Equals(new Vector3(x,y,z), playerUnits[i].transform.position)){
						// Debug.Log("Unit Found");
						return playerUnits[i];
					}
				}
			}
			// If we get here, there is no unit selected
			// Debug.Log("Unit not found");
			return null;
		}
		
		/*
		public void DestroyLineObject(){
			Destroy(line);
		}
		*/
	}
}