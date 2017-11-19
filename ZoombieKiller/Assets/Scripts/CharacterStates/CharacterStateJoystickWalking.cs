using UnityEngine;
using System.Collections;

public class CharacterStateJoystickWalking : CharacterStateBase {
	
	Vector3 currLocation = new Vector3();
	int framesSinceLastMove = 0;
	Vector3 moveToPoint = Vector3.zero;
	float lookSpeed = 6f;
	// Use this for initialization
	void Start () {
	}
	
	public void joystickUpdate(Vector3 targetPosition) {
		framesSinceLastMove = 0;
		if (Vector3.Distance(transform.position,targetPosition) > 0.3f) {
			moveToPoint.x = targetPosition.x;
			moveToPoint.y = 0f;
			moveToPoint.z = targetPosition.z;

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

			//actually applying the movement of the player
			Vector3 diffrence = moveToPoint - transform.position;
			diffrence.Normalize();
			Vector3  moveDirection = diffrence * parentChar.charSpeed;
			parentChar.mRigidBody.velocity = moveDirection;

			// we are not in walking state
			ChangeCharacterState( this );
		}
	}

	public override void startState() {
		//make sure we are running the run animation
		runAnimation("run");
	}

	public override void excuteState() {
		framesSinceLastMove++;
		if (framesSinceLastMove > 1) {
			//the user is not tryin to move the character
			parentChar.ChangeCharacterState(GetComponent<CharacterStateIdle>());
		}
	}

	public override void endState() {
		animationRunningCompleted();
		parentChar.mRigidBody.velocity = Vector3.zero;
	}
}
