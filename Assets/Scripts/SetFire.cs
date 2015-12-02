using UnityEngine;
using System.Collections;

public class SetFire : MonoBehaviour {

	Game game;
	waypointSystem wps;
	bool playerInside = false;
	Animation playerAnim;
	float countDown = 0.0f;
	void Start () {
		game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
		wps = gameObject.GetComponentInParent<waypointSystem>();
		playerAnim = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animation> ();
	}

	void Update () {

		if (game.setFirePopUp.activeSelf && Input.GetButton ("F") && playerInside) {
			if (wps.tower.onFire){
				return;
			}
			countDown += Time.deltaTime;
			if (!playerAnim.IsPlaying("StartFire")){
				playerAnim.Play ("StartFire");
			}
			if (countDown > 1.5f) {
				wps.tower.onFire = true;
			}
		} else
			countDown = 0;
	
	}

	void OnTriggerStay (Collider other){
		CheckList (true, other);
		playerInside = true;
	}
	void OnTriggerExit (Collider other){
		CheckList (false, other);
		playerInside = false;
	}

	void CheckList (bool active, Collider other) {
		if (wps.taken && wps.startSpawning) {
			if (other.GetComponent<Player>()) {
				game.SetFirePopUp (active);
			}
		} else
			game.SetFirePopUp (false);
	}

}
