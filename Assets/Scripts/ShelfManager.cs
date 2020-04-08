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
		float y = rectTransform.position.y;
		print(y);
		if(upState && y < upPos) {
			float diff = upPos - y + (speed * 4);
			rectTransform.position += new Vector3(0, (((diff * (diff / 2)) / (upPos / 2))) * Time.deltaTime * speed, 0);
		}
		else if(upState && y > upPos) {
			rectTransform.position += new Vector3(0, upPos - y, 0);
		}
		else if(!upState && y > 0) {
			float diff = y + (speed * 4);
			rectTransform.position -= new Vector3(0, (((diff * (diff / 2)) / (upPos / 2))) * Time.deltaTime * speed, 0);
		}
		else if(!upState && y < 0) {
			rectTransform.position -= new Vector3(0, y, 0);
		}
	}

	public void Pressed() {
		upState = !upState;
	}
}
