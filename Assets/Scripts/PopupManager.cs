using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour {

	public GameObject scan, found;

	public static PopupManager popupManager;

	public const int SCAN = 0, FOUND = 1;

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
}
