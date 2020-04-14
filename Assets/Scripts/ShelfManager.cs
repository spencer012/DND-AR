using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour {


	bool upState = false;

	private float upPos = 436f;

	RectTransform rectTransform;

	public float speed = 10;

	void Start() {
		rectTransform = GetComponent<RectTransform>();
	}

	void Update() {
		float x = rectTransform.position.x;
		//print(x);
		if(upState && x > -upPos) {
			float diff = upPos + x + (speed * 4);
			rectTransform.position -= new Vector3((((diff * (diff / 2)) / (upPos / 2))) * Time.deltaTime * speed, 0, 0);
		}
		else if(upState && x < -upPos) {
			rectTransform.position -= new Vector3(x + upPos, 0, 0);
		}
		else if(!upState && x < 0) {
			float diff = -x + (speed * 4);
			rectTransform.position += new Vector3((((diff * (diff / 2)) / (upPos / 2))) * Time.deltaTime * speed, 0, 0);
		}
		else if(!upState && x > 0) {
			rectTransform.position -= new Vector3(x, 0, 0);
		}
	}

	public void Pressed() {
		upState = !upState;
	}
}
