using UnityEngine;
using System.Collections;

public abstract class CharacterStateBase : MonoBehaviour {

	string animationStateName;
	public CharacterBase parentChar {
		get {
//			if (m_parentChar == null) {
//				m_parentChar = GetComponent<CharacterBase>();
//			}
			return m_parentChar;
		}
	}
	CharacterBase m_parentChar = null;

    private void Awake()
    {
        m_parentChar = GetComponent<CharacterBase>();
    }

    public abstract void startState ();
	public abstract void excuteState ();
	public abstract void endState ();

	public void ChangeCharacterState(CharacterStateBase targetState) {
		parentChar.ChangeCharacterState(targetState);
	}

	public void runAnimation(string pAnimationStateName) {
		animationStateName = pAnimationStateName;
		tryRuningAnimationAgain();
	}

	public void tryRuningAnimationAgain() {
		if (parentChar.getCharacterState() == this) {
			if (parentChar.mAnimator.IsInTransition(0)) {
				parentChar.mAnimator.Play(animationStateName);
				Invoke("tryRuningAnimationAgain",0.2f);
				return;
			}
			if (Animator.StringToHash(animationStateName) != parentChar.mAnimator.GetCurrentAnimatorStateInfo(0).tagHash) {
				parentChar.mAnimator.CrossFade(animationStateName,0.1f,0,0f);
				Invoke("tryRuningAnimationAgain",0.5f);
			}
		}
	}

	public void animationRunningCompleted() {
		CancelInvoke("tryRuningAnimationAgain");
	}
}
