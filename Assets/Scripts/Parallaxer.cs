using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour {

	class ParallaxObject{
		public Transform transform;
		public bool inUse;
		public ParallaxObject(Transform t) {transform = t;}
		public void Use() {inUse = true;}
		public void Dispose() {inUse = false;}
	}

	[System.Serializable]
	public struct YHeightRange{
		public float min;
		public float max;
	}

	public GameObject BackgroundPrefab; // background sprite
	public int prefabTotal; // total number of cloned sprites
	public float shiftSpeed; // movement speed
	public float spawnRate; // time delay between spawns in seconds

	public YHeightRange Y_HeightRange; // struct for max and mix spawn position
	public Vector3 defaultSpawnPos; // spawn position

	float spawnTimer;
	float targetAspect;

	ParallaxObject[] parallaxObjects; // array of total parralax objects

	GameLogic game;

	void Awake(){
		// initialize variables
		Configure();
	}

	void Start(){
		game = GameLogic.Instance; // instance of gamelogic
	}


	void Update(){
		if (game.GameOver) // if gamelogic is on game ober
			return;

		Shift (); // move sprited
		spawnTimer += Time.deltaTime; // add time to total spawn timer

		// spawn new sprite if spawn timer is larger than spawn rate
		if (spawnTimer > spawnRate) {
			Spawn();
			spawnTimer = 0;
		}
	}


	// configure specified total background sprites
	void Configure(){
		parallaxObjects = new ParallaxObject[prefabTotal];
		for (int i = 0; i < parallaxObjects.Length; i++) {
			GameObject go = Instantiate(BackgroundPrefab as GameObject); // create instance
			Transform t = go.transform; 
			t.SetParent (transform);
			t.position = Vector3.one * 1000;
			parallaxObjects[i] = new ParallaxObject(t);
		}
	}


	// moves parallax objects into place
	void Spawn(){
		Transform t = GetParallaxObject ();
		if (t == null) // if true, this indicates that poolSize is too small
			return;
		Vector3 pos = Vector3.zero;
		pos.x = defaultSpawnPos.x; // set x as original position
		pos.y = Random.Range (Y_HeightRange.min, Y_HeightRange.max); // set y to random position between min and max
		t.position = pos; 
	}


	// loop through parallax objects
	// transform object position
	// dispose when they go off screen
	void Shift(){
		for (int i = 0; i < parallaxObjects.Length; i++) {
			parallaxObjects [i].transform.position += -Vector3.right * shiftSpeed * Time.deltaTime;
			CheckDisposeObject (parallaxObjects [i]);
		}
	}

	// places parallax objects off screen
	void CheckDisposeObject(ParallaxObject poolObject){
		if (poolObject.transform.position.x < -defaultSpawnPos.x) {
			poolObject.Dispose ();
			poolObject.transform.position = Vector3.one * 1000;
		}
	}

	// get first parralax object available
	Transform GetParallaxObject(){
		for (int i = 0; i < parallaxObjects.Length; i++) {
			if (!parallaxObjects [i].inUse) {
				parallaxObjects [i].Use ();
				return parallaxObjects[i].transform;
			}
		}
		return null;
	}

	// Dispose of all parallax objects
	public void OnGameOverParallaxer(){
		for (int i = 0; i < parallaxObjects.Length; i++) {
			parallaxObjects [i].Dispose ();
			parallaxObjects [i].transform.position = Vector3.one * 1000;
		}
	}
}
