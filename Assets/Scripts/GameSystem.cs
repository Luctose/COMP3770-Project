using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitControl;
using LevelControl;
using PI;

// This class controls the turn-based aspect
public class GameSystem : MonoBehaviour{
	// Get Other scripts attached to the game object
	// LevelInitializer lvlInit;		// Might be able to get character prefabs
	public Camera camera;
	PlayerInteractions pInteractions;
	
	// This decides if it's the player's turn or not
	public bool playerTurn;
	public UnitController activeCharacter;	// This is the selected unit
	int activeIndex;
	
	// I want to control moved units here too
	public GameObject[] playerTeamManager;
	public bool[] playerHasMoved;		// Keep track of units moved this turn
	public int playerTeamSize;
	bool flag = false;	// For init playerTeam
	// Have the same for the Enemy team
	public GameObject[] enemyTeamManager;
	public bool[] enemyHasMoved;
	public int enemyTeamSize;
	
	
	void Awake(){
		// This doesn't work on Start, so maybe here
		// playerTeamManager = GameObject.FindGameObjectsWithTag("Player");
		// enemyTeamManager = GameObject.FindGameObjectsWithTag("Enemy");
	}
    // Start is called before the first frame update
    void Start(){
		// lvlInit = GetComponent<LevelInitializer>();
		pInteractions = GetComponent<PlayerInteractions>();
		
		playerTeamSize = playerTeamManager.Length;
		// enemyTeamSize = enemyTeamManager.Length;
		
        // playerHasMoved = new bool[playerTeamSize];
        // enemyHasMoved = new bool[enemyTeamSize];
    }

    // Update is called once per frame
    void Update(){
		if(playerTurn){
			// Left Mouse-click selects a unit for movement
			if(Input.GetMouseButtonUp(0)){	
				if(flag == false){
					InitalizeEnemyTeam();
					InitalizePlayerTeam();
				}
				SelectUnit();
			}
			
			// Space bar confirms unit movement
			if(Input.GetKeyUp(KeyCode.Space)){
				MoveUnit();
			}
		}
		else {	// PlayerTurn = false
			// Make it player's turn for now
			playerTurn = true;
			PlayerTurnStart();
			Debug.Log("Player team can move again!");
		}
    }
	
	// Left-click operation
	void SelectUnit(){
		GameObject selectedUnit = GetPlayerUnit();
		// Check that mouse has hit a player unit
		if(selectedUnit != null){
			activeCharacter = selectedUnit.GetComponent<UnitController>();
			// Debug.Log("activeCharacter = " + activeCharacter.ToString());
			pInteractions.activeUnit = activeCharacter;
		}
		else if(!activeCharacter.GetComponent<UnitController>().IsMoving()){
			// Left-click once character is moved / not selected 
			// Remove unit selection and clear line
			Destroy(GameObject.FindWithTag("Line"));
			activeCharacter.Deselect();
			activeCharacter = null;
			activeIndex = -5;
			pInteractions.activeUnit = activeCharacter;
		}
	}
	
	// Space bar operation
	void MoveUnit(){
		// Debug.Log("Space: Index = " + activeIndex);
		if(activeIndex > -1 && playerHasMoved[activeIndex] == false){		// Unit is selected currently
			// Move the unit
			activeCharacter.GetComponent<UnitController>().movePath = true;
			activeCharacter.GetComponent<UnitController>().EnableHighlight(false);
			playerHasMoved[activeIndex] = true;
		}
		else {
			// Check if all player units have moved
			for(int i = 0; i < playerTeamSize; i++){
				if(playerHasMoved[i] == false)
					break;
				if(i == playerTeamSize - 1)
					playerTurn = false;	// If we get here, all player units have moved
			}
		}
	}
	
	// Get player objects and initialize variables
	void InitalizePlayerTeam(){
		playerTeamManager = GameObject.FindGameObjectsWithTag("Player");
		// playerTeamManager = lvlInit.playerTeamPrefabs;
		playerTeamSize = playerTeamManager.Length;
		// Debug.Log("Team size: " + playerTeamSize);
		playerHasMoved = new bool[playerTeamSize];
		// Render the unit highlight
		for(int i = 0; i < playerTeamSize; i++){
			playerTeamManager[i].GetComponent<UnitController>().EnableHighlight(true);
		}
		PlayerTurnStart();
		flag = true;	// So we don't run this again
	}
	
	// Set player team for movement
	public void PlayerTurnStart(){
		// Should tell player it is their turn
		for(int i = 0; i < playerTeamSize; i++){
			playerHasMoved[i] = false;
			playerTeamManager[i].GetComponent<UnitController>().EnableHighlight(true);
		}
		
	}
	
	public void EnemyTurnStart(){
		for(int i = 0; i < enemyTeamSize; i++)
			enemyHasMoved[i] = false;
		// Begin EnemyAI
		
	}
	
	// Get player objects and initialize variables
	void InitalizeEnemyTeam(){
		enemyTeamManager = GameObject.FindGameObjectsWithTag("Enemy");
		enemyTeamSize = enemyTeamManager.Length;
		// Debug.Log("Team size: " + enemyTeamSize);
		enemyHasMoved = new bool[enemyTeamSize];
		/*
		// Render the unit highlight
		for(int i = 0; i < enemyTeamSize; i++){
			enemyTeamManager[i].GetComponent<UnitController>().EnableHighlight(true);
		}
		PlayerTurnStart();
		flag = true;
		*/
	}
	
	// Return the unit that the player has selected
	public GameObject GetPlayerUnit(){
		// Based on active Camera's perspective
		// Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		Debug.Log("Mouse click: " + Input.mousePosition.x + " " + Input.mousePosition.y + " " + Input.mousePosition.z + " ");
		
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
			for(int i = 0; i < playerTeamSize; i++){
				Debug.Log("Unit Position: " + playerTeamManager[i].transform.position.ToString());
				if(Vector3.Equals(new Vector3(x,y,z), playerTeamManager[i].transform.position)){
					if(playerHasMoved[i] == false){	// Player has not moved
						// Debug.Log("Index = " + i);
						activeIndex = i;
						return playerTeamManager[i];
					}
					else {
						Debug.Log("Unit " + i + " cannot move again.");
						return null;	// Unit cannot move again
					}
				}
			}
		}
		// If we get here, there is no unit selected
		// Debug.Log("Unit not found");
		return null;
	}
}
