using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void StartGameButton(){
		Application.LoadLevel (1);
	}

	public void QuitGame (){
		Application.Quit ();
	}
}
