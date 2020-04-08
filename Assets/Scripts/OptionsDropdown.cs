using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsDropdown : MonoBehaviour {

	public GameObject dropDown;

	bool open = false;

	void Start() {
		dropDown.SetActive(open);
	}

	void Update() {

	}

	public void Pressed() {
		open = !open;
		dropDown.SetActive(open);
	}
}
