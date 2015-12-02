using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Skills : MonoBehaviour {

	GameObject[] skillEffects;
	GameObject skillEffects2;
	ParticleSystem skillParticles;
	Player player;
	public bool skillIsRunning1 = false;
	public bool skillIsRunning2 = false;
	public bool skillOne;
	public GameObject skillOneUI;
	public GameObject skillTwoUI;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		skillEffects = GameObject.FindGameObjectsWithTag ("SkillEffect");
		skillEffects2 = GameObject.FindGameObjectWithTag ("SkillEffect2");
	}

	void Update () {
		if (skillOne) {
			SkillsUI ();
		}
	}

	void OnTriggerEnter (Collider other){
		if (other.GetComponent<EnemyPlayer>()) {
			if (skillOne){
				player.enemyList1.Add (other.gameObject);
			}else player.enemyList2.Add (other.gameObject);
		}
	}
	void OnTriggerExit (Collider other){
		if (other.GetComponent<EnemyPlayer>()) {
			if (skillOne){
				player.enemyList1.Remove (other.gameObject);
			}else player.enemyList2.Remove (other.gameObject);
		}
	}

	public void SkillDamage (List<GameObject> list, int skill){

		RemoveFromList(list);
		if (list.Count == 0) {
			return;
		} else {
			StartCoroutine(SkillTimer (2.0f, list, skill));
		}
	}

	IEnumerator SkillTimer (float waitTime, List<GameObject> enemyList, int skill){
	
		if (skill == 1) {
			skillIsRunning1 = true;
			SkillAttack (enemyList, skill);
			yield return new WaitForSeconds (waitTime);
			skillIsRunning1 = false;
		}
		if (skill == 2) {
			skillIsRunning2 = true;
			SkillAttack (enemyList, skill);
			yield return new WaitForSeconds (waitTime);
			skillIsRunning2 = false;
		}
	}

	public void SkillsUI(){
		SkillUI (skillIsRunning1, skillOneUI);
		SkillUI (skillIsRunning2, skillTwoUI);
	}

	public void SkillUI(bool isRunning, GameObject skillUI){
		int i;
		if (isRunning) {
			i = 1;
		} else
			i = 0;
		skillUI.GetComponent<Slider>().value = i;
	}

	void SkillAttack(List<GameObject> enemyList, int skill){
		player.GetComponent<Animation>().Play ("attack");
		if (skill == 1) {
			foreach (GameObject go in skillEffects) {
				go.GetComponent<ParticleSystem> ().emissionRate = 8;;
			}
		} else
			skillEffects2.GetComponent<ParticleSystem> ().emissionRate = 50;
		
		foreach (GameObject go in enemyList) {
			player.PlayerAttack (go, 40, gameObject);
		}
		StartCoroutine (EffectTimer (1, skill));

	}

	IEnumerator EffectTimer (float timer, int skill){
		yield return new WaitForSeconds (timer);
		if (skill == 1) {
			foreach (GameObject go in skillEffects) {
				go.GetComponent<ParticleSystem> ().emissionRate = 0;
			}
		} else
			skillEffects2.GetComponent<ParticleSystem> ().emissionRate = 0;
	}

	void RemoveFromList (List<GameObject> list){

		if (!skillIsRunning1 || !skillIsRunning2) {
			for(int i = list.Count-1; i >=0; i-- ) {
				if(list[i] == null) {
					list.RemoveAt(i);
				}
			}
		}
	}
}
