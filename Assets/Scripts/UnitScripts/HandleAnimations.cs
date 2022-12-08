using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitControl{
	public class HandleAnimations : MonoBehaviour{
		Animator anim;
		UnitStates states;
		
		// Start is called before the first frame update
		void Start()
		{
			states = GetComponent<UnitStates>();
			SetupAnimator();
		}

		// Update is called once per frame
		void Update()
		{
			anim.SetFloat("Movement", (states.move) ? 1 : 0, 0.1f, Time.deltaTime);
			states.movingSpeed = anim.GetFloat("Movement") * states.maxSpeed;
		}
		
		void SetupAnimator(){
			anim = GetComponent<Animator>();
			Animator[] a = GetComponentsInChildren<Animator>();
			
			for(int i = 0; i < a.Length; i++){
				if(a[i] != anim){
					anim.avatar = a[i].avatar;
					Destroy(a[i]);
					break;
				}
			}
		}
	}
}