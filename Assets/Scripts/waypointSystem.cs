using UnityEngine;
using System.Collections;

public class waypointSystem : MonoBehaviour {

	float spawnTimer = 3;
	public GameObject spawnObject;
	int spawn = 0;
	int maxSpawn;
	public bool taken; // If a tower is at this waypoint. enemyTower changes this variable.
	public bool startSpawning;
	bool firstTime = true;
	Transform spawnPoint;
    EnemyPlayer enemyPlayer;
	public enemyTower tower; // Set Tower in enemySpawner. SetFire script using this to find which tower to set the fire.

	void Start () {

		maxSpawn = Random.Range (2, 10);
		spawnPoint = gameObject.transform.FindChild ("enemyPlayerSpawner");

	}
	

	void Update () {

		if (startSpawning && firstTime) {
			StartCoroutine (WaitTimer (spawnTimer));
			firstTime = false;
		}

	}

	void spawner () {
		if (!taken) {
			return;
		}

		GameObject spawned = Instantiate (spawnObject, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        if (tower.onFire)
        {
            enemyPlayer = spawned.GetComponent<EnemyPlayer>();
            enemyPlayer.onFire = true;
        }
		spawn++;

		StartCoroutine (WaitTimer(spawnTimer));
	}

	IEnumerator WaitTimer (float waitTime){
		if (spawn >= maxSpawn) {
			yield return null;
		} else {
			yield return new WaitForSeconds (waitTime);
			spawner ();
			spawnTimer = Random.Range(2,10);
		}
	}

	public void Reset (){
		spawn = 0;
		startSpawning = false;
		firstTime = false;
	}
}
