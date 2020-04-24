using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour {

	public GameObject scan, found,
		posChange, mapChange;

	public static PopupManager popupManager;

	public const int SCAN = 0, FOUND = 1, 
		POSCHANGE = 0, MAPCHANGE = 1;

	void Start() {
		popupManager = this;

		scan.SetActive(false);
		found.SetActive(false);
	}

	public void DisplayPopup(int type, bool show) {
		//print(type + " " + show);
		switch (type) {
			case SCAN:
				scan.SetActive(show);
				break;
			case FOUND:
				found.SetActive(show);
				break;
			default:
				throw new System.Exception("Popup " + type + " does not exist");
		}
	}

	public void Confirm(int type) {
		switch(type) {
			case POSCHANGE:
				posChange.SetActive(true);
				mapChange.SetActive(false);
				break;
			case MAPCHANGE:
				mapChange.SetActive(true);
				posChange.SetActive(false);
				break;
		}
	}

	public void Yes(int type) {
		No();
		switch(type) {
			case POSCHANGE:
				MySceneManager.sceneManager.ChangeScene(MySceneManager.CHOOSINGPOS);
				break;
			case MAPCHANGE:
				MySceneManager.sceneManager.ChangeScene(MySceneManager.MAPSELECTOR);
				break;
		}
	}

	public void No() {
		GameManager.selectionManager.UIAction();
		mapChange.SetActive(false);
		posChange.SetActive(false);
	}
}
