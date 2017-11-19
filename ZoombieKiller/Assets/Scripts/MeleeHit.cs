using UnityEngine;
using System.Collections;

public class MeleeHit : MonoBehaviour {
	public GameObject meleeExplosiveEffect;
	// Use this for initialization
	void Start () {
	}

	void destroySelf() {
		Destroy(gameObject);
	}

	void showExplosiveEffect(Transform pOnTransform) {
		if (meleeExplosiveEffect != null) {
			//use the "Y" or height from the bullet, and position it on the character x/z
			GameObject explosiveCopy = GameObject.Instantiate(meleeExplosiveEffect,new Vector3(pOnTransform.position.x,transform.position.y,pOnTransform.position.z),pOnTransform.rotation) as GameObject;
			explosiveCopy.gameObject.SetActive(true);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponent<Hero>() != null) {
			SoundEffectsController.Instance.playSoundEffectOneShotIfAllowancePassedSinceLastPlayed("zombieSmash",0.5f);
			collision.gameObject.GetComponent<Hero>().decreaseHealthBy(1f);
			showExplosiveEffect(collision.transform);
		}
	}
}
