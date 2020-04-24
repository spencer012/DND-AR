using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour {

	public GameObject mapSelectionTogglePrefab;

	public RectTransform rectTransform;

	public RectTransform border;
	public Button up, down;

	[HideInInspector]
	public Texture2D[] sprites;

	private int height, selectedHeight = 1;
	public float bufferPercent = 0.07f;
	private float bufferWidth, displayWidth, bufferHeight;

	bool initialized = false;

	void Awake() {
#if !UNITY_EDITOR
		InitDisplay();
#endif
	}
	public void InitDisplay() {
		height = Mathf.CeilToInt(MapStorage.CurrentMaps.Length / 3f);
		if(height < 2)
			height = 2;

		if(height < 3) {
			up.interactable = false;
		}
		down.interactable = false;

		bufferWidth = (border.position.x) / 3 * bufferPercent;
		displayWidth = (border.position.x) / 3 * (1 - bufferPercent);
		bufferHeight = Screen.height / 2;

		float yPos = 0;
		for(int i = 0; i < height; i++) {
			yPos += bufferHeight / 2;
			float xPos = 0;
			for(int j = 0; j < 3; j++) {
				xPos += bufferWidth + (displayWidth / 2);

				GameObject g = Instantiate(mapSelectionTogglePrefab, transform);
				RectTransform gt = g.GetComponent<RectTransform>();
				gt.position = new Vector3(xPos, yPos, 0);
				gt.sizeDelta = new Vector2(displayWidth, displayWidth);
				MapNode node = g.GetComponent<MapNode>();

				int index = i * 3 + j;
				if(index < MapStorage.CurrentMaps.Length) {
					node.image.texture = sprites[index];
					node.index = index;
				}
				else {
					node.toggle.interactable = false;
					node.image.enabled = false;
					node.index = index;
				}
				xPos += (displayWidth / 2);
			}
			yPos += bufferHeight / 2;
		}

		initialized = true;
	}

	void Update() {
		if(initialized) {
			float targetHeight = (selectedHeight - 1) * bufferHeight;
			if(rectTransform.anchoredPosition.y > targetHeight - 1 && rectTransform.anchoredPosition.y < targetHeight + 1 && rectTransform.anchoredPosition.y != targetHeight) {
				rectTransform.anchoredPosition = Vector2.up * targetHeight;
			}
			else if(rectTransform.anchoredPosition.y != targetHeight) {
				float x = (targetHeight - rectTransform.anchoredPosition.y);
				rectTransform.anchoredPosition += Vector2.up * (x * 4 + 1)* Time.deltaTime;
			}
		}
	}

	public void Move(int step) {
		selectedHeight += -step;

		if(selectedHeight == 1) {
			down.interactable = false;
		}
		else {
			down.interactable = true;
		}

		if(selectedHeight - 3 == -height) {
			up.interactable = false;
		}
		else {
			up.interactable = true;
		}
	}
}
