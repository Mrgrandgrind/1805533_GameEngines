using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public static GameLogic Instance;

	public GameObject HighScore;
	public GameObject[] Parallax;
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
		HighScore.GetComponent<ScoreText> ().UpdateScore ();
    }

	public void OnCountdownFinished() {
		SetPageState(PageState.None);
		bird.GetComponent<Controlls>().OnGameStarted();
		score = 0;
		gameOver = false;
	}

	public void OnPlayerDies() {;
		gameOver = true;
		this.GetComponent<AudioSource> ().Stop();
		int savedScore = PlayerPrefs.GetInt("HighScore");
		if (score > savedScore) {
			Debug.LogError (savedScore);
			PlayerPrefs.SetInt("HighScore", score);
		}
		HighScore.GetComponent<ScoreText> ().UpdateScore ();
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

	void UpdatePages(bool Start, bool GameOver, bool Countdown){
		startPage.SetActive(Start);
		gameOverPage.SetActive(GameOver);
		countdownPage.SetActive(Countdown);
	}

    public void ConfirmGameOver()
        //activated when replay buttonm is hit
    {
		bird.GetComponent<Controlls>().OnGameOverConfirmed(); // Event sent to Controlls

		for (int i = 0; i < Parallax.Length; i++) {
			Parallax[i].GetComponent<Parallaxer>().OnGameOverParallaxer();
		}
			
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    //activated when play buttonm is hit
    {
		SetPageState(PageState.Countdown);
		Counter.GetComponent<CountdownData>().StartCountdown ();
		this.GetComponent<AudioSource> ().Play();
    }
}
