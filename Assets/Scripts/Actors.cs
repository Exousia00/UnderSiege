using UnityEngine;
using System.Collections;

public class Actors : MonoBehaviour {

	enemyTower eTower;
	EnemyPlayer ePlayer;
	Player cPlayer;
	float h;

	public void PlayerAttack (GameObject go, int damage, GameObject player){
	
		if (go.GetComponent<enemyTower>()) {
			eTower = go.GetComponent<enemyTower> ();
			h = eTower.health;
			eTower.health = TakeDamage (h,10);

		} else if (go.GetComponent<EnemyPlayer>() && go.GetComponent<EnemyPlayer>().neverDoneDeath) {
			ePlayer = go.GetComponent<EnemyPlayer> ();
			h = ePlayer.health;
			ePlayer.health = TakeDamage (h,damage);
			ePlayer.GetComponent<Animation>().Stop();
			ePlayer.GetComponent<Animation>().Play("Damage");
			Vector3 dirc = ePlayer.transform.position - player.transform.position;
			dirc.y = 0;
			dirc.Normalize();
			ePlayer.GetComponent<Rigidbody>().AddForce(dirc * 250);
		}

	}

	public void EnemyAttack (GameObject go){

		cPlayer = go.GetComponent<Player>();
		h = cPlayer.health;
		cPlayer.health = TakeDamage(h,5);
	}

    // Burning damage Tick.

	public float TakeDamage ( float h, int d){
		
		h -= d;
		return h;
	}
}











