using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GridMaster;

namespace PathFinding{
    public class Pathfinder{
        GridBase gridBase;
        public Node startPosition;
        public Node endPosition;
		// Volatile to close the thread
        public volatile bool jobDone = false;
        PathfindMaster.PathfindingJobComplete completeCallback;
        List<Node> foundPath;
		
		public int pathLimit = 5;		// Limit the nodes travelled

        //Constructor
        public Pathfinder(Node start, Node target, PathfindMaster.PathfindingJobComplete callback){
            startPosition = start;
            endPosition = target;
            completeCallback = callback;
            gridBase = GridBase.GetInstance();
        }

        public void FindPath(){	// Now call the algorithm
            foundPath = FindPathActual(startPosition, endPosition);
			// Tell the thread to close
            jobDone = true;
        }

		// Send path to Master class
        public void NotifyComplete(){
            if(completeCallback != null){
                completeCallback(foundPath);
            }
        }
		
		// This it the A* Algorithm 
        private List<Node> FindPathActual(Node start, Node target){
            List<Node> foundPath = new List<Node>();		// Final path
            List<Node> openSet = new List<Node>();			// Need to check
            HashSet<Node> closedSet = new HashSet<Node>();	// Already Checked

            openSet.Add(start);		// Add first node

            // while (openSet.Count > 0){
            while (openSet.Count > 0 && closedSet.Count < pathLimit){	// Limit path distance
				// Save first node
                Node currentNode = openSet[0];

                for (int i = 0; i < openSet.Count; i++){
                    // Check the costs of the current node with the openSet
                    if (openSet[i].fCost < currentNode.fCost || 
						(openSet[i].fCost == currentNode.fCost &&
							openSet[i].hCost < currentNode.hCost))
                    {
                        // Assign a new current node
                        if (!currentNode.Equals(openSet[i])){
                            currentNode = openSet[i];
                        }
                    }
                }

                // Move node to the checked set
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // Current node is the target node
                if (currentNode.Equals(target)){
                    // Retrace path back to the start
                    foundPath = RetracePath(start, currentNode);
                    break;
                }

				
                // If we get here, check the neighbours of the node
                foreach (Node neighbour in GetNeighbours(currentNode,true)){
                    if (!closedSet.Contains(neighbour)){
                        // Neighbour movement cost
                        float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        // If cost is lower than neighbour
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, target);
                            // Assign the parent node
                            neighbour.parentNode = currentNode;
                            // Add neighbour to open set
                            if (!openSet.Contains(neighbour)){
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }
            
            // Return the Path
            return foundPath;
        }
		
		// Retrace from endNode to startNode
        private List<Node> RetracePath(Node startNode, Node endNode){
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode){
                path.Add(currentNode);
                currentNode = currentNode.parentNode;	// Using Parent nodes
            }

            path.Reverse();

            return path;
        }
		
		// Find the neighbours of the given node
        private List<Node> GetNeighbours(Node node, bool getVerticalneighbours = false){
            List<Node> retList = new List<Node>();	// This is returned
			// Start with negative value
            for (int x = -1; x <= 1; x++){
                for (int yIndex = -1; yIndex <= 1; yIndex++){
                    for (int z = -1; z <= 1; z++){
                        int y = yIndex;

                        // If there is no verticality, don't check Y
                        if (!getVerticalneighbours){
                            y = 0;
                        }
						// 
						if (x != 0 && y != 0 && z != 0){
							Node searchPos = new Node();

                            searchPos.x = node.x + x;
                            searchPos.y = node.y + y;
                            searchPos.z = node.z + z;

                            Node newNode = GetNeighbourNode(searchPos, true, node);
							// Add Node to the retList
                            if (newNode != null){
                                retList.Add(newNode);
                            }
						}
						
                        if (x == 0 && y == 0 && z == 0){
                            // 000 is the current node (Ignore)
                        }
                        else {
                            Node searchPos = new Node();
                            // We want what's forward/backwars,left/righ,up/down from unit
                            searchPos.x = node.x + x;
                            searchPos.y = node.y + y;
                            searchPos.z = node.z + z;

                            Node newNode = GetNeighbourNode(searchPos, true, node);
                            if (newNode != null){
                                retList.Add(newNode);
                            }
                        }
						
                    }
                }
            }
            return retList;
        }

        private Node GetNeighbourNode(Node adjPos, bool searchTopDown, Node currentNodePos){
            // this is where the meat of it is
            Node retVal = null;
            // let's take the node from the adjacent positions
            Node node = GetNode(adjPos.x, adjPos.y, adjPos.z);
            // Check if node is walkable
            if (node != null && node.isWalkable){
                retVal = node;
            }
			// If there is more than 1 floor
            else if (searchTopDown){
                // Look at node beneath unit
                adjPos.y -= 1;
                Node bottomBlock = GetNode(adjPos.x, adjPos.y, adjPos.z);
                
                // Node beneath is walkable
                if (bottomBlock != null && bottomBlock.isWalkable){
                    retVal = bottomBlock;
                }
                else{
                    // otherwise, look above
                    adjPos.y += 2;
                    Node topBlock = GetNode(adjPos.x, adjPos.y, adjPos.z);
                    if (topBlock != null && topBlock.isWalkable){
                        retVal = topBlock;
                    }
                }
            }

            // if the node is diagonal to the current node then check the neighbouring nodes
            // Need to have 4 nodes walkable to move diagonally
            int originalX = adjPos.x - currentNodePos.x;
            int originalZ = adjPos.z - currentNodePos.z;

            if (Mathf.Abs(originalX) == 1 && Mathf.Abs(originalZ) == 1){
                // First block is originalX, 0 and the second to check is 0, originalZ
                // They need to be walkable
                Node neighbour1 = GetNode(currentNodePos.x + originalX, currentNodePos.y, currentNodePos.z);
                if (neighbour1 == null || !neighbour1.isWalkable){
                    retVal = null;
                }

                Node neighbour2 = GetNode(currentNodePos.x, currentNodePos.y, currentNodePos.z + originalZ);
                if (neighbour2 == null || !neighbour2.isWalkable){
                    retVal = null;
                }
            }
			
			/*
            // Test conditions
            if (retVal != null){
                //Do not approach a node from the left
                if(node.x > currentNodePos.x) {
                    node = null;
                }
            }
			*/
            return retVal;
        }

		// Get Node from GridBase
        private Node GetNode(int x, int y, int z){
            Node n = null;

            lock(gridBase){
                n = gridBase.GetNode(x, y, z);
            }
            return n;
        }

		// Find the distance between each node
        private int GetDistance(Node posA, Node posB){
            int distX = Mathf.Abs(posA.x - posB.x);
            int distZ = Mathf.Abs(posA.z - posB.z);
            int distY = Mathf.Abs(posA.y - posB.y);

            if (distX > distZ){
                return 14 * distZ + 10 * (distX - distZ) + 10 * distY;
            }	// Else
            return 14 * distX + 10 * (distZ - distX) + 10 * distY;
        }
    }
}