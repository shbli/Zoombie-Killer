using UnityEngine;
using System.Collections;

public abstract class CharacterBase : MonoBehaviour {
	protected CharacterStateBase currentState;
	CharacterStateBase prevState;
	[HideInInspector]
	public Animator mAnimator;

	[HideInInspector]
	public Rigidbody mRigidBody;

	public float charSpeed;

	[HideInInspector]
	public bool lockStateChange = false;

	[SerializeField]
	protected float mHealth = 100f;
	protected float originalHealth;

	[SerializeField]
	HealthBar mBar;

	CharacterStateDead deadState;
	public bool isDead {
		get {
			if (currentState == deadState) {
				return true;
			}
			return false;
		}
	}

	// Use this for initialization
	protected void Start () {
		originalHealth = mHealth;
		mAnimator = GetComponentInChildren<Animator>();
		mRigidBody = GetComponent<Rigidbody>();
		loadCharacterStates();
	}

	protected virtual void loadCharacterStates() {
		deadState = gameObject.AddComponent<CharacterStateDead>();
		gameObject.AddComponent<CharacterStateHit>();
	}

	public abstract void onHitStateCompleted();

	public CharacterStateBase getCharacterState() {
		return currentState;
	}

	public void decreaseHealthBy(float pAmount) {
		mHealth -= pAmount;
		if (mBar != null) {
			mBar.setBarPercantage(100f*(mHealth/originalHealth));
		}
		if (mHealth <= 0) {
			lockStateChange = false;
			ChangeCharacterState(GetComponent<CharacterStateDead>());
			return;
		}
		GetComponent<CharacterStateHit>().tryToAnimateHit();
	}

	public void ChangeCharacterState(CharacterStateBase targetState) {
		if (lockStateChange) {
			return;
		}
		if (currentState != targetState) {
			// if I'm already dead, it's impossible to change my state
			if (!isDead) {
				currentState.endState();
				prevState = currentState;
				currentState = targetState;
				currentState.startState();
			}
		}
	}

	
	// Update is called once per frame
	protected void Update () {
		currentState.excuteState();
	}
}
