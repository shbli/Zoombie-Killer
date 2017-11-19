using UnityEngine;
using System.Collections;

public class CharacterStateEnemyWin : CharacterStateBase {
	// Use this for initialization
	void Start () {
	}
	
	//called when the state starts
	public override void startState() {
		runAnimation("idle");
		parentChar.lockStateChange = true;
		parentChar.mRigidBody.velocity = Vector3.zero;
	}

	//called each update while state is active
	public override void excuteState() {
	}
	
	public override void endState() {
		animationRunningCompleted();
	}
}
