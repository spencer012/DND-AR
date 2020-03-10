using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMover : MonoBehaviour {

	public static float timeToMove = 1,
		upDistance = 1;

	private static Vector3 myNull = new Vector3(-10, -10, -10);
	private Vector3 moveTo = myNull,
		upOrig = myNull, upMoved = myNull, orig = myNull;

	private float timeLerp = 0;


	void Update() {
		if (moveTo != myNull) {
			timeLerp += Time.deltaTime / timeToMove;
		}

		if(upOrig != myNull) { //go to each position one at a time
			transform.position = Vector3.Lerp(orig, upOrig, timeLerp);
			if (timeLerp >= 1) {
				upOrig = myNull;
				timeLerp = 0;
			}
		}
		else if(upMoved != myNull) {
			transform.position = Vector3.Lerp(upOrig, upMoved, timeLerp);
			if(timeLerp >= 1) {
				upOrig = myNull;
				timeLerp = 0;
			}
		}
		else if(moveTo != myNull) {
			transform.position = Vector3.Lerp(upMoved, moveTo, timeLerp);
			if(timeLerp >= 1) {
				transform.position = moveTo;
				tag = "Piece";
				Destroy(this);
			}
		}
	}

	public void MoveTo(Vector3 pos) {
		moveTo = pos;
		upMoved = pos + Vector3.up * upDistance;
		upOrig = new Vector3(transform.position.x, upMoved.y, transform.position.z);
		orig = transform.position;
		tag = "Moving";
	}
}
