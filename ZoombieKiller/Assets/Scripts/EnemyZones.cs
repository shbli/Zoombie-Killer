using UnityEngine;
using System.Collections;

public class EnemyZones : MonoBehaviour {
	// Use this for initialization
	void Start () {
		ZonesController.Instance.setZonesParent(gameObject);
		Destroy(this);
	}
}
