using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	GameObject player;
	Vector3 newPos;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 newPos = new Vector3 (player.transform.position.x, transform.position.y, transform.position.z);
		if (Vector3.Distance (newPos, new Vector3 (0,transform.position.y, transform.position.z)) < 5) {
			transform.position = Vector3.Lerp (transform.position, newPos, Time.deltaTime * 10);
		}
	}
}
