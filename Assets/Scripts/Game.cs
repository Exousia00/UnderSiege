using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	GameObject pauseMenu;
	GameObject startMenu;
	GameObject gameOverMenu;
	GameObject victoryMenu;
	public GameObject setFirePopUp;
	public int count = 0; 
	public Slider progressBar;
	public bool gameover= false;
	public Text healthText;
	public Text timerText;
	public int timeLimit;
	public int towersDied = 0;
	public int nextLevel;
	enemySpawner spawner;

	void Start () {
		healthText = GameObject.FindGameObjectWithTag ("PlayerHealthUI").GetComponent<Text>();
		timerText = GameObject.FindGameObjectWithTag ("TimerUI").GetComponent<Text>();
		progressBar = GameObject.FindGameObjectWithTag ("ProgressBar").GetComponent<Slider>();
		pauseMenu = GameObject.FindGameObjectWithTag ("PauseMenu");
		startMenu = GameObject.FindGameObjectWithTag ("startMenu");
		gameOverMenu = GameObject.FindGameObjectWithTag ("GameOverMenu");
		victoryMenu = GameObject.FindGameObjectWithTag ("VictoryMenu");
		setFirePopUp = GameObject.FindGameObjectWithTag ("SetFirePopUp");
		spawner = GameObject.FindGameObjectWithTag ("TowerSpawner").GetComponent<enemySpawner>();
		pauseMenu.SetActive (false);
		gameOverMenu.SetActive (false);
		setFirePopUp.SetActive (false);
		victoryMenu.SetActive (false);
		StartCoroutine (Timer());
		Time.timeScale = 0;
	}

	// Update is called once per frame
	void Update () {
		DetectEnemies ();
		ProgressBar ();
		ButtonInput ();
		GameOverMenu ();
		VictoryMenu();
	}

	void ButtonInput () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PauseMenu();
		}
	}

	public void DetectEnemies(){
		GameObject[] enemyPlayer;
		List<GameObject> enemy = new List<GameObject>();
		
		enemyPlayer = GameObject.FindGameObjectsWithTag ("enemy");
		
		for (int i = 0; i < enemyPlayer.Length; i++){
			
			enemy.Add (enemyPlayer[i]);
			
		}
		count = enemy.Count;
	}

	public void ProgressBar (){

		progressBar.value = count;

		if (progressBar.maxValue <= progressBar.value) {
			gameover = true;
		}

	}

	public void PauseMenu (){
		if (pauseMenu.activeSelf) {
			pauseMenu.SetActive (false);
			Time.timeScale = 1.0f;
		} else {
			pauseMenu.SetActive (true);
			Time.timeScale = 0.0f;
		}
	}

	public void GameOverMenu (){
		if (gameover) {
			gameOverMenu.SetActive(true);
			Time.timeScale = 0.0f;
		}
	}
	public void VictoryMenu (){
		timerText.text = "Time Left: " + timeLimit;
		if (timeLimit <= 0 || spawner.maxSpawn == towersDied) {
			victoryMenu.SetActive (true);
			Time.timeScale = 0.0f;
		}
	}

	IEnumerator Timer(){
		yield return new WaitForSeconds (1.0f); // wait half a second
		timeLimit--;
		StartCoroutine (Timer ());
	}

	public void ResumeButton () {
		pauseMenu.SetActive(false);
		Time.timeScale = 1.0f;
	}

	public void PlayButton () {

		startMenu.SetActive(false);
		Time.timeScale = 1.0f;
	}

	public void RestartButton () {
		Time.timeScale = 1.0f;
		Application.LoadLevel (Application.loadedLevelName);
	}

	public void SetFirePopUp ( bool active) {
		setFirePopUp.SetActive (active);
	}

	public void MainMenuButton () {
		Time.timeScale = 1.0f;
		Application.LoadLevel (0);
	}

	public void QuitButton () {
		Application.Quit();
	}

	public void VictoryButton () {
		Application.LoadLevel (nextLevel);
	}

}
