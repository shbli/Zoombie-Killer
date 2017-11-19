using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZonesController : MonoBehaviour {
	#region singletonImplementation
	static ZonesController instance = null;
	public static ZonesController Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<ZonesController>();
			}
			return instance;
		}
	}
	ZonesController() {
		//save time instead of searching for the game controller
		//check if the instance is not null, we are creating more than one instance, warn us
		if (instance != null) {
			Debug.LogError("There's an instance already created, click on the next error to check it", gameObject);
			Debug.LogError("Original ZonesController instance is",instance.gameObject);
			return;
		}
		instance = this;
	}
	#endregion

	GameObject mZonesParent;
	List <Enemy> zoneEnemies = new List<Enemy>();
	int currentZone = 0;
	int zoombiesKilled = 0;
	public List<GameObject> timeLights = new List<GameObject>();
	int selectedLightIndex = 0;
	bool allowContiue = true;

	[HideInInspector]
	public int zoneLoopCount = 0;
	[SerializeField]
	GameObject boyHero;
	[SerializeField]
	GameObject girlHero;
	[HideInInspector]
	public int zonesPassed = 0;

	public bool isBoy = true;

	const int multiShotZonePassAmount = 6;
	// Use this for initialization
	void Start () {
		zonesPassed = 0;
		zoombiesKilled = 0;
		selectedLightIndex = timeLights.Count-1;
	}

	public void setZonesParent(GameObject pParent) {
		mZonesParent = pParent;
	}

	public void startNewGame() {
		//load the hero
		if (isBoy) {
			(GameObject.Instantiate(boyHero) as GameObject).SetActive(true);
		} else {
			(GameObject.Instantiate(girlHero) as GameObject).SetActive(true);
		}
		CancelInvoke("moveToInitialZone");
		Invoke("moveToInitialZone",0.5f);
	}

	public void onContinueSuccess() {
		Destroy(GameController.Instance.GameHero.gameObject);

		//reload the hero
		if (isBoy) {
			(GameObject.Instantiate(boyHero) as GameObject).SetActive(true);
		} else {
			(GameObject.Instantiate(girlHero) as GameObject).SetActive(true);
		}

		CancelInvoke("onContinueStep2");
		Invoke("onContinueStep2",1f);
	}

	void onContinueStep2() {
		//reset hero pos to center of screen
		GameController.Instance.GameHero.transform.position = 
			new Vector3(
				mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().heroPos.position.x,
				0f,
				mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().heroPos.position.z);

		float enemyContinueDelay = 5f;
		foreach(Enemy _enemy in zoneEnemies) {
			_enemy.Invoke("onPlayerContinue",enemyContinueDelay);
			enemyContinueDelay += 1f;
		}
		if (zonesPassed >= multiShotZonePassAmount) {
			GameController.Instance.GameHero.attachMultiShotWeapon();
		}
	}

	void switchToNextTime() {
		//switch the time
		timeLights[selectedLightIndex].gameObject.SetActive(false);
		selectedLightIndex = ( (selectedLightIndex+1) % timeLights.Count );
		timeLights[selectedLightIndex].gameObject.SetActive(true);
	}

	void moveToInitialZone() {
		currentZone = 0;
		transform.position = mZonesParent.transform.GetChild(currentZone).position;
		GameController.Instance.GameHero.transform.position = 
			new Vector3(
				mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().heroPos.position.x,
				0f,
				mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().heroPos.position.z);

		mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().reset();
		mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().Invoke("spawnEnemies",3f);
		BlackLoadingScreen.Instance.Fadeout();
		GameController.Instance.isControlAllowed = true;
	}

	public void addZoneEnemy(Enemy pEnemy) {
		while(zoneEnemies.Contains(pEnemy)) {
			zoneEnemies.Remove(pEnemy);
		}
		zoneEnemies.Add(pEnemy);
	}
	public void removeZoneEnemy(Enemy pEnemy) {
		zoombiesKilled++;
		while(zoneEnemies.Contains(pEnemy)) {
			zoneEnemies.Remove(pEnemy);
		}
		if (zoneEnemies.Count <= 0) {
			GameController.Instance.isControlAllowed = false;
			SoundEffectsController.Instance.stopSoundEffectLoop("zombies");
			zonesPassed++;
			if (zonesPassed % 2 == 0) {
				switchToNextTime();
				zoneLoopCount++;
			}
			if (zonesPassed >= multiShotZonePassAmount) {
				GameController.Instance.GameHero.attachMultiShotWeapon();
			}
			CancelInvoke("moveToNextZone");
			Invoke("moveToNextZone",3f);
		}
	}
	void moveToNextZone() {
		currentZone++;

		if (currentZone >= mZonesParent.transform.childCount) {
			BlackLoadingScreen.Instance.Fadein();
			CancelInvoke("moveToInitialZone");
			Invoke("moveToInitialZone",2f);
			return;
		}

		iTween.MoveTo(
			gameObject,
			iTween.Hash(
				iTween.HashKeys.position,mZonesParent.transform.GetChild(currentZone).position,
				iTween.HashKeys.time,6f));

		//set the hero new zone target position
		GameController.Instance.GameHero.GetComponent<CharacterStateChangingZone>().targetPosition = 
			new Vector3(
				mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().heroPos.position.x,
				0f,
				mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().heroPos.position.z);
		//change the state so the hero start moving to the new zone
		GameController.Instance.GameHero.ChangeCharacterState(GameController.Instance.GameHero.GetComponent<CharacterStateChangingZone>());
	}

	public void heroZoneChangeCompleted() {
		mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().reset();
		mZonesParent.transform.GetChild(currentZone).GetComponent<ZoneEnemySpawner>().Invoke("spawnEnemies",3f);
		GameController.Instance.isControlAllowed = true;
	}

	public void onHeroDie() {
		MainMenu.Instance.onNewScoreAcquired(zoombiesKilled);
		DiePopup.Instance.showPopUp(zoombiesKilled,allowContiue);
		allowContiue = false;
	}

	public void ResetController() {
		SoundEffectsController.Instance.stopSoundEffectLoop("zombies");
		while(zoneEnemies.Count > 0) {
			Enemy tempEnemy = zoneEnemies[0];
			zoneEnemies.RemoveAt(0);
			Destroy(tempEnemy.gameObject);
		}
		Destroy(GameController.Instance.GameHero.gameObject);
		allowContiue = true;
		zoombiesKilled = 0;
		zoneLoopCount = 0;
		zonesPassed = 0;
		selectedLightIndex = timeLights.Count-1;
		foreach(GameObject _light in timeLights) {
			_light.SetActive(false);
		}
		timeLights[0].gameObject.SetActive(true);
		transform.position = mZonesParent.transform.GetChild(0).position;
		MainMenu.Instance.prepMenu();
	}
}
