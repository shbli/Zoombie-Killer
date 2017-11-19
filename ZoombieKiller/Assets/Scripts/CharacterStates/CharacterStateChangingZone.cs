using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterStateChangingZone : CharacterStateObjectAvoidanceBase {
	// Use this for initialization
	void Start () {
	}
	
	public override void startState() {
		base.startState();
		//make sure we are running the run animation
		runAnimation("run");
	}

	public override void excuteState() {
		base.excuteState();
		if (isObjectAvoidanceActive) {
			return;
		}
		//let's continue with our regular movement here, as our object avoidance is not active
		moveToPoint.x = targetPosition.x;
		moveToPoint.y = 0f;
		moveToPoint.z = targetPosition.z;

		if (Vector3.Distance(transform.position,moveToPoint) > 0.5f) {
			//let's rotate our player
			currLocation.x = transform.position.x;
			currLocation.z = transform.position.z;
			currLocation.y = transform.position.y;
			Vector3 PrevLocation = currLocation;
			currLocation.x = moveToPoint.x;
			currLocation.z = moveToPoint.z;
			if (currLocation != PrevLocation) {
				parentChar.mAnimator.transform.rotation = Quaternion.Lerp (parentChar.mAnimator.transform.rotation,  Quaternion.LookRotation(currLocation - PrevLocation), Time.deltaTime * lookSpeed);		
			}

			//actually applying the movement of the player
			Vector3 diffrence = moveToPoint - transform.position;
			diffrence.Normalize();
			Vector3  moveDirection = diffrence * parentChar.charSpeed;
			parentChar.mRigidBody.velocity = moveDirection;
		} else {
			ChangeCharacterState(GetComponent<CharacterStateIdle>());
			ZonesController.Instance.heroZoneChangeCompleted();
		}
	}

	public override void endState() {
		base.endState();
		animationRunningCompleted();
		parentChar.mRigidBody.velocity = Vector3.zero;
	}
}
