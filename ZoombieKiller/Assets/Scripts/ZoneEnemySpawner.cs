using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneEnemySpawner : MonoBehaviour {
	public List <GameObject> mEnemies = new List<GameObject>();
	public Transform firstPos;
	public Transform secondPos;
	public Transform heroPos;

	int totalCount = 0;

	// Use this for initialization
	void Start () {
		Destroy(GetComponent<Camera>());
		Destroy(firstPos.GetComponent<MeshFilter>());
		Destroy(firstPos.GetComponent<MeshRenderer>());

		Destroy(secondPos.GetComponent<MeshFilter>());
		Destroy(secondPos.GetComponent<MeshRenderer>());

		Destroy(heroPos.GetComponent<MeshFilter>());
		Destroy(heroPos.GetComponent<MeshRenderer>());
	}

	public void reset() {
		totalCount = Random.Range(4 + ZonesController.Instance.zoneLoopCount,7 + ZonesController.Instance.zoneLoopCount);
	}

	public void spawnEnemies() {
		if (totalCount > 0) {
			//let's start zoombies roring sound
			SoundEffectsController.Instance.playSoundEffectsLoop("zombies");
			totalCount--;
			Invoke("spawnEnemies",Random.Range(2f,4f));
			int randomPos = Random.Range(1,3);
			Vector3 enemySpawnPos;
			if (randomPos == 1) {
				enemySpawnPos = firstPos.position;
			} else {
				enemySpawnPos = secondPos.position;
			}
			randomPos = Random.Range(1,4);
			if (randomPos == 1) {
				enemySpawnPos += new Vector3(0,0,3f);
			} else if (randomPos == 2) {
				enemySpawnPos += new Vector3(0,0,-3f);
			} else {
				enemySpawnPos += new Vector3(0,0,0);
			}
			GameObject spawnedEnemy = GameObject.Instantiate(mEnemies[Random.Range(0,mEnemies.Count)]) as GameObject;
			spawnedEnemy.transform.position = enemySpawnPos;
			spawnedEnemy.SetActive(true);
		}
	}
}
