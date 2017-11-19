using UnityEngine;
using System.Collections;

public class Hero : CharacterBase {
	CharacterStateIdle idleState;
	public bool isIdle {
		get {
			if (currentState == idleState) {
				return true;
			}
			return false;
		}
	}

	CharacterStateJoystickFiring joystickFireState;
	public bool isFiring {
		get {
			if (currentState == joystickFireState) {
				return true;
			}
			return false;
		}
	}

	CharacterStateJoystickWalking joystickWalkingState;
	public bool isWalking {
		get {
			if (currentState == joystickWalkingState) {
				return true;
			}
			return false;
		}
	}

	public enum WeaponType {
		pistol = 0,
		multiFire = 1
	}

	[System.Serializable]
	public struct WeaponDetails {
		public WeaponType weaponType;
		public GameObject weaponGO;
		public ParticleSystem mMultiShotParticle;
		public GameObject bullet;
	}

	[SerializeField]
	WeaponDetails mPistol;
	[SerializeField]
	WeaponDetails mMultishot;
	WeaponDetails mAttachedWeapon;
	public bool isMale = true;
	public WeaponType mAttachedWeaponType {
		get {
			return mAttachedWeapon.weaponType;
		}
	}

	// Use this for initialization
	void Start () {
		base.Start();
		mHealth = 15f;
		originalHealth = mHealth;
		gameObject.name = "Hero";
		GameController.Instance.setHero(this);
		attachPistol();
	}

	[ContextMenu("AttachPistol")]
	public void attachPistol() {
		mMultishot.weaponGO.SetActive(false);
		mPistol.weaponGO.SetActive(true);
		mAttachedWeapon = mPistol;
	}

	[ContextMenu("AttachMultiShot")]
	public void attachMultiShotWeapon() {
		mMultishot.weaponGO.SetActive(true);
		mPistol.weaponGO.SetActive(false);
		mAttachedWeapon = mMultishot;
	}

	public override void onHitStateCompleted ()
	{
		ChangeCharacterState(GetComponent<CharacterStateIdle>());
	}

	protected override void loadCharacterStates ()
	{
		base.loadCharacterStates ();

		idleState = gameObject.AddComponent<CharacterStateIdle>();
		joystickWalkingState = gameObject.AddComponent<CharacterStateJoystickWalking>();
		joystickFireState = gameObject.AddComponent<CharacterStateJoystickFiring>();
		gameObject.AddComponent<CharacterStateChangingZone>();

		currentState = idleState;
		currentState.startState();
	}

	public void moveToDirection(float pForwardAmount, float pRightAmount) {
		if (isFiring) {
			return;
		}

		Vector3 newMoveToPosition = 
			transform.position 
			+ (GameController.Instance.transform.forward*pForwardAmount)
			+ (GameController.Instance.transform.right*pRightAmount);

		//visualize the position for testing the virtual joystick
		//transform.FindChild("Cube").position = new Vector3(newMoveToPosition.x,transform.position.y+0.5f,newMoveToPosition.z);
		joystickWalkingState.joystickUpdate(new Vector3(newMoveToPosition.x,transform.position.y+0.5f,newMoveToPosition.z));
	}

	public void fireToDirection(float pForwardAmount, float pRightAmount) {
		Vector3 newMoveToPosition = 
			transform.position 
			+ (GameController.Instance.transform.forward*pForwardAmount)
			+ (GameController.Instance.transform.right*pRightAmount);

		//visualize the position for testing the virtual joystick
		//transform.FindChild("Cube").position = new Vector3(newMoveToPosition.x,transform.position.y+0.5f,newMoveToPosition.z);

		joystickFireState.fireJoystickUpdate(new Vector3(newMoveToPosition.x,transform.position.y+0.5f,newMoveToPosition.z));
	}

	public void firePistol() {
		mAttachedWeapon.mMultiShotParticle.Play();
		GameObject bulletCopy = GameObject.Instantiate(mAttachedWeapon.bullet,mAttachedWeapon.bullet.transform.position,mAttachedWeapon.bullet.transform.rotation) as GameObject;
		bulletCopy.gameObject.SetActive(true);
		GameController.Instance.ShakeCamera();
		if (mAttachedWeaponType == WeaponType.multiFire) {
			SoundEffectsController.Instance.playSoundEffectOneShot("multiShot");
		} else if (mAttachedWeaponType == WeaponType.pistol) {
			SoundEffectsController.Instance.playSoundEffectOneShot("pistol");
		}
	}

	// Update is called once per frame
	void Update () {
		base.Update();
	}
}
