using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DiePopup : MonoBehaviour {
	#region singletonImplementation
	static DiePopup instance = null;
	public static DiePopup Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<DiePopup>();
			}
			return instance;
		}
	}
	DiePopup() {
		//save time instead of searching for the game controller
		//check if the instance is not null, we are creating more than one instance, warn us
		if (instance != null) {
			Debug.LogError("There's an instance already created, click on the next error to check it", gameObject);
			Debug.LogError("Original DiePopup instance is",instance.gameObject);
			return;
		}
		instance = this;
	}
	#endregion

	public Text killCount;
	public GameObject continueGO;
	// Use this for initialization
	void Start () {
		Invoke("deactivate",2f);
	}

	void deactivate() {
		gameObject.SetActive(false);
	}
	void activate() {
		CancelInvoke("deactivate");
		gameObject.SetActive(true);
	}

	/// <summary>
	/// Hides the pop up.
	/// </summary>
	public void hidePopUp() {
		SoundEffectsController.Instance.playSoundEffectOneShot("openmenu");
		Invoke("deactivate",1f);
		GetComponent<Animator>().CrossFade("Hide",0.1f);
	}

	/// <summary>
	/// Shows the pop up.
	/// </summary>
	/// <param name="totalKills">Total kills.</param>
	/// <param name="contiueAllowed">If set to <c>true</c> contiue allowed.</param>
	public void showPopUp(int totalKills, bool contiueAllowed) {
		SoundEffectsController.Instance.playSoundEffectOneShot("openmenu");
		activate();
		GetComponent<Animator>().CrossFade("Show",0.1f);
		killCount.text = totalKills.ToString();
		continueGO.SetActive(contiueAllowed);
	}

	public void onSocialNetworkShareSuccess() {
		if (continueGO.activeInHierarchy) {
			hidePopUp();
			ZonesController.Instance.onContinueSuccess();
		}
	}
	public void onCloseButtonClicked() {
		Debug.Log("onCloseButtonClicked");
		SoundEffectsController.Instance.playSoundEffectOneShot("button");
		hidePopUp();
		BlackLoadingScreen.Instance.Fadein();
		ZonesController.Instance.Invoke("ResetController",1f);
	}
}
