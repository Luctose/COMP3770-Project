using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour{
	// The base class for all characters 
	// public Character character;
	public Mage ahagan;		// Main Character
	public Bruiser secondCharacter;
	public Swordsman thirdCharacter;
	
	// Enemy scripts (Depends on the level)
	public Swordsman swordEnemy;
	public Bruiser axeEnemy;
	public Soldier lanceEnemy;
	
    // Start is called before the first frame update
    void Start(){
        Object.DontDestroyOnLoad(this);
		// character = GetComponent
		
		InitializePlayerTeam();
		InitializeEnemyTeam();
    }

	// This is ran after loading another scene
    void OnLoad(){
		
	}
	
	void Update(){
		// Test
		if(Input.GetKeyDown("i")){
			Debug.Log("Name = " + ahagan.ClassName);
		}
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
	public void HasDied(GameObject deadChar){
		
		
		Debug.Log(deadChar + " has died");
		Destroy(deadChar);
	}
}