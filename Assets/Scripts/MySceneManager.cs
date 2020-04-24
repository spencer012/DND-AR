using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
	public static MySceneManager sceneManager;

	public static int START = 0, CHOOSINGSTART = 1, CHOOSINGPOS = 2, MAIN = 3, MAPMAKER = 4, MAPSELECTOR = 5;

	private static int currentScene = START;
	public static int CurrentScene { get => currentScene; }
	

	void Start() {
		if(sceneManager == null) {
			sceneManager = this;
			DontDestroyOnLoad(this);
		}
		else {
			Destroy(gameObject);
		}
	}

	public void ChangeScene(int scene) {
		StartCoroutine(ChangeSceneAsync(scene));
		currentScene = scene;
	}
	public IEnumerator ChangeSceneAsync(int scene) {
		bool mainUIon = false;

		if(scene == START) { //start menu
			//print("11111111111111111111111");
			yield return SceneManager.LoadSceneAsync(0);
			yield break;
		}
		else if(currentScene == START && scene > START) {
			//print("2222222222222222222222");
			yield return SceneManager.LoadSceneAsync(1);
			yield return null;
		}

		if(scene == CHOOSINGSTART) { //the beginning of placement, raycast to a plane, set height, and confirm
			//print("3333333333333333333333333");
			GameManager.gameManager.placeOrigin.enabled = true;
			GameManager.gameManager.placeOrigin.ChangeScene(CHOOSINGSTART);
		}
		else if(scene == CHOOSINGPOS) { //from options menu, can change rotation
			//print("44444444444444444444444444444");
			GameManager.gameManager.placeOrigin.enabled = true;
			GameManager.gameManager.placeOrigin.ChangeScene(CHOOSINGPOS);
		}
		else if(scene == MAIN) {
			//print("5555555555555555555555555555555");
			GameManager.gameManager.placeOrigin.enabled = false;
			GameManager.gameManager.PlaceBoard();
			mainUIon = true;
		}
		else if(scene == MAPMAKER) {

		}
		else if(scene == MAPSELECTOR) {
			GameManager.gameManager.mapSelectUI.SetActive(true);
		}
		else {
			throw new System.Exception("Scene " + scene + " does not exist in MySceneManager");
		}

		if (scene != START)
			GameManager.gameManager.mainUI.SetActive(mainUIon);
	}
}
