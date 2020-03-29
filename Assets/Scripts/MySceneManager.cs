using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
	public static MySceneManager sceneManager;

	public static int TRANSITION = -1, START = 0, CHOOSINGSTART = 1, CHOOSING = 2, MAIN = 3, MAPMAKER = 4;

	private static int currentScene = START;

	public static int CurrentScene { get => currentScene;}

	void Start() {
		if(sceneManager == null) {
			sceneManager = this;
			DontDestroyOnLoad(this);
		}
		else {
			Destroy(gameObject);
		}
	}
	private void SceneTrans() {
		ChangeScene(currentScene);
	}
	public void ChangeScene(int scene) {
		if(scene == START) { //start menu
			SceneManager.LoadScene(0);
			return;
		}
		else if(currentScene == START && scene > START && scene <= MAPMAKER) {
			SceneManager.LoadScene(1);
			currentScene = TRANSITION;
			Invoke("SceneTrans", 0.01f);
			return;
		}
		else if(currentScene == TRANSITION && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("main")) {
			currentScene = CHOOSINGSTART;
			Invoke("SceneTrans", 0.01f);
			return;
		}
		else if(currentScene == TRANSITION) {
			return;
		}

		if (scene == CHOOSINGSTART) { //the beginning of placement, raycast to a plane, set height, and confirm
			GameManager.gameManager.placeOrigin.ChangeScene(CHOOSINGSTART);
		}
		else if(scene == CHOOSING) { //from options menu, can change rotation, height, and has a button to go back to choosing position
			GameManager.gameManager.placeOrigin.ChangeScene(CHOOSING);
		}
		else if(scene == MAIN) {
			GameManager.gameManager.PlaceBoard();
		}
		else if(scene == MAPMAKER) {

		}
		else {
			throw new System.Exception("Scene " + scene + " does not exist in MySceneManager");
		}
	}
}
