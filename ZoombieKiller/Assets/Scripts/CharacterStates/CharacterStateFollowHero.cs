using UnityEngine;
using System.Collections;

public class CharacterStateFollowHero : CharacterStateObjectAvoidanceBase {

	Vector3 currLocation = new Vector3();
	int framesSinceLastMove = 0;
	Vector3 moveToPoint = Vector3.zero;
	float lookSpeed = 10f;
	Transform target;
	Enemy mEnemy;

	void Start() {
		mEnemy = GetComponent<Enemy>();
	}
	public override void startState() {
		base.startState();
		//make sure we are running the run animation
		runAnimation("funnyWalk");
		target = GameController.Instance.GameHero.transform;
		if (mEnemy == null) {
			mEnemy = GetComponent<Enemy>();
		}
	}

	public override void excuteState() {
		if (GameController.Instance.GameHero.isDead) {
			ChangeCharacterState(GetComponent<CharacterStateEnemyWin>());
			return;
		}

		targetPosition = target.position;
		base.excuteState();
		if (isObjectAvoidanceActive) {
			return;
		}
		if (Vector3.Distance(transform.position,target.position) > mEnemy.distanceFromHero) {
			//we are still far from the hero
			moveToPoint.x = target.position.x;
			moveToPoint.y = 0f;
			moveToPoint.z = target.position.z;

			//let's rotate our player
			currLocation.x = transform.position.x;
			currLocation.z = transform.position.z;
			currLocation.y = transform.position.y;
			Vector3 PrevLocation = currLocation;
			currLocation.x = target.position.x;
			currLocation.z = target.position.z;
			if (currLocation != PrevLocation) {
				parentChar.mAnimator.transform.rotation = Quaternion.Lerp (parentChar.mAnimator.transform.rotation,  Quaternion.LookRotation(currLocation - PrevLocation), Time.deltaTime * lookSpeed);		
			}

			//actually applying the movement of the player
			Vector3 diffrence = moveToPoint - transform.position;
			diffrence.Normalize();
			Vector3  moveDirection = diffrence * parentChar.charSpeed;
			parentChar.mRigidBody.velocity = moveDirection;
		} else {
			ChangeCharacterState(GetComponent<CharacterStateAttack>());
		}

	}

	public override void endState() {
		base.excuteState();
		animationRunningCompleted();
		parentChar.mRigidBody.velocity = Vector3.zero;
	}
}
