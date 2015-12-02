using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyTower : Actors {



	public GameObject currentWaypoint;
	public float speed;
	waypointSystem wps;
	Quaternion endRot;
	public float health = 100;
	public bool onFire = false;
	bool firstTime = true;
	ParticleSystem pSystem;
	Game game;
	bool neverDiedBefore = true;
	void Start () {
		endRot = transform.rotation;
		pSystem = gameObject.GetComponentInChildren<ParticleSystem> ();
		wps = currentWaypoint.GetComponent<waypointSystem>();
		game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
	}

	void Update () {

		if (onFire && firstTime) {
			StartCoroutine( Tick (1.0f));
			pSystem.emissionRate = 40;
            firstTime = false;
		}
		if (currentWaypoint.transform.position != transform.position) {
			Movement (currentWaypoint);
		} else {
			wps.startSpawning = true;
			transform.rotation = Quaternion.Slerp (transform.rotation, endRot, Time.deltaTime * 10);
		}
		if (health <= 0 && neverDiedBefore) {
			Death();
		}
	}
	

	// Move to the specified target.
	void Movement(GameObject target){
		if (target != null) {
			float moveSpeed = speed * Time.deltaTime;
			Quaternion newRot = Quaternion.LookRotation (target.transform.position - transform.position, Vector3.forward);
			newRot.x = 0.0f;
			newRot.z = 0.0f;

			transform.rotation = Quaternion.Slerp (transform.rotation, newRot, Time.deltaTime * 10);
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, moveSpeed);

		}
	}

    IEnumerator Tick(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        health = TakeDamage(health, 10);
        StartCoroutine(Tick(waitTime));
    }
	
	// Destroys tower and sets the waypoint as free.
	void Death (){
		neverDiedBefore = false;
		wps.taken = false;
		game.towersDied++;
		wps.Reset ();
		Destroy (gameObject, 2);
	}
	
}
