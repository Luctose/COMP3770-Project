using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelControl{
	public class LevelManager : MonoBehaviour{
		// This is a list of floors containing levels
		// Objects using LevelObject class
		public List<ObjsPerFloor> lvlObjects = new List<ObjsPerFloor>();
		
		public static LevelManager instance;
		public static LevelManager GetInstance(){
			return instance;
		}
		
		void Awake(){
			instance = this;
		}
		
		// Each instance of ObjsPerFloor is an individual floor
		[System.Serializable]
		public class ObjsPerFloor{
			public int floorIndex;
			public List<GameObject> objs = new List<GameObject>();
		}
	}
}