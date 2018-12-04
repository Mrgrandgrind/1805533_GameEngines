using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))] //creates text component if missing on GameObject
public class CountdownData : MonoBehaviour
{
	public GameObject Camera;
	Text countdown;

	// Initialises and starts countdown
	public void StartCountdown()
	{
		countdown = GetComponent<Text>();
		countdown.text = "3";
		StartCoroutine("CountdownSequence");
	}


	// IEnumarator to allow for countdown delay
	IEnumerator CountdownSequence()
	{
		// Countdown Sequence
		int count = 3;
		for (int i = 0; i < count; i++)
		{
			countdown.text = (count - i).ToString();
			yield return new WaitForSeconds(1);
		}
		Camera.GetComponent<GameLogic>().OnCountdownFinished(); // Tells GameLogic countdown is finished
	}
}