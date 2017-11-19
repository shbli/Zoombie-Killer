using UnityEngine;
using System.Collections;

public class AnimationEventHandler : MonoBehaviour {
	public void firePistol() {
		GetComponentInParent<Hero>().firePistol();
	}
	public void playFootStepSound() {
		SoundEffectsController.Instance.playSoundEffectOneShot("footstep");
	}
	public void hitAnimationCompleted() {
		GetComponentInParent<CharacterStateHit>().onHitAnimationCompleted();
	}
}
