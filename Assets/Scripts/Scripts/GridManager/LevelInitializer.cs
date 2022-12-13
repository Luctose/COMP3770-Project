using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelControl{
	public class LevelInitializer : MonoBehaviour{
		public GameObject gridBasePrefab;	// Level Model prefab (?) Should be main floor and boundary walls
		
		public GameObject mainCharacter;		// Player controlled unit to render
		public GameObject secondCharacter;		// Player controlled unit to render
		public GameObject thirdCharacter;		// Player controlled unit to render
		public GameObject enemy1;			// Enemy unit to render
		public GameObject enemy2;			// Enemy unit to render
		public GameObject enemyBoss;		// Enemy unit to render
		
		public GameObject[] playerTeamPrefabs;
		public GameObject[] enemyTeamPrefabs;
		int playerCount = 3;		// Number of units for player to control
		int enemyCount = 3;		// Number of units for player to control
		
		/*
		public int playerOffsetX;
		public int playerOffsetZ;
		public int enemyStartX;
		public int enemyStartZ;
		public int enemyOffsetX;
		public int enemyOffsetZ;
		*/
		
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
			playerTeamPrefabs = new GameObject[playerCount];
			enemyTeamPrefabs = new GameObject[enemyCount];
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
			// Hard-set the Player and Enemy units
			// Instantiate(playerPrefab, new Vector3(unitOffsetX * i, 1, unitOffsetZ * i), Quaternion.identity);
			playerTeamPrefabs[0] = Instantiate(mainCharacter, new Vector3(0, 1, 0), Quaternion.identity);
			playerTeamPrefabs[1] = Instantiate(secondCharacter, new Vector3(6, 1, 6), Quaternion.identity);
			playerTeamPrefabs[2] = Instantiate(thirdCharacter, new Vector3(10, 1, 6), Quaternion.identity);
			
			enemyTeamPrefabs[0] = Instantiate(enemy1, new Vector3(12, 1, 12), Quaternion.identity);
			// enemyTeamPrefabs[1] = Instantiate(enemy2, new Vector3(2, 1, 30), Quaternion.identity);
			// enemyTeamPrefabs[2] = Instantiate(enemyBoss, new Vector3(10, 1, 44), Quaternion.identity);
			enemyTeamPrefabs[1] = Instantiate(enemyBoss, new Vector3(10, 1, 44), Quaternion.identity);
			enemyTeamPrefabs[2] = Instantiate(enemy2, new Vector3(2, 1, 30), Quaternion.identity);
			// Swap indexes bcuz of how enemy units are destoryed upon death
			
			/*
			// int x = unitOffsetX;
			// int z = unitOffsetZ;
			// int i;
			for(int i = 0; i < playerCount; i++){
				// Instantiate(playerPrefab, new Vector3(unitOffsetX * i, 1, unitOffsetZ * i), Quaternion.identity);
				// Add it to a list somewhere for accessing in PlayerInteractions
				// playerTeamPrefabs[i] = playerPrefab;
				playerTeamPrefabs[i] = Instantiate(playerPrefab, new Vector3(playerOffsetX * i, 1, playerOffsetZ * i), Quaternion.identity);
				// playerTeamPrefabs[i] = Instantiate(playerPrefab, new Vector3(6, 1, 2), Quaternion.identity);
			}
			for(int i = 0; i < enemyCount; i++){
				enemyTeamPrefabs[i] = Instantiate(enemyPrefab, new Vector3(enemyStartX + (enemyOffsetX * i), 
					1, enemyStartX + (enemyOffsetZ * i)), Quaternion.identity);
			}
			*/
			yield return waitEF;
		}
		
		// Get the PlayerInteractions component
		IEnumerator EnablePlayerInteractions(){
			GetComponent<PI.PlayerInteractions>().enabled = true;
			yield return waitEF;
		}
	}
}