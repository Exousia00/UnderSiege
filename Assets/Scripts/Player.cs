using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : Actors {
	public List<GameObject> enemyList1 = new List<GameObject>();
	public List<GameObject> enemyList2 = new List<GameObject>();
	public float health = 100;
	public GameObject skillOne;
	Vector3 moveDirection = Vector3.zero;
	Vector3 moveTo = new Vector3();
	Vector3 relativePos;
	CharacterController cc;
	bool hitEnemy = false;
	GameObject enemy;
	Game gc;
	bool isRunning = false;
	Skills skills;

	void Start () {

		moveTo = transform.position;
		cc = gameObject.GetComponent<CharacterController>();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Game>();
		skills = skillOne.GetComponent<Skills>();
	}

	void Update () {

		if (Vector3.Distance (transform.position, moveTo) < 1.9f && !GetComponent<Animation> ().IsPlaying ("StartFire") && !GetComponent<Animation> ().IsPlaying ("attack")&& !GetComponent<Animation> ().IsPlaying ("idle")) 
        {
			GetComponent<Animation> ().CrossFade ("idle");
		}

        CheckEnemy();
		HealthCheck ();
		KeyInput ();
		Move ();
	}
    void CheckEnemy(){
        if (enemy != null)
        {
			if (enemy.GetComponent<enemyTower>()){
				return;
			}
            else if (!enemy.GetComponent<EnemyPlayer>().neverDoneDeath)
            {
                hitEnemy = false;
                enemy = null;
                moveTo = transform.position;
            }
        }
    }
	void HealthCheck ()
    {
		gc.healthText.text = "Health: " + health;
		if (health <= 0) 
        {
			Death();
			return;
		}
	}

	void KeyInput(){
		if (Input.GetMouseButton (0)) 
        {
			locatePos ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha1) && !skills.skillIsRunning1) 
        {
			StopCoroutine (AttackTimer(1.0f,40));
            skills.SkillDamage(enemyList1, 1);
		}

		if (Input.GetKeyDown (KeyCode.Alpha2) && !skills.skillIsRunning2) 
        {
			StopCoroutine (AttackTimer(1.0f,40));
			skills.SkillDamage (enemyList2, 2);
		} 

		if (enemy != null) 
        {
			AutoAttack ();
		}


	}

	void AutoAttack ()
    {
		if (Vector3.Distance(transform.position, enemy.transform.position) < 2.1f && hitEnemy && !isRunning && !GetComponent<Animation>().IsPlaying("attack"))
        {
			GetComponent<Animation>().Play("attack");
			StartCoroutine (AttackTimer (1.0f, 40));
		}
	}

	IEnumerator AttackTimer (float waitTime, int damage)
    {
		isRunning = true;
		PlayerAttack (enemy,damage, gameObject);
		yield return new WaitForSeconds (waitTime);
		isRunning = false;
	}

	void Death ()
    {
		gc.gameover = true;
		Destroy (gameObject, 1);
	}

	void locatePos () 
    {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 1000))
		{
			if (hit.transform.tag == "wall")
            {
				moveTo = hit.point;
				relativePos = moveTo - transform.position;
				hitEnemy = false;
			}
			else if (hit.transform.GetComponent<EnemyPlayer>() || hit.transform.GetComponent<enemyTower>())
            {
				moveTo = hit.point;
				relativePos = moveTo - transform.position;
				hitEnemy = true;
				enemy = hit.transform.gameObject;
			}
			
		}

	}

	void Movement () 
    {
		float moveSpeed = 5 * Time.deltaTime;

		if (cc.isGrounded) 
        {
			if (!GetComponent<Animation> ().IsPlaying ("attack"))
            {
				GetComponent<Animation> ().CrossFade ("run", 0.05f);
			}
			moveDirection = (moveTo - transform.position);
			moveDirection = moveDirection.normalized;
		}
		moveDirection.y -= 20 * Time.deltaTime;
		cc.Move (moveDirection * moveSpeed);
	}

	void Move (){
		Vector3 distance = (moveTo - transform.position);		
		RotatePlayer ();

		if (distance.magnitude > 2.0f && hitEnemy) 
        { Movement(); }
		else if (distance.magnitude > 0.2f && !hitEnemy) 
        { Movement(); }
	}

	void RotatePlayer ()
    {
		Quaternion desiredRot = Quaternion.LookRotation (relativePos);
		if (hitEnemy && enemy != null) 
        {
			desiredRot = Quaternion.LookRotation (enemy.transform.position - transform.position, Vector3.forward);
		}
		desiredRot.x = 0.0f;
		desiredRot.z = 0.0f;
		if (desiredRot.y != transform.rotation.y)
        {
			transform.rotation = Quaternion.Slerp (transform.rotation, desiredRot, Time.deltaTime * 40);
		}
	}

}

/*


	* Need to make icons.
	* Need to make a win condition when all enemys are kill and no more will come.
	* Different play modes. Surviver mode each enemy is worth 10 points will unlimited towers and spawner.

*/

	








