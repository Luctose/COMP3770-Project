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
	public Data data;	// Drop into Inspector
	PlayerInteractions pInteractions;
	CombatSystem combat;
	
	// This decides if it's the player's turn or not
	public bool playerTurn;
	public UnitController activeCharacter;	// This is the selected unit
	int activeIndex;
	
	// I want to control moved units here too
	public GameObject[] playerTeamManager;
	public bool[] playerHasMoved;		// Keep track of units moved this turn
	public bool[] playerHasAttacked;		// Keep track of units attacks this turn
	public int playerTeamSize;
	bool flag = false;	// For init playerTeam
	bool battle;	// To confirm combat
	// Have the same for the Enemy team
	public GameObject[] enemyTeamManager;
	public bool[] enemyHasMoved;
	public int enemyTeamSize;
	int enemyBattleIndex;
	
	
	void Awake(){
		// This doesn't work on Start, so maybe here
		// playerTeamManager = GameObject.FindGameObjectsWithTag("Player");
		// enemyTeamManager = GameObject.FindGameObjectsWithTag("Enemy");
	}
    // Start is called before the first frame update
    void Start(){
		// lvlInit = GetComponent<LevelInitializer>();
		combat = data.GetComponent<CombatSystem>();
		pInteractions = GetComponent<PlayerInteractions>();
		
		playerTeamSize = playerTeamManager.Length;
		enemyBattleIndex = 0;
		// enemyTeamSize = enemyTeamManager.Length;
		battle = false;		// Init
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
				if(battle){
					battle = false;
				}
				SelectUnit();
			}
			
			// Space bar confirms unit movement
			if(Input.GetKeyUp(KeyCode.Space)){
				if(!battle){
					// Debug.Log("Move Unit.");
					MoveUnit();
					battle = true;	// Enable battle
				}
				else {
					// Engage in combat
					Debug.Log("Begin Combat");
					DoBattle();
					battle = false;
				}
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
			pInteractions.activeUnit = activeCharacter;
			data.SetSelectedUnit(-1);	// Make it null
			activeIndex = -5;
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
		playerHasAttacked = new bool[playerTeamSize];
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
			playerHasAttacked[i] = false;
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
		// Debug.Log("Mouse click: " + Input.mousePosition.x + " " + Input.mousePosition.y + " " + Input.mousePosition.z + " ");
		
		if(Physics.Raycast(ray, out hit, 200)){
			// Debug.Log("Ray hit: " + hit.point.ToString());
			// Save positions in local context
			float worldX = hit.point.x;
			float worldY = hit.point.y;
			float worldZ = hit.point.z;
			// Round to int (for easier calculation, finding node)
			int x = Mathf.RoundToInt(worldX);
			int y = Mathf.RoundToInt(worldY) + 1;
			int z = Mathf.RoundToInt(worldZ);
			
			// Debug.Log("Mouse: " + x + " " + y + " " + z);
			for(int i = 0; i < playerTeamSize; i++){
				// Debug.Log("Unit Position: " + playerTeamManager[i].transform.position.ToString());
				// if(Vector3.Equals(new Vector3(x,y,z), playerTeamManager[i].transform.position)){	// Float Inequality
				if(new Vector3(x,y,z) == playerTeamManager[i].transform.position){
					// Debug.Log("Test turn 2");
					if(playerHasMoved[i] == false){	// Player has not moved
						// Debug.Log("Index = " + i);
						activeIndex = i;
						data.SetSelectedUnit(i);
						return playerTeamManager[i];
					}
					else {
						Debug.Log("Unit " + i + " cannot move again.");
						return null;	// Unit cannot move again
					}
				}
				else {
					Debug.Log("Failed. hasMoved = " + playerHasMoved[i]);
				}
			}
		}
		// If we get here, there is no unit selected
		// Debug.Log("Unit not found");
		return null;
	}
	
	// This class will invoke the CombatSystem
	public void DoBattle(){
		Character attacker = null;
		Character defender = null;
		// bool isMagic;
		
		// Select the enemy to attack
		// Place enemy units so they can only be attacked in order
		if(enemyBattleIndex == 0){	// Needs to be updated
			defender = data.swordEnemy;
			// Debug.Log("Defender: " + defender.ClassName);
		}
		else if(enemyBattleIndex == 1){	
			defender = data.lanceEnemy;
			// Debug.Log("Defender: " + defender.ClassName);
		}
		else if(enemyBattleIndex == 2){
			defender = data.axeEnemy;
			// Debug.Log("Defender: " + defender.ClassName);
		}
		else {
			Debug.Log("Error: Defender not within list.");
			return;
		}
		
		// Pass the active player unit	
		if(activeIndex == 0){	// This is always Ahagan (Main char)
			attacker = data.ahagan;
			// isMagic = true;
			// Debug.Log("Attacker: " + attacker.ClassName);
		}
		else if(activeIndex == 1){	
			attacker = data.secondCharacter;
			// isMagic = false;
			// Debug.Log("Attacker: " + attacker.ClassName);
		}
		else if(activeIndex == 2){
			attacker = data.thirdCharacter;
			// isMagic = false;
			// Debug.Log("Attacker: " + attacker.ClassName);
		}
		else {
			Debug.Log("Error: Active Unit not within list.");
			return;
		}
		
		if(attacker != null && defender != null){
			// Invoke the CombatSystem (ref Character attacker, ref Character defender, bool isMagic)
			CombatSystem.RunCombatSystem(ref attacker, ref defender);		// Static method
			// combat.RunCombatSystem(ref attacker, ref defender, isMagic);
			Debug.Log("Combat Invoked: " + attacker.ClassName + " vs " + defender.ClassName);
		}
		else {
			Debug.Log("Error: CombatSystem failed to invoke.");
		}
	}
}
