using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMover : MonoBehaviour {

	public static float timeToMove = 1,
		upDistance = 1f * BoardManager.scale;

	
	private Vector3 moveTo,	upOrig, upMoved, orig;
	private int step = 0;

	private float timeLerp = 0;


	void Update() {
		timeLerp += Time.deltaTime / timeToMove;

		if(step == 0) { //go to each position one at a time
			transform.localPosition = Vector3.Lerp(orig, upOrig, timeLerp);
			if (timeLerp >= 1) {
				step++;
				timeLerp = 0;
				//print("Orig done");
			}
		}
		else if(step == 1) {
			transform.localPosition = Vector3.Lerp(upOrig, upMoved, timeLerp);
			if(timeLerp >= 1) {
				step++;
				timeLerp = 0;
				//print("Moved done");
			}
		}
		else if(step == 2) {
			transform.localPosition = Vector3.Lerp(upMoved, moveTo, timeLerp);
			if(timeLerp >= 1) {
				transform.localPosition = moveTo;
				tag = "Piece";
				//print("moveTo done");
				Destroy(this);
			}
		}
	}

	public void MoveTo(Vector3 pos) {
		if(moveTo == pos) {
			return;
		}
		moveTo = pos;
		upMoved = pos + (Vector3.up * upDistance);
		upOrig = new Vector3(transform.localPosition.x, upMoved.y, transform.localPosition.z);
		orig = transform.localPosition;
		if (tag == "Moving") {
			step++;
		}
		tag = "Moving";
	}
}
