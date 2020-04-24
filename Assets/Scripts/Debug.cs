using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug : MonoBehaviour {

	public ShelfDisplayRack displayRack;

	public Vector3 direction;
	public float padding;

	void Start() {
		InvokeRepeating("get", 0, 0.5f);
	}
	
	void Update() {


	}

	void get() {
		RuntimePreviewGenerator.PreviewDirection = direction;
		RuntimePreviewGenerator.Padding = padding;
		GetComponent<RawImage>().texture = RuntimePreviewGenerator.GenerateModelPreview(displayRack.displays[0].prefab.transform, 1000, 1000);
	}
}
