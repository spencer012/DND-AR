using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gameManager;

	public GameObject sessionOrigin;

	[HideInInspector]
	public PlaceOrigin placeOrigin;

	void Start() {
		if (gameManager == null) {
			gameManager = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(gameObject);
		}

		placeOrigin = sessionOrigin.GetComponent<PlaceOrigin>();
	}
	
	void Update() {

	}
}
