using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	#region singletonImplementation
	static MainMenu instance = null;
	public static MainMenu Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<MainMenu>();
			}
			return instance;
		}
	}
	MainMenu() {
		//save time instead of searching for the game controller
		//check if the instance is not null, we are creating more than one instance, warn us
		if (instance != null) {
			Debug.LogError("There's an instance already created, click on the next error to check it", gameObject);
			Debug.LogError("Original MainMenu instance is",instance.gameObject);
			return;
		}
		instance = this;
	}
	#endregion

	public Text killCount;
	public Animator boyAnimator;
	public Animator girlAnimator;
	public Button boyButton;
	public Button girlButton;

	public GameObject virtualJoystick;
	public GameObject firingVirtualJoystick;

	// Use this for initialization
	void Start () {
		Invoke("prepMenu",3f);
	}

	public void prepMenu() {
		gameObject.SetActive(true);
		SoundEffectsController.Instance.playSoundEffectOneShot("menuOpen");
		BlackLoadingScreen.Instance.Invoke("Fadeout",1f);
		virtualJoystick.SetActive(false);
		firingVirtualJoystick.SetActive(false);
		killCount.text = PlayerPrefs.GetInt(topScoreKey,0).ToString();
		if (ZonesController.Instance.isBoy) {
			onBoySelected();
		} else {
			onGirlSelected();
		}
	}

	public void onBoySelected() {
		SoundEffectsController.Instance.playSoundEffectOneShot("button");
		ZonesController.Instance.isBoy = true;
		boyButton.enabled = false;
		girlButton.enabled = true;
		boyAnimator.CrossFade("happy",0.1f);
		girlAnimator.CrossFade("sad",0.1f);
	}

	public void onGirlSelected() {
		SoundEffectsController.Instance.playSoundEffectOneShot("button");
		ZonesController.Instance.isBoy = false;
		girlButton.enabled = false;
		boyButton.enabled = true;
		girlAnimator.CrossFade("happy",0.1f);
		boyAnimator.CrossFade("sad",0.1f);
	}

	public void onMainMenuClicked() {
		SoundEffectsController.Instance.playSoundEffectOneShot("button");
		BlackLoadingScreen.Instance.Fadein();
		Invoke("hideMainMenu",1f);
	}

	public void hideMainMenu() {
		virtualJoystick.SetActive(true);
		firingVirtualJoystick.SetActive(true);
		gameObject.SetActive(false);
		ZonesController.Instance.startNewGame();
	}

	const string topScoreKey = "topScoreKey";
	public void onNewScoreAcquired(int pScore) {
		//we scored a new top score
		if (pScore > PlayerPrefs.GetInt(topScoreKey,0)) {
			PlayerPrefs.SetInt(topScoreKey,pScore);
		}
	}
	public void onTwitterClicked() {
	}
	public void onFacebookClicked() {
	}
}
