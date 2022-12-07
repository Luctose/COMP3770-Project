using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitControl{
	// This is to make the Unit "hold" the weapon with their hand
	// Not sure if needed
	public class IKHandler : MonoBehaviour{
		public Transform leftHandTarget;
		public float leftHandWeight;
		
		Animator anim;
		
		// Start is called before the first frame update
		void Start()
		{
			anim = GetComponent<Animator>();
		}

		void OnAnimatorIK(){
			if(leftHandTarget){
				anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
				anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
				anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
				anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
			}
		}
	}
}