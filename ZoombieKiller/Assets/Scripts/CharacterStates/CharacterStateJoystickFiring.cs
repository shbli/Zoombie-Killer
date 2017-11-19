using UnityEngine;
using System.Collections;

public class CharacterStateJoystickFiring : CharacterStateBase {
	
	Vector3 currLocation = new Vector3();
	int framesSinceLastFire = 0;
	Vector3 moveToPoint = Vector3.zero;
	float lookSpeed = 6f;
	// Use this for initialization
	void Start () {
	}
	
	public void fireJoystickUpdate(Vector3 targetPosition) {
		framesSinceLastFire = 0;
		if (Vector3.Distance(transform.position,targetPosition) > 0.3f) {
			//let's rotate our player
			currLocation.x = transform.position.x;
			currLocation.z = transform.position.z;
			currLocation.y = transform.position.y;
			Vector3 PrevLocation = currLocation;
			currLocation.x = targetPosition.x;
			currLocation.z = targetPosition.z;
			if (currLocation != PrevLocation) {
				parentChar.mAnimator.transform.rotation = Quaternion.Lerp (parentChar.mAnimator.transform.rotation,  Quaternion.LookRotation(currLocation - PrevLocation), Time.deltaTime * lookSpeed);		
			}

			// we are not in firing state
			ChangeCharacterState( this );
		}
	}

	public override void startState() {
		//make sure we are running the run animation
		if (GetComponent<Hero>().mAttachedWeaponType == Hero.WeaponType.multiFire) {
			runAnimation("multiShot");
		} else if (GetComponent<Hero>().mAttachedWeaponType == Hero.WeaponType.pistol) {
			runAnimation("pistolFire");
		}
	}

	public override void excuteState() {
		framesSinceLastFire++;
		if (framesSinceLastFire > 1) {
			//I'm not moving neither firing
			parentChar.ChangeCharacterState(GetComponent<CharacterStateIdle>());
		}
	}

	public override void endState() {
		animationRunningCompleted();
	}
}