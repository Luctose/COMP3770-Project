using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hold variables for other classes to access
// Can be changed in Inspector
namespace UnitControl{
	// [RequireComponent(typeof(HandleAnimations))]
	public class UnitStates : MonoBehaviour{
		public int team;
		public int health;
		public bool selected;	// 
		public bool hasPath;
		public bool move;
		public bool hasMoved;	// If unit has moved in current turn
		
		public float maxSpeed = 6;
		public float movingSpeed;
	}
}