using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelControl{
	public class LevelInitializer : MonoBehaviour{
		public GameObject gridBasePrefab;	// Level Model prefab (?) Should be main floor and boundary walls
		public GameObject unitPrefab;		// Player controlled unit to render
		
		public int unitCount;	// Number of units for player to control
		
		private WaitForEndOfFrame waitEF;	// Allows sync of threads
		
		public GridStats gridStats;
		
		[System.Serializable]
		public class GridStats{			// These can be changed in Inspector
			public int maxX = 10;
			public int maxY = 3;
			public int maxZ = 10;
			
			public float offsetX = 1;	// These need testing to confirm use
			public float offsetY = 1;
			public float offsetZ = 1;
		}
		
		// Start is called before the first frame update
		void Start(){
			waitEF = new WaitForEndOfFrame();
			StartCoroutine("InitLevel");
		}
		
		// Create threads for rendering grid and units
		IEnumerator InitLevel(){
			// yield return StartCoroutine(CreateGrid());
			yield return StartCoroutine(CreateUnits());
			yield return StartCoroutine(EnablePlayerInteractions());
		}
		
		// Uses Grid stats to place pathing nodes along level floor 
		IEnumerator CreateGrid(){
			GameObject gameObj = Instantiate(gridBasePrefab, Vector3.zero, Quaternion.identity) as GameObject;
			gameObj.GetComponent<GridMaster.GridBase>().InitGrid(gridStats);	// Invoke grid base
			yield return waitEF;
		}
		
		// Clones of unit prefab are added to 3D world
		IEnumerator CreateUnits(){
			for(int i = 0; i < unitCount; i++){
				Instantiate(unitPrefab, new Vector3(0,1,0), Quaternion.identity);
			}
			yield return waitEF;
		}
		
		// Get the PlayerInteractions component
		IEnumerator EnablePlayerInteractions(){
			GetComponent<PI.PlayerInteractions>().enabled = true;
			yield return waitEF;
		}
	}
}