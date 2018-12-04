using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

	Text Highscore;

	// displays high score text
	void Start(){
		Highscore = GetComponent<Text> ();
		Highscore.text = "High Score: " + PlayerPrefs.GetInt ("HighScore").ToString();
	}
}
