using UnityEngine;
using System.Collections;

public class CharacterStateHit : CharacterStateBase {
	bool allowHit = true;
	bool isHero = false;
	// Use this for initialization
	void Start () {
		allowHit = true;
		if (GetComponent<Hero>() != null) {
			isHero = true;
		} else {
			isHero = false;
		}
	}

	public void tryToAnimateHit() {
		if (isHero) {
			if (parentChar.getCharacterState() == GetComponent<CharacterStateJoystickFiring>()) {
				return;
			}
			if (!allowHit) {
				return;
			}
		}
		ChangeCharacterState(this);
	}

	public void onHitAnimationCompleted() {
		CancelInvoke("onHitAnimationCompleted");
		parentChar.lockStateChange = false;
		parentChar.onHitStateCompleted();
		if (isHero) {
			allowHit = false;
			Invoke("allowHitAgain",Random.Range(10f,15f));
		}
	}

	public void allowHitAgain() {
		allowHit = true;
	}

	//called when the state starts
	public override void startState() {
		parentChar.lockStateChange = true;
		runAnimation("hit");
		//in case the on hit completed did not get called, we call it after one second
		Invoke("onHitAnimationCompleted",1f);
		if (GetComponent<Hero>()) {
			if (GetComponent<Hero>().isMale) {
				SoundEffectsController.Instance.playSoundEffectOneShot("boyHurt");
			} else {
				SoundEffectsController.Instance.playSoundEffectOneShot("girlHurt");
			}
		} else {
			//play hit sound effect
			SoundEffectsController.Instance.playSoundEffectOneShot("zombieHurt");
		}
	}

	//called each update while state is active
	public override void excuteState() {
	}
	
	public override void endState() {
		CancelInvoke("onHitAnimationCompleted");
		animationRunningCompleted();
	}
}
