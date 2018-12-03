using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

	Text score;

	public void UpdateScore(){
		Debug.Log ("updateScore");
		score = GetComponent<Text> ();
		score.text = "High Score: " + PlayerPrefs.GetInt ("HighScore").ToString();
	}
}
