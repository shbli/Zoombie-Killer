using UnityEngine;
using System.Collections;

public class CharacterStateIdle : CharacterStateBase {
	// Use this for initialization
	void Start () {
	}
	
	//called when the state starts
	public override void startState() {
		runAnimation("idle");
	}

	//called each update while state is active
	public override void excuteState() {
	}
	
	public override void endState() {
		animationRunningCompleted();
	}
}
