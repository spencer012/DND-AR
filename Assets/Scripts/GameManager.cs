using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gameManager;

	public GameObject sessionOrigin;
	public PlaceOrigin placeOrigin;

	void Start() {
		if (gameManager == null) {
			gameManager = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(gameObject);
		}
	}
	
	void Update() {

	}
}
