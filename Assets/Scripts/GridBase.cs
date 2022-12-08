using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LevelControl;

namespace GridMaster{
    public class GridBase : MonoBehaviour{
        // Dimensions of the Grid
		public int maxX;
		public int maxY;
		public int maxZ;
		// Location of Initializing point
		// public int InitX;
		// public int InitY;
		// public int InitZ;
		// Offset FloorPrefab scale (For positioning in world)
		public float offsetX;
		public float offsetY;
		public float offsetZ;
		
		public Node[, ,] grid;	// Node positions from the Grid
		
		public GameObject gridFloorPrefab;	// This should be the FloorPrefab (quad/cube with node ref)
		
		public Vector3 startNodePosition;
		public Vector3 endNodePosition;
		
		public int enabledY;	// For verticality
		List<GameObject> YCollisions = new List<GameObject>();	// For multiple floors
		
		LevelManager lvlManager;	// This holds all obstacles on map
		
		// This is called from LevelInitializer
		public void InitGrid(LevelControl.LevelInitializer.GridStats gridStats){
			// Save GridStats in this class
			maxX = gridStats.maxX;
			maxY = gridStats.maxY;
			maxZ = gridStats.maxZ;
			offsetX = gridStats.offsetX;
			offsetY = gridStats.offsetY;
			offsetZ = gridStats.offsetZ;
			
			lvlManager = LevelManager.GetInstance();
			
			// CreateGrid();
			CreateMouseCollision();
			CloseAllMouseCollisions();
			
			YCollisions[enabledY].SetActive(true);
		}
		
		//Singleton
        public static GridBase instance;
        public static GridBase GetInstance(){
            return instance;
        }

        void Awake(){
            instance = this;
        }

        void Start(){
			// CreateGrid() does the same thing but better
            // The typical way to create a grid
            grid = new Node[maxX, maxY, maxZ];

			// Indexing on 3 axis'
            for (int x = 0; x < maxX; x++){
                for (int y = 0; y < maxY; y++){
                    for (int z = 0; z < maxZ; z++){
                        // Apply the offsets and create the world object for each node
                        float posX = x * offsetX;
                        float posY = y * offsetY;
                        float posZ = z * offsetZ;
						/*float posX = (InitX + x) * offsetX;
							float posY = (InitY + y) * offsetY;
							float posZ = (InitZ + z) * offsetZ;
						*/
                        GameObject go = Instantiate(gridFloorPrefab, new Vector3(posX, posY, posZ),
                            Quaternion.identity) as GameObject;
                        // Rename it
                        go.transform.name = x.ToString() + " " + y.ToString() + " " + z.ToString();
                        // This is for hierarchy organization
                        go.transform.parent = transform;

                        //Create a new node and update it's values
                        Node node = new Node();
                        node.x = x;
                        node.y = y;
                        node.z = z;
                        node.worldObject = go;	// Apply 
						
						node.nodeRef = go.GetComponentInChildren<NodeReferences>();
                        
                        // This is a list of all hit boxes for the node
						// BoxCastAll(centerPoint, extention (to fill the node to the edge), direction)
                        // RaycastHit[] hits = Physics.BoxCastAll(new Vector3(posX, posY, posZ), new Vector3(1,0,1), Vector3.forward);
						RaycastHit[] hits = Physics.BoxCastAll(new Vector3(posX,posY,posZ), new Vector3(0.5f,0.5f,0.5f), Vector3.up);
							
						// Make sure node is not walkable
                        for (int i = 0; i < hits.Length; i++){
                            node.isWalkable = false;  
							// node.nodeRef.ChangeTileMaterial(1);	// This is causing grid problems
							// Save the level object
							LevelObject lvlObj = hits[i].transform.GetComponent<LevelObject>();
							// Debug.Log(lvlObj.ToString());
							hits[i].collider.enabled = false;	// Disable the collider (idk why, resources?)
							
							
							
							
							
							/*
							// If there is an object ABOVE the level object
							if(!lvlManager.lvlObjects[y].objs.Contains(lvlObj.gameObject)){
								// Debug.Log("Test");
								lvlManager.lvlObjects[y].objs.Add(lvlObj.gameObject);	// Save it
							}
							
							// Enable node renderer
							node.nodeRef.tileRenderer.enabled = true;
							// If the level object is an obstacle
							if(lvlObj.objType == LevelObject.LvlObjectType.obstacle){
								node.isWalkable = false;	// Prevent movement
								// node.nodeRef.ChangeTileMaterial(1);	// Display unwalkable material
								
							}
							// If the level object is a wall
							if(lvlObj.objType == LevelObject.LvlObjectType.wall){
								node.isWalkable = false;	// Prevent movement
								// node.nodeRef.ChangeTileMaterial(1);	// Display unwalkable material
								
							}
							// If level object is another floor
							if(lvlObj.objType == LevelObject.LvlObjectType.floor){
								// Make it walkable
								node.isWalkable = true;
								// node.nodeRef.ChangeTileMaterial(0);
							}
							*/
                        }
                        //then place it to the grid
                        grid[x, y, z] = node;
                    }
                }
            }
        }

