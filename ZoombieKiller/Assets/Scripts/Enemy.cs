using UnityEngine;
using System.Collections;

public class Enemy : CharacterBase {
	public float distanceFromHero = 1f;
	// Use this for initialization
	void Start () {
		base.Start();
		mHealth = 5f + ZonesController.Instance.zonesPassed;
		originalHealth = mHealth;
		ZonesController.Instance.addZoneEnemy(this);
	}

	public override void onHitStateCompleted ()
	{
		ChangeCharacterState(GetComponent<CharacterStateFollowHero>());
	}

	protected override void loadCharacterStates ()
	{
		base.loadCharacterStates ();
		//adding the basic state first
		currentState = gameObject.AddComponent<CharacterStateFollowHero>();
		currentState.startState();

		gameObject.AddComponent<CharacterStateAttack>();
		gameObject.AddComponent<CharacterStateEnemyWin>();
	}

	public void onPlayerContinue() {
		lockStateChange = false;
		ChangeCharacterState(GetComponent<CharacterStateFollowHero>());
	}

	// Update is called once per frame
	void Update () {
		base.Update();
	}
}
