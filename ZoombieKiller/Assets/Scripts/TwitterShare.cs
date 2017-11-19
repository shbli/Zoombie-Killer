using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwitterShare : MonoBehaviour {
	const string messageToShare = "I challenge you to defeat my score at #BrutalWorld by @COMPASSGAMES https://itunes.apple.com/us/app/brutalworld/id1089983157?ls=1&mt=8";
	// Use this for initialization
	void Start () {
		SPTwitter.Instance.OnTwitterInitedAction += OnInit;
		SPTwitter.Instance.OnAuthCompleteAction += OnAuth;
		SPTwitter.Instance.OnPostingCompleteAction += OnPost;
	}

	void enableButton() {
		GetComponent<Button>().enabled = true;
	}

	public void onTwitterShareClicked() {
		Debug.Log("onTwitterShareClicked");
		SoundEffectsController.Instance.playSoundEffectOneShot("button");
		GetComponent<Button>().enabled = false;
		Invoke("enableButton",10f);
		if (SPTwitter.instance.IsInited) {
			initSuccess();
			return;
		}
		SPTwitter.Instance.Init();
	}

	private void OnInit(TWResult res) {
		if (res.IsSucceeded) {
			initSuccess();
		} else {
			Debug.Log("Unable to Init twitter");
		}

	}

	void initSuccess() {
		if (SPTwitter.instance.IsAuthed) {
			authSuccess();
			return;
		}
		SPTwitter.Instance.AuthenticateUser();
	}

	private void OnAuth(TWResult res) {
		if (res.IsSucceeded) {
			authSuccess();
		} else {
			Debug.Log("Unable to authnticate user");
		}
	}

	void authSuccess() {
		StartCoroutine(PostTWScreenshot());
	}

	IEnumerator PostTWScreenshot() {
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();

		SPTwitter.Instance.Post(messageToShare,tex);
	}


	private void OnPost(TWResult res) {
		if(res.IsSucceeded) {
			Debug.Log("Congrats, you just postet something to twitter");
			if (transform.parent.GetComponent<DiePopup>()) {
				transform.parent.GetComponent<DiePopup>().onSocialNetworkShareSuccess();
			}
		} else {
			Debug.Log("Opps, post failed, something was wrong");
		}
	}
}
