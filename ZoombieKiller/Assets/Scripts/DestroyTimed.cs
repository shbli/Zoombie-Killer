using UnityEngine;
using System.Collections;

public class DestroyTimed : MonoBehaviour {
	public float destroyAfter = 2f;
	// Use this for initialization
	void Start () {
		Invoke("destroySelf",destroyAfter);
	}

	void destroySelf() {
		Destroy(gameObject);
	}
}
