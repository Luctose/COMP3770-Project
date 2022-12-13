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
	public static void RunCombatSystem(ref Character attacker, ref Character defender){
		// NOTE: Defender can counter attack once!
		int attackFreq;

		// Not going to violate math
		if(defender.Speed == 0){
			attackFreq = 2;
		}else{
			attackFreq = attacker.Speed / defender.Speed;	// Defender can only counterattack once
		}

		int AtkDmg = GetDamageOfUnit(ref attacker);

		int DefDmg = GetDamageOfUnit(ref defender);

		AtkDmg = ApplyDefense(ref attacker, ref defender, AtkDmg);
		DefDmg = ApplyDefense(ref defender, ref attacker, DefDmg);
		
		for(int i = 0; i < attackFreq; ++i){
			defender.Hp -= AtkDmg;
			// Defender Counter attack
			if (i == 0)		// Defender counterattack (only once)
				attacker.Hp -= DefDmg;
		}
		// May or may not need to Update data storage
	}

	static int GetDamageOfUnit(ref Character unit){
		// Need unit.Equpped not made an attribute yet
		// unit.Equipped.Type needed aswell to see if attacking with physical or magic

		if(true/* unit.Equipped.Type == "physical" */){
			return unit.AttackDamage + (10/*unit.Equipped.Damage*/ / 10);
		}else{
			return unit.MagicDamage + (10/*unit.Equipped.Damage*/ / 10);
		}
	}

	static int ApplyDefense(ref Character attacker, ref Character defender, int damage){
		if(true/* attacker.Equipped.Type == "physical"*/){
			damage = damage - defender.PhysicalResistance;
		}else{
			damage = damage - defender.MagicResistance;
		}

		// Ensure it is not negative
		if(damage < 0){
			damage = 0;
		}
		// Return the net damage
		return damage;
	}
}