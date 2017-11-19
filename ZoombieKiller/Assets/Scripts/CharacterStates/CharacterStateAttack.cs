using UnityEngine;
using System.Collections;

public class CharacterStateAttack : CharacterStateBase {

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
		//make sure we are running the run animation
		runAnimation("attack1");
		target = GameController.Instance.GameHero.transform;
	}

	public override void excuteState() {
		if (GameController.Instance.GameHero.isDead) {
			ChangeCharacterState(GetComponent<CharacterStateEnemyWin>());
			return;
		}
		if ( Vector3.Distance(transform.position,target.position) > (mEnemy.distanceFromHero + 0.2f) ) {
			ChangeCharacterState(GetComponent<CharacterStateFollowHero>());
			return;
		}
		//we are still attacking the the hero
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
	}

	public override void endState() {
		animationRunningCompleted();
	}
}
