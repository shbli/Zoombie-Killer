using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float speed = 100f;
	public GameObject bulletExplosiveEffect;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.right * speed;
		Invoke("destroySelf",3f);
	}

	void destroySelf() {
		Destroy(gameObject);
	}

	void showExplosiveEffect(Transform pOnTransform) {
		if (bulletExplosiveEffect != null) {
			//use the "Y" or height from the bullet, and position it on the character x/z
			GameObject explosiveCopy = GameObject.Instantiate(bulletExplosiveEffect,new Vector3(pOnTransform.position.x,transform.position.y,pOnTransform.position.z),pOnTransform.rotation) as GameObject;
			explosiveCopy.gameObject.SetActive(true);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponent<Enemy>() != null) {
			collision.gameObject.GetComponent<Enemy>().decreaseHealthBy(1f);
			showExplosiveEffect(collision.transform);
		}
		destroySelf();
	}
}
