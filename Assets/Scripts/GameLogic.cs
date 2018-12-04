using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public static GameLogic Instance;

	public GameObject HighScore; // Highscore game object
	public GameObject[] Parallax; // Parallax script array
	public GameObject bird; // Bird character (contains gamelogic script)
	public GameObject Counter;
	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countdownPage;
	public Text scoreText;

	// Enumerated variable for all page types
	enum PageState
	{
		None,
		Start,
		GameOver,
		Countdown
	}

	int score = 0; // Current score
	bool gameOver = true; // GameOver variable
	public bool GameOver {
		get {
			return gameOver; 
		} 
	}
	public int Score {
		get { 
			return score; 
		} 
	}

	// initialise
	void Awake() {
		Instance = this;
		SetPageState(PageState.Start);
		HighScore.GetComponent<ScoreText> ().UpdateScore ();
	}
		

	// Function for determining the current page state
	void SetPageState(PageState state) {
		switch (state)
		{
		case PageState.None:
			UpdatePages (false, false, false);
			break;

		case PageState.Start:
			UpdatePages (true, false, false);
			break;

		case PageState.GameOver:
			UpdatePages (false, true, false);
			break;

		case PageState.Countdown:
			UpdatePages (false, false, true);
			break;
		}
	}

	// Sets current page to active, de-activating the remaining
	void UpdatePages(bool Start, bool GameOver, bool Countdown){
		startPage.SetActive(Start);
		gameOverPage.SetActive(GameOver);
		countdownPage.SetActive(Countdown);
	}

	//activated when replay buttonm is hit
	public void ConfirmGameOver()
	{
		bird.GetComponent<Controlls>().OnGameOverConfirmed(); // Event sent to Controlls

		// Resets all parallax layers
		for (int i = 0; i < Parallax.Length; i++) {
			Parallax[i].GetComponent<Parallaxer>().OnGameOverParallaxer();
		}

		scoreText.text = "0"; // updates score text
		SetPageState(PageState.Start); // Sets page to Start
	}

	//activated when play buttonm is hit
	public void StartGame()
	{
		// Starts countdown Sequence
		SetPageState(PageState.Countdown);
		Counter.GetComponent<CountdownData>().StartCountdown ();
		this.GetComponent<AudioSource> ().Play();
	}

	// Resets score and gamestate
	// Tells bird to start playing
	public void OnCountdownFinished() {
		SetPageState(PageState.None);
		bird.GetComponent<Controlls>().OnGameStarted();
		score = 0;
		gameOver = false;
	}

	// Checks if the final score is obove high score, updates if it is
	public void OnPlayerDies() {;
		gameOver = true;
		this.GetComponent<AudioSource> ().Stop(); // death Audio
		int savedScore = PlayerPrefs.GetInt("HighScore"); // Accesses playerPrefs for current HighScore

		// If current score is higher than Highschore, Higscore is updated
		if (score > savedScore) {
			PlayerPrefs.SetInt("HighScore", score);
		}

		HighScore.GetComponent<ScoreText> ().UpdateScore (); //Update score text
		SetPageState(PageState.GameOver); // set page to GameOver
	}

	// Update and display score
	public void OnPlayerScored(){
		score++;
		scoreText.text = score.ToString ();
	}
}
