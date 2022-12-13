using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not sure if this needs MonoBehaviour
public class CombatSystem : MonoBehaviour{
	// Get class instances
	public static Data data;
	
	void Awake(){
		// Initialize scripts
		data = GetComponent<Data>();
	}
	
    // Running method for this class
	public static void RunCombatSystem(ref Character attacker, ref Character defender){
		// Debug.Log("Attacker Damage " + attacker.MagicDamage);
		// Debug.Log("Defender Damage: " + defender.AttackDamage);
		// NOTE: Defender can counter attack once!
		int attackFreq;

		// Not going to violate math
		if(defender.Speed == 0){
			attackFreq = 2;
		}else if(attacker.Speed > defender.Speed){
			attackFreq = attacker.Speed / defender.Speed;	// Defender can only counterattack once
		}else{
			attackFreq = 1;
		}

		int AtkDmg = GetDamageOfUnit(ref attacker);
		int DefDmg = GetDamageOfUnit(ref defender);
		
		// Debug.Log("AtkDmg: " + AtkDmg);
		// Debug.Log("DefDmg: " + DefDmg);

		AtkDmg = ApplyDefense(ref attacker, ref defender, AtkDmg);
		DefDmg = ApplyDefense(ref defender, ref attacker, DefDmg);
		
		// Debug.Log("AtkDmg after Defense: " + AtkDmg);
		// Debug.Log("Do attack. " + attackFreq);
		
		for(int i = 0; i < attackFreq; ++i){
			// defender.Hp -= AtkDmg;
			defender.Hp -= AtkDmg + 10;		// For quickness in demo
			// Defender Counter attack
			if (i == 0)		// Defender counterattack (only once)
				attacker.Hp -= DefDmg;
		}
		
		Debug.Log("Attacker HP: " + attacker.Hp);
		Debug.Log("Defender HP: " + defender.Hp);
		// May or may not need to Update data storage
		if(attacker.Hp <= 0){
			data.HasDied(ref attacker);
			Debug.Log("Attacker has died: " + attacker.Hp);
		}
		if (defender.Hp <= 0){
			data.HasDied(ref defender);
			Debug.Log("Defender has died: " + defender.Hp);
		}
	}

	static int GetDamageOfUnit(ref Character unit){
		// Need unit.Equpped not made an attribute yet
		// unit.Equipped.Type needed aswell to see if attacking with physical or magic

		if(unit.Equipped.Type.Equals("Physical")){
			return unit.AttackDamage + (unit.Equipped.Damage / 10);
		}else{
			return unit.MagicDamage + (unit.Equipped.Damage / 10);
		}
	}

	static int ApplyDefense(ref Character attacker, ref Character defender, int damage){
		if(attacker.Equipped.Type.Equals("Physical")){
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