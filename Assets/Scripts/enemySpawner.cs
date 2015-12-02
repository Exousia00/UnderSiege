using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemySpawner : MonoBehaviour {

	public float spawnTimer;
	public GameObject spawnObject;
	public int spawn = 0;
	public int maxSpawn;
	public float[] spawnTimers;
	GameObject[] waypoints;
	waypointSystem wps;
	GameObject currentWaypoint;
	GameObject spawned;
	void Start () {
		waypoints = GameObject.FindGameObjectsWithTag ("waypoint");
		StartCoroutine (WaitTimer(spawnTimer));
		spawnTimer = spawnTimers[spawn];
	}

	void spawner () {
		spawned = Instantiate (spawnObject, transform.position, transform.rotation) as GameObject;
		spawned.GetComponent<enemyTower>().currentWaypoint = currentWaypoint;
		wps.tower = spawned.GetComponent<enemyTower>();
		spawn++;
		spawnTimer = spawnTimers[spawn];
		StartCoroutine (WaitTimer(spawnTimer));
	}

	// Waypoint picker. Finds all free waypoint then randomly selects one to send the tower to, then sets that waypoint as taken.
	void WaypointPicker (GameObject[] go){
		List<GameObject> freeWaypoints = new List<GameObject>();

		for (int i = 0; i < go.Length; i++){
			wps = go[i].GetComponent<waypointSystem>();
			if (wps.taken == false){
				freeWaypoints.Add (go[i]);
			}
		}
		
		if (freeWaypoints.Count > 0) {
			currentWaypoint = freeWaypoints [Random.Range (0, freeWaypoints.Count)];
			wps = currentWaypoint.GetComponent<waypointSystem>();
			wps.taken = true;

		} else {
			currentWaypoint = null;
		}
	}

	IEnumerator WaitTimer (float waitTime){
		if (spawn >= maxSpawn) {
			yield return null;
		} else {
			yield return new WaitForSeconds (waitTime);
			WaypointPicker(waypoints);
			if (currentWaypoint != null){
				spawner();
			}
		

		}
	}
}
