using UnityEngine;
using System.Collections;

public class EnemyPlayer : Actors {

	public float health  = 100;
	public float speed;
	GameObject player;
	bool isRunning = false;
	public bool neverDoneDeath = true;
    bool neverDoneFire = true;
    public bool onFire = false;
    ParticleSystem pSystem;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
        pSystem = gameObject.GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (health <= 0) {
            if (neverDoneDeath)
            {
                Death();
            }
			return;
		}
        if (onFire && neverDoneFire)
        {
            neverDoneFire = false;
            pSystem.emissionRate = 10;
            StartCoroutine(Tick(1.0f));
        }
		RotEnemy ();
		if (!GetComponent<Animation> ().IsPlaying ("Damage")) {
			MoveOrAttack ();
		} else 
			StopCoroutine ("AttackTimer");
	}

	IEnumerator AttackTimer (float waitTime){
		isRunning = true;
		GetComponent<Animation>().Play("Attack");
		EnemyAttack(player);
		yield return new WaitForSeconds (waitTime);
		isRunning = false;
	}

	void MoveOrAttack (){
		if (Vector3.Distance (transform.position, player.transform.position) > 2 && !GetComponent<Animation>().IsPlaying ("Attack")) {
			StopCoroutine("AttackTimer");
			GetComponent<Animation>().Play("Walk");
			Movement ();


		} 
		else if (!isRunning && Vector3.Distance (transform.position, player.transform.position) < 2 ) {

			StartCoroutine (AttackTimer (2.8f));
			
		}
	}

	void Movement(){
		if (player != null) {
			float moveSpeed = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, player.transform.position, moveSpeed);
			
		}
	}

	void RotEnemy (){
		if (player != null) {
			Quaternion newRot = Quaternion.LookRotation (player.transform.position - transform.position, Vector3.forward);
			newRot.z = newRot.x = 0.0f;
			transform.rotation = Quaternion.Slerp (transform.rotation, newRot, Time.deltaTime * 10);
		}
	}

    public IEnumerator Tick(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        health = TakeDamage(health, 10);
        StartCoroutine(Tick(waitTime));
    }

	void Death ()
    {
		neverDoneDeath = false;
		GetComponent<Animation> ().Play ("Death");
		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
		Destroy (gameObject, 4);
	}

}
