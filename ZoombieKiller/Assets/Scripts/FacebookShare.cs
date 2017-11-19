using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FacebookShare : MonoBehaviour {
	const string messageToShare = "I challenge you to defeat my score at #BrutalWorld by @COMPASSGAMES https://itunes.apple.com/us/app/brutalworld/id1089983157?ls=1&mt=8";
	// Use this for initialization
	void Start () {
		SPFacebook.OnInitCompleteAction += OnInit;
		SPFacebook.OnAuthCompleteAction += OnAuth;
		SPFacebook.OnPostingCompleteAction += OnPost;
	}

	void enableButton() {
		GetComponent<Button>().enabled = true;
	}

	public void onFaceBookShareClicked() {
		Debug.Log("onFaceBookShareClicked");
		SoundEffectsController.Instance.playSoundEffectOneShot("button");
//		StartCoroutine(PostFBScreenshot());
//		//TODO: Fix facebook share code
		GetComponent<Button>().enabled = false;
		Invoke("enableButton",10f);

		if (SPFacebook.Instance.IsInited) {
			//share
			initSucces();
			return;
		}

		SPFacebook.instance.Init();
	}

	IEnumerator PostFBScreenshot() {
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();

		SPFacebook.Instance.PostImage(messageToShare,tex);
		//SPShareUtility.FacebookShare(messageToShare,tex);
	}

	private void OnInit() {
		initSucces();
	}

	void initSucces() {
		if(SPFacebook.instance.IsLoggedIn) {
			onAuthSuccess();
			return;
		}

		SPFacebook.instance.Login("publish_actions");
	}

	private void OnAuth(FB_Result result) {
		if(SPFacebook.instance.IsLoggedIn) {
			onAuthSuccess();
		} else {
			Debug.Log("Failed to log in");
		}

	}

	void onAuthSuccess() {
		StartCoroutine(PostFBScreenshot());
	}

	private void OnPost(FB_PostResult res) {
		if(res.IsSucceeded) {
			Debug.Log("Posting complete");
			Debug.Log("Posy id: " + res.PostId);
			if (transform.parent.GetComponent<DiePopup>()) {
				transform.parent.GetComponent<DiePopup>().onSocialNetworkShareSuccess();
			}
		} else {
			Debug.Log("Oops, post failed, something was wrong");
		}
	}
}
