using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour{
	// Keep a single instance of the DataStorage
	public static Data Instance;
	// The base class for all characters 
	// public Character character;
	public Mage ahagan;		// Main Character
	public Bruiser secondCharacter;
	public Swordsman thirdCharacter;
	
	// Enemy scripts (Depends on the level)
	public Swordsman swordEnemy;
	public Bruiser axeEnemy;
	public Soldier lanceEnemy;
	
	public Character selectedUnit;
	
    // Start is called before the first frame update
    void Start(){
		// Singleton
		if (Instance != null){
			Destroy(gameObject);
			return;
		}
		
		Instance = this;
        Object.DontDestroyOnLoad(this);
		// character = GetComponent
		
		selectedUnit = null;
		
		InitializePlayerTeam();
		InitializeEnemyTeam();
    }

	// This is ran after loading another scene
    void OnLoad(){
		
	}
	
	void Update(){
		/*
		// Test
		if(Input.GetKeyDown("i")){
			Debug.Log("Name = " + ahagan.ClassName);
		}
		*/
	}
	
	// First-time initialization
	void InitializePlayerTeam(){
		// Get the class scripts
		ahagan	= new Mage();
		ahagan.ClassName = "Ahagan";
		ahagan.ClassDesc = "Grand Mage";
		
		secondCharacter = new Bruiser();
		// ahagan.ClassName = "Ahagan";
		
		thirdCharacter = new Swordsman();
		// ahagan.ClassName = "Ahagan";
	}
	
	// First-time initialization
	void InitializeEnemyTeam(){
		swordEnemy = new Swordsman();
		swordEnemy.ClassName = "Prision Guard";
		
		axeEnemy = new Bruiser();
		axeEnemy.ClassName = "Prision Guard";
		
		lanceEnemy = new Soldier();
		lanceEnemy.ClassName = "Prision Guard";
	}
	
	// A character has died
	public void HasDied(ref Character ded){
		if(ded == ahagan){
			Debug.Log("Game Over");
		}
		else if(ded == secondCharacter){
			secondCharacter = null;
		}
		else if(ded == thirdCharacter){
			thirdCharacter = null;
		}
		else if(ded == swordEnemy){
			swordEnemy = null;
		}
		else if(ded == axeEnemy){
			axeEnemy = null;
		}
		else if(ded == lanceEnemy){
			lanceEnemy = null;
		}
		
		/*
		switch(ded){
			case ahagan:
				Debug.Log("Agahan has died.");
				// Game over!
				break;
			case secondCharacter:
				Debug.Log("Second Char has died.");
				secondCharacter = null;		// Remove from storage
				// Destroy(deadChar);
				break;
			case thirdCharacter:
				Debug.Log("Third Char has died.");
				thirdCharacter = null;		// Remove from storage
				break;
			case swordEnemy:
				swordEnemy = null;
				break;
			case axeEnemy:
				axeEnemy = null;
				break;
			case lanceEnemy:
				lanceEnemy = null;
				break;
			default:
				Debug.Log("Error: HasDied = Unknown");
				break;
		}
		*/
	}
	
	// Setter/Getter (More cheese)
	public void SetSelectedUnit(int i){
		switch (i){
			case 0:
				selectedUnit = ahagan;
				break;
			case 1:
				selectedUnit = secondCharacter;
				break;
			case 2:
				selectedUnit = thirdCharacter;
				break;
			default:
				selectedUnit = null;
				// Debug.Log("Error: Data.SetSelectedUnit");
				break;
		}
	}
	
	public Character GetSelectedUnit(){
		return selectedUnit;
	}
}