		// Checks for and returns a node from the given x,y,z coordinates
		// 		Otherwise returns null
        public Node GetNode(int x, int y, int z){
            Node retVal = null;
			// Check if x,y,z are within the grid boundary
            if (x < maxX && x >= 0 && y >= 0 && y < maxY && z >= 0 && z < maxZ){
                retVal = grid[x, y, z];
            }
            return retVal;
        }
		
		// Returns the node based on Vector3 position
		public Node NodeFromWorldPosition(Vector3 worldPosition){
			// Save positions in local context
			float worldX = worldPosition.x;
			float worldY = worldPosition.y;
			float worldZ = worldPosition.z;
			// APply offsets
			worldX /= offsetX;
			worldY /= offsetY;
			worldZ /= offsetZ;
			// Round to int (for easier calculation, finding node)
			int x = Mathf.RoundToInt(worldX);
			int y = Mathf.RoundToInt(worldY);
			int z = Mathf.RoundToInt(worldZ);
			// Adjust coordinates to stay within grid boundary
			if(x > maxX - 1)
				x = maxX - 1;
			if(y > maxY - 1)
				y = maxY - 1;
			if(z > maxZ - 1)
				z = maxZ - 1;
			if(x < 0)
				x = 0;
			if(y < 0)
				y = 0;
			if(z < 0)
				z = 0;
			
			return grid[x,y,z];	// Return the node
		}

		// This is used by UnitController class
        public Node GetNodeFromVector3(Vector3 pos){
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            int z = Mathf.RoundToInt(pos.z);

            Node retVal = GetNode(x, y, z);
            return retVal;
        }
		
		// This is called above, by external classes
		/*
		void CreateGrid(){
			grid = new Node[maxX, maxY, maxZ];
			
			// Save the world obstacles and walls
			for(int i = 0; i < maxY; i++){
				lvlManager.lvlObjects.Add(new LevelManager.ObjsPerFloor());
				lvlManager.lvlObjects[i].floorIndex = i;
			}
			// Indexing a 3D world space
			for(int x = 0; x < maxX; x++){
				for(int y = 0; y < maxY; y++){
					for(int z = 0; z < maxZ; z++){
						// Apply offsets and create world object for each node
						float posX = x * offsetX;
						float posY = y * offsetY;
						float posZ = z * offsetZ;
						GameObject go = Instantiate(gridFloorPrefab, new Vector3(posX, posY, posZ),
							Quaternion.identity) as GameObject;
						// Rename it
						go.transform.name = x.ToString() + " " + y.ToString() + " " + z.ToString();
						// This is for organization within the hierarchy
						go.transform.parent = transform;
						// Create the pathing node
						Node node = new Node();
						node.x = x;
						node.y = y;
						node.z = z;
						node.worldObject = go;
						node.nodeRef = go.GetComponentInChildren<NodeReferences>();
						node.isWalkable = false;
						node.nodeRef.tileRenderer.enabled = false;	// Default Node visuals
						// Store all raycast hit boxes
						RaycastHit[] hits = Physics.BoxCastAll(new Vector3(posX,posY,posZ),
							new Vector3(0.5f,0.5f,0.5f), Vector3.up);
						
						// Index through all hitboxes
						for(int i = 0; i < hits.Length; i++){
							// If there is a level object within raycast bounds
							if(hits[i].transform.GetComponent<LevelObject>()){
								// Save the level object
								LevelObject lvlObj = hits[i].transform.GetComponent<LevelObject>();
								// hits[i].collider.enabled = false;	// Disable the collider (idk why, resources?)
								// If there is an object ABOVE the level object
								if(!lvlManager.lvlObjects[y].objs.Contains(lvlObj.gameObject)){
									lvlManager.lvlObjects[y].objs.Add(lvlObj.gameObject);	// Save it
								}
								// Enable node renderer
								node.nodeRef.tileRenderer.enabled = true;
								// If the level object is an obstacle
								if(lvlObj.objType == LevelObject.LvlObjectType.obstacle){
									node.isWalkable = false;	// Prevent movement
									node.nodeRef.ChangeTileMaterial(1);	// Display unwalkable material
									break;	// Next iteration
								}
								// If level object is another floor
								if(lvlObj.objType == LevelObject.LvlObjectType.floor){
									// Make it walkable
									node.isWalkable = true;
									node.nodeRef.ChangeTileMaterial(0);
								}
							}
						}
						// Then place it on the grid
						grid[x,y,z] = node;
					}
				}
			}
		}
		*/
		
		// Create collisions for multi-floor areas
		void CreateMouseCollision(){
			for(int y = 0; y < maxY; y++){
				GameObject go = new GameObject();
				go.transform.name = "Collision for Y " + y.ToString();
				go.AddComponent<BoxCollider>();
				go.GetComponent<BoxCollider>().size = new Vector3(maxX * offsetX, 0.1f, maxZ * offsetZ);
				
				go.transform.position = new Vector3((maxX * offsetX)/2 - offsetX/2, y * offsetY,
					(maxZ * offsetZ)/2 - offsetZ/2);
				
				YCollisions.Add(go);	// Add to list
			}
		}
		
		// Disable all vertical mouse collisions
		void CloseAllMouseCollisions(){
			for(int i = 0; i < YCollisions.Count; i++){
				YCollisions[i].SetActive(false);
			}
		}
    }
}