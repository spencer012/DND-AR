using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gameManager;

	public GameObject sessionOrigin;

	//[HideInInspector]
	public PlaceOrigin placeOrigin;

	public GameObject boardPrefab, board;

	public GameObject mainUI, mapSelectUI;

	public static ShelfManager shelfManager;
	public static SelectionManager selectionManager;
	
	public TileMap currentMap;

	void Start() {
		if (gameManager == null) {
			gameManager = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(gameObject);
		}

		placeOrigin = sessionOrigin.GetComponent<PlaceOrigin>();

		//Screen.orientation = ScreenOrientation.Landscape;

		//PlaceBoard();
	}
	
	public void PlaceBoard() {
		if (board == null)
			board = Instantiate(boardPrefab, placeOrigin.startOriginPose.position, placeOrigin.startOriginPose.rotation, PlaceOrigin.anchor.transform);
		//board = Instantiate(boardPrefab);
		board.GetComponent<BoardManager>().GenerateBoard(currentMap);
	}
}
