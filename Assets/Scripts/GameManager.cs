using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gameManager;

	public GameObject sessionOrigin;

	[HideInInspector]
	public PlaceOrigin placeOrigin;

	public GameObject boardPrefab, board;

	void Start() {
		if (gameManager == null) {
			gameManager = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(gameObject);
		}

		placeOrigin = sessionOrigin.GetComponent<PlaceOrigin>();

		//Screen.orientation = ScreenOrientation.Landscape;
	}
	
	public void PlaceBoard() {
		board = Instantiate(boardPrefab, placeOrigin.startOriginPose.position, placeOrigin.startOriginPose.rotation);
		board.transform.SetParent(PlaceOrigin.anchor.transform);
	}
}
