using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelControl{
	public class LevelObject : MonoBehaviour{
		public LvlObjectType objType;
		
		public enum LvlObjectType{
			floor,
			obstacle,
			wall
		}
	}
}