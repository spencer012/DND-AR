using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug : MonoBehaviour {
	public Text text;
	public float totalMovement = 0;
	void Start() {
		Input.gyro.enabled = true;
	}
	
	void Update() {
		Vector3 r = Input.gyro.rotationRateUnbiased,
			a = Input.gyro.userAcceleration;
		float rc = ((Abs(r.x) + Abs(r.y) + Abs(r.z)) / 10);
		totalMovement += (rc + (4 * (Abs(a.x) + Abs(a.y) + Abs(a.z)))) * Time.deltaTime;
		text.text = System.String.Format("{0}\n{1}\t{2}", Input.gyro.rotationRateUnbiased, Input.gyro.userAcceleration, totalMovement);


	}

	float Abs(float a) {
		return Mathf.Abs(a);
	}
}
