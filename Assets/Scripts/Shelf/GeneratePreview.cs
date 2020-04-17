using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePreview : MonoBehaviour {

	private static bool done = false;

	public ShelfDisplayRack displayRack;

	void Start() {
		if (!done)
			StartCoroutine(Gen());
		else
			Destroy(gameObject);
	}

	IEnumerator Gen() {
		RuntimePreviewGenerator.PreviewDirection = new Vector3(50, -35, -40);
		RuntimePreviewGenerator.BackgroundColor = new Color(0, 0, 0, 0);
		RuntimePreviewGenerator.Padding = -0.05f;

		for(int i = 0; i < displayRack.displays.Length; i++) {
			if (displayRack.displays[i].prefab != null)
				displayRack.displays[i].sprite = RuntimePreviewGenerator.GenerateModelPreview(displayRack.displays[i].prefab.transform, 1000, 1000);
			yield return null;
		}
		done = true;
		Destroy(gameObject);
	}
}
