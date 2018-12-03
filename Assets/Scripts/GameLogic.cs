using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public static GameLogic Instance;

	public GameObject HighScore;
	public GameObject Parallax;
	public GameObject bird;
	public GameObject Counter;
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;


    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    int score = 0;
    bool gameOver = true;
    public bool GameOver { get { return gameOver; } }
	public int Score { get { return score; } }

    void Awake() {
        Instance = this;
		SetPageState(PageState.Start);
    }

	public void OnCountdownFinished() {
		SetPageState(PageState.None);
		bird.GetComponent<Controlls>().OnGameStarted();
		score = 0;
		gameOver = false;
	}

	public void OnPlayerDies() {
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt("HighScore");
		if (score > savedScore) {
			Debug.LogError (savedScore);
			PlayerPrefs.SetInt("HighScore", score);
			HighScore.GetComponent<ScoreText> ().UpdateScore ();
		}
		SetPageState(PageState.GameOver);
	}

	public void OnPlayerScored(){
		Debug.LogError ("Scored!");
		score++;
		scoreText.text = score.ToString ();
	}

    void SetPageState(PageState state) {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;

            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;

            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;

            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver()
        //activated when replay buttonm is hit
    {
		bird.GetComponent<Controlls>().OnGameOverConfirmed(); // Event sent to Controlls
		Parallax.GetComponent<Parallaxer>().OnGameOverParallaxer();
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    //activated when play buttonm is hit
    {
		SetPageState(PageState.Countdown);
		Counter.GetComponent<CountdownData>().StartCountdown ();
    }
}
