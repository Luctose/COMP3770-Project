using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridMaster;

namespace UnitControl{
	public class UnitController : MonoBehaviour{
		UnitStates states;	// This must be attached to same object as this class
		GridBase grid;
		// Vars for pathfinding
		public Vector3 startingPosition;
		public Node currentNode;
		public bool movePath;
		public bool unitHasMoved;
		
		int indexPath = 0;
		// 
		public List<Node> currentPath = new List<Node>();
		public List<Node> shortPath = new List<Node>();
		
		public float speed = 5;
		
		float startTime, fractLength;
		Vector3 startPos, targetPos;
		bool updatedPos;
		
		public GameObject unitHighlight;	// This is for highlighting movable units
		public bool highlightFlag;
		
		// Start is called before the first frame update
		void Start(){
			grid = GridBase.GetInstance();
			states = GetComponent<UnitStates>();
			PlaceOnNodeImmediate(startingPosition);
			
			currentNode = grid.GetNodeFromVector3(startingPosition);
			unitHighlight = Instantiate(unitHighlight, transform.position, Quaternion.identity);
		}

		// Update is called once per frame
		void Update(){
			// Open Inventory on E press
			// if(selected && Input.GetKeyUp(KeyCode.E)
				// openInv = true;
				// OpenInventory(this);	// Return openInv to false once finished
				
			// Store if unit is moving
			states.move = movePath;
			
			if(movePath){
				if(!updatedPos){
					// Index is the current node of the unit
					if(indexPath < shortPath.Count - 1){
						indexPath++;
					}
					else {
						// This is the end of the path
						movePath = false;
						// this.states.hasMoved = true;
						// unitHasMoved = true;
					}
					// Update nodes for further movement along shortPath
					currentNode = grid.NodeFromWorldPosition(transform.position);
					startPos = currentNode.worldObject.transform.position;
					targetPos = shortPath[indexPath].worldObject.transform.position;
					// targetPos.y = 1;
					// Now update anmimation variables
					fractLength = Vector3.Distance(startPos, targetPos);
					startTime = Time.time;
					updatedPos = true;	// Flag
				}
				// Adjust unit animations
				// Speed based on distance and time
				float distCover = (Time.time - startTime) * states.movingSpeed;
				if(fractLength == 0)
					fractLength = 0.1f;
				
				float fracJourney = distCover / fractLength;
				if(fracJourney > 1)
					updatedPos = false;
				
				// Set Vector unit is moving to
				Vector3 targetPosition = Vector3.Lerp(startPos, targetPos, fracJourney);
				Vector3 dir = targetPos - startPos;
				dir.y = 0;
				// If there is vertical movement (Y-axis)
				if(!Vector3.Equals(dir, Vector3.zero)){
					Quaternion targetRotation = Quaternion.LookRotation(dir);
					transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
						Time.deltaTime * states.maxSpeed);
				}
				targetPosition.y = 1;
				// Move unit 
				transform.position = targetPosition;
			}
			
			if(highlightFlag){
				unitHighlight.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
			}
		}
		
		// Determine if the currentPath is the shortest path 
		public void EvaluatePath(){
			Vector3 curDirection = Vector3.zero;
			Vector3 nextDirection;
			
			// Start at 1 for indexing previous node
			for(int i = 1; i < currentPath.Count; i++){
				nextDirection = new Vector3(currentPath[i-1].x - currentPath[i].x,
					currentPath[i-1].y - currentPath[i].y,
					currentPath[i-1].z - currentPath[i].z);
				// If the path leads where unit is going, save prev and cur node
				if(!Vector3.Equals(nextDirection, curDirection)){
					shortPath.Add(currentPath[i-1]);
					shortPath.Add(currentPath[i]);
				}
				// Set next node to check 
				curDirection = nextDirection;
			}
			// Add this path to the List
			shortPath.Add(currentPath[currentPath.Count - 1]);
		}
		
		// Default the movement variables
		public void ResetMovingVariables(){
			updatedPos = false;
			indexPath = 0;
			fractLength = 0;
		}
		
		// This is for skipping movement animations
		// Also initialize unit at start
		public void PlaceOnNodeImmediate(Vector3 nodePos){
			int x = Mathf.CeilToInt(nodePos.x);	// Ceiling to Integer
			int y = Mathf.CeilToInt(nodePos.y);
			int z = Mathf.CeilToInt(nodePos.z);
			// Get the node from the given position
			Node node = grid.GetNode(x,y,z);
			
			if(node != null)
				transform.position = node.worldObject.transform.position;
		}
		
		// This is called from GameSystem
		public void EnableHighlight(bool flag){
			unitHighlight.SetActive(flag);
			highlightFlag = flag;
		}
		
		/*
		public bool HasUnitMoved(){
			// return this.states.hasMoved;
			return unitHasMoved;
		}
		*/
		
		public bool IsMoving(){
			// return this.states.move;
			return states.move;
		}
		
		// Return if unit can be selected
		public bool Select(){
			// if(!this.states.hasMoved){
			if(!unitHasMoved){
				states.selected = true;
				// Selection highlight circle thing
				return true;
			}
			else
				return false;
		}
		
		public void Deselect(){
			states.selected = false;
			shortPath.Clear();
			ResetMovingVariables();
			// Remove Selection highlight circle thing
		}
	}
}