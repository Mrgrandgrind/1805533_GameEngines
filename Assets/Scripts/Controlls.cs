using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controlls : MonoBehaviour {

	public float tapForce = 10;
	public float tiltSmooth = 5;
	public Vector3 startPos;
	public GameObject Camera;
	public AudioSource Die;
	public AudioSource Point;
	public AudioSource Jump;

	Rigidbody2D RigidBody;
	Quaternion downRotation;
	Quaternion forwardRotation;

	GameLogic game;

	// Initialization
	void Start() {
		RigidBody = GetComponent<Rigidbody2D>();
		downRotation = Quaternion.Euler(0, 0, -90);
		forwardRotation = Quaternion.Euler(0, 0, 35);
		game = GameLogic.Instance;
		RigidBody.simulated = false;
		this.GetComponent<Animator> ().enabled = false;
	}
		

	// Update is called once per frame
	void Update() {
		if (game.GameOver)
			return;
		if (Input.GetMouseButtonDown(0)) {
			transform.rotation = forwardRotation;
			RigidBody.velocity = Vector3.zero;
			RigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
			Jump.Play ();
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "DeathCollision")
		{
			Die.Play();
			this.GetComponent<Animator> ().enabled = false;
			this.RigidBody.simulated = false;
			//register dead event
			Camera.GetComponent<GameLogic>().OnPlayerDies();
			// Play a sound
		}

		if (col.gameObject.tag == "ScoreCollision")
		{
			Point.Play();
			//register Score event
			Camera.GetComponent<GameLogic>().OnPlayerScored(); //Event sent to Gamelogic
			// Play a sound
		}
	}


	public void OnGameStarted(){
		RigidBody.velocity = Vector3.zero;
		RigidBody.simulated = true;
		this.GetComponent<Animator> ().enabled = true;
	}

	public void OnGameOverConfirmed(){
		transform.localPosition = startPos;
		transform.rotation = Quaternion.identity;
		// Add paralax Congigure function call
	}
}
