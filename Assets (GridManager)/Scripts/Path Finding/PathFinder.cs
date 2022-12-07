using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GridMaster;

namespace PathFinding{
    public class Pathfinder{
        GridBase gridBase;
        public Node startPosition;
        public Node endPosition;

        public volatile bool jobDone = false;
        PathfindMaster.PathfindingJobComplete completeCallback;
        List<Node> foundPath;

        //Constructor
        public Pathfinder(Node start, Node target, PathfindMaster.PathfindingJobComplete callback){
            startPosition = start;
            endPosition = target;
            completeCallback = callback;
            gridBase = GridBase.GetInstance();
        }

        public void FindPath(){         
            foundPath = FindPathActual(startPosition, endPosition);

            jobDone = true;
        }

        public void NotifyComplete(){
            if(completeCallback != null)
            {
                completeCallback(foundPath);
            }
        }

        private List<Node> FindPathActual(Node start, Node target){
            // A* Algorithm
            List<Node> foundPath = new List<Node>();	// Final path

            List<Node> openSet = new List<Node>();			// Need to check
            HashSet<Node> closedSet = new HashSet<Node>();	// Already Checked

            openSet.Add(start);		// Add first node

            while (openSet.Count > 0){
                Node currentNode = openSet[0];

                for (int i = 0; i < openSet.Count; i++){
                    // Check the cost of the current node
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

        private List<Node> GetNeighbours(Node node, bool getVerticalneighbours = false){
            List<Node> retList = new List<Node>();	// This is returned

            for (int x = -1; x <= 1; x++){
                for (int yIndex = -1; yIndex <= 1; yIndex++){
                    for (int z = -1; z <= 1; z++){
                        int y = yIndex;

                        // If there is no verticality, don't check Y
                        if (!getVerticalneighbours){
                            y = 0;
                        }
						
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
						/*
                        if (x == 0 && y == 0 && z == 0){
                            //000 is the current node
                        }
                        else
                        {
                            Node searchPos = new Node();

                            //the nodes we want are what's forward/backwars,left/righ,up/down from us
                            searchPos.x = node.x + x;
                            searchPos.y = node.y + y;
                            searchPos.z = node.z + z;

                            Node newNode = GetNeighbourNode(searchPos, true, node);

                            if (newNode != null)
                            {
                                retList.Add(newNode);
                            }
                        }
						*/
                    }
                }
            }

            return retList;
        }

        private Node GetNeighbourNode(Node adjPos, bool searchTopDown, Node currentNodePos){
            //this is where the meat of it is
            //We can add all the checks we need here to tweak the algorythm to our heart's content
            //but first let's start from the the usual stuff you'll see in A*

            Node retVal = null;
            
            //let's take the node from the adjacent positions we passed
            Node node = GetNode(adjPos.x, adjPos.y, adjPos.z);

            // Check if node is walkable
            if (node != null && node.isWalkable){
                retVal = node;
            }
			// If there is more than 1 floor
            else if (searchTopDown){
                // Look what the adjacent node have under him
                adjPos.y -= 1;
                Node bottomBlock = GetNode(adjPos.x, adjPos.y, adjPos.z);
                
                // if there is a bottom block and we can walk on it
                if (bottomBlock != null && bottomBlock.isWalkable){
                    retVal = bottomBlock;
                }
                else{
                    // otherwise, we look what it has on top of it
                    adjPos.y += 2;
                    Node topBlock = GetNode(adjPos.x, adjPos.y, adjPos.z);
                    if (topBlock != null && topBlock.isWalkable){
                        retVal = topBlock;
                    }
                }
            }

            //if the node is diagonal to the current node then check the neighbouring nodes
            //so to move diagonally, we need to have 4 nodes walkable
            int originalX = adjPos.x - currentNodePos.x;
            int originalZ = adjPos.z - currentNodePos.z;

            if (Mathf.Abs(originalX) == 1 && Mathf.Abs(originalZ) == 1)
            {
                // the first block is originalX, 0 and the second to check is 0, originalZ
                //They need to be pathfinding walkable
                Node neighbour1 = GetNode(currentNodePos.x + originalX, currentNodePos.y, currentNodePos.z);
                if (neighbour1 == null || !neighbour1.isWalkable)
                {
                    retVal = null;
                }

                Node neighbour2 = GetNode(currentNodePos.x, currentNodePos.y, currentNodePos.z + originalZ);
                if (neighbour2 == null || !neighbour2.isWalkable)
                {
                    retVal = null;
                }
            }

            //and here's where we can add even more additional checks
            if (retVal != null)
            {
                //Example, do not approach a node from the left
                /*if(node.x > currentNodePos.x) {
                    node = null;
                }*/
            }

            return retVal;
        }

        private Node GetNode(int x, int y, int z)
        {
            Node n = null;

            lock(gridBase)
            {
                n = gridBase.GetNode(x, y, z);
            }
            return n;
        }

        private int GetDistance(Node posA, Node posB)
        {
            //We find the distance between each node
            //not much to explain here

            int distX = Mathf.Abs(posA.x - posB.x);
            int distZ = Mathf.Abs(posA.z - posB.z);
            int distY = Mathf.Abs(posA.y - posB.y);

            if (distX > distZ)
            {
                return 14 * distZ + 10 * (distX - distZ) + 10 * distY;
            }

            return 14 * distX + 10 * (distZ - distX) + 10 * distY;
        }

    }
}