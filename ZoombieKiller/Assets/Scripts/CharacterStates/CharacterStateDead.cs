using UnityEngine;
using System.Collections;

public class CharacterStateDead : CharacterStateBase {
	public override void startState() {
		parentChar.lockStateChange = true;
		Destroy(GetComponent<Rigidbody>());
		Collider[] allColliders = GetComponents<Collider>();
		foreach(Collider _col in allColliders) {
			Destroy(_col);
		}
		runAnimation("die");
		if (GetComponent<Enemy>()) {
			ZonesController.Instance.removeZoneEnemy(GetComponent<Enemy>());
			Invoke("destroySelf",60);
		}
		if (GetComponent<Hero>()) {
			//our hero has died
			ZonesController.Instance.onHeroDie();
		}
	}

	public void destroySelf() {
		Destroy(gameObject);
	}

	public override void excuteState() {
	}

	public override void endState() {
	}
}
