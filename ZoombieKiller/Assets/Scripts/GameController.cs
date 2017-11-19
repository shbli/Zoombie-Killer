using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	#region singletonImplementation
	static GameController instance = null;
	public static GameController Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<GameController>();
			}
			return instance;
		}
	}
	GameController() {
		//save time instead of searching for the game controller
		//check if the instance is not null, we are creating more than one instance, warn us
		if (instance != null) {
			Debug.LogError("There's an instance already created, click on the next error to check it", gameObject);
			Debug.LogError("Original GameController instance is",instance.gameObject);
			return;
		}
		instance = this;
	}
	#endregion

	public bool isControlAllowed = false;
	Hero mHero;
	public Hero GameHero {
		get {
			return mHero;
		}
	}
	Vector3 originalCameraRotation;
	// Use this for initialization
	void Start () {
		isControlAllowed = false;
		Application.targetFrameRate = 60;
		originalCameraRotation = transform.eulerAngles;
	}

	public void resetCameraRotation() {
		transform.eulerAngles = originalCameraRotation;
	}

	public void ShakeCamera() {
		//if the camera is not already shaking
		if (!GetComponent<iTween>()) {
			iTween.ShakeRotation(
				gameObject,
				iTween.Hash(
					iTween.HashKeys.name,"cmrashk",
					iTween.HashKeys.amount,Vector3.one*0.5f,
					iTween.HashKeys.time,0.1f,
					iTween.HashKeys.oncomplete,"resetCameraRotation"
				)
			);
		}
	}

	public void setHero(Hero pHero) {
		mHero = pHero;
	}
	public bool isHeroControllable {
		get {
			if (!isControlAllowed) {
				return false;
			}
			if (mHero.isIdle) {
				return true;
			}
			if (mHero.isFiring) {
				return true;
			}
			if (mHero.isWalking) {
				return true;
			}
			return false;
		}
	}
	// Update is called once per frame
	void Update () {
		if (!isHeroControllable) {
			return;
		}

		float forwardMovement = 0f;
		float rightwardMovement = 0f;

		if (Input.GetKey(KeyCode.W)) {
			forwardMovement = 5f;
		}

		if (Input.GetKey(KeyCode.S)) {
			forwardMovement = -5f;
		}

		if (Input.GetKey(KeyCode.D)) {
			rightwardMovement = 5f;
		}

		if (Input.GetKey(KeyCode.A)) {
			rightwardMovement = -5f;
		}

		if (rightwardMovement != 0 || forwardMovement != 0) {
			mHero.moveToDirection(forwardMovement,rightwardMovement);
		}

		forwardMovement = 0f;
		rightwardMovement = 0f;

		if (Input.GetKey(KeyCode.UpArrow)) {
			forwardMovement = 5f;
		}

		if (Input.GetKey(KeyCode.DownArrow)) {
			forwardMovement = -5f;
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			rightwardMovement = 5f;
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			rightwardMovement = -5f;
		}

		if (rightwardMovement != 0 || forwardMovement != 0) {
			mHero.fireToDirection(forwardMovement,rightwardMovement);
		}
	}
}
