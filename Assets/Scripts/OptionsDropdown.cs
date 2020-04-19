using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsDropdown : MonoBehaviour {

	public GameObject dropDown;

	bool open = false;

	void Start() {
		dropDown.SetActive(open);
	}

	public void Pressed() {
		GameManager.selectionManager.UIAction();
		open = !open;
		dropDown.SetActive(open);
	}

	public void DeleteSelected() {
		GameManager.selectionManager.UIAction();
		GameManager.selectionManager.DeleteSelected();
	}

	public void OriginPlace() {
		MySceneManager.sceneManager.ChangeScene(MySceneManager.CHOOSINGPOS);
	}
}
