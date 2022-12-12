using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not sure if this needs MonoBehaviour
public class CombatSystem : MonoBehaviour{
	// Get class instances
	// DataStorage data;
	// Attacker Scripts & Stats
	// int attackerHP, attackerDamage, attackerDefense, attackerSpeed;
	// Defender Scripts & Stats
	// int defenderHP, defenderDamage, defenderDefense, defenderSpeed;
	// Weapons equipped
	// bool magic;
	// int attackerWeaponDMG, attackerWeaponSpeed, defenderWeaponDMG, defenderWeaponSpeed;
	// Animation stuff
	
	void Awake(){
		// Initialize scripts
	}
	
    // Running method for this class
	public void RunCombatSystem(GameObject atatcker, GameObject defender, bool isMagic){
		// magic = isMagic;
		// GetAttackerStats(attacker);
		// GetDefenderStats(defender);
		// CalculateDamage();
		// Update Data Storage
		
	}
	
	// Fetch the Attacker's stats
	void GetAttackerStats(GameObject attacker){
		// Health
		// attackerHP = attackerHealth;
		// Strength/Magic
		// if(magic)
			// this.attackerDamage = attackerMagic;
		// else {this.attackerDamage = attackerDamage;}
		// Speed
		// this.attackerSpeed = attackerSpeed;
		// Weapons
		// attackerWeaponDMG = attackerEquippedWeaponDMG;
	}
	
	// Fetch Defender's Stats
	void GetDefenderStats(GameObject defender){
		// Health
		// Defense/Resistance
		// if(magic)
			// this.defenderDefense = defenderResistance;
		// else {this.defenderDefense = defenderDefense;}
		// Speed
		// this.defenderSpeed = defenderSpeed;
		// Weapons
		// defenderWeaponDMG = defenderEquippedWeaponDMG;
	}
	
	// Calculate the Damage
	void CalculateDamage(){
		// NOTE: Defender can counter attack once!
		// int attackFreq = attackerSpeed / defenderSpeed;	// Defender can only counterattack once
		// int AtkDmg = attackerDamage + (attackerEquippedWeaponDMG / 10);		This needs tweaking (%?)
		// int DefDmg = defenderDamage + (defenderEquippedWeaponDMG / 10);
		// int totalAttackerDamage = AtkDmg - defenderDefense;
		// int totalDefenderDamage = DefDmg - attackerDefense;
		
		// if(totalAttackerDamage < 0){
			// Make it 0 damage
			// totalAttackerDamage = 0;
		// if(totalDefenderDamage < 0){
			// Make it 0 damage
			// totalDefenderDamage = 0;
		
		// for(int i = 0; i < attackFreq; i++){			
			// defenderHP -= totalDamage;
			// Defender Counter attack
			// if (i == 0)		// Defender counterattack (only once)
				// attackerHP -= totalDefenderDamage;
		// }
	}
}