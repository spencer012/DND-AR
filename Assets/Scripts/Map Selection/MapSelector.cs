using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelector : MonoBehaviour {
	TileMap[] maps;

	void Start() {
		maps = MapStorage.CurrentMaps;
		MapNode.parent = this;
	}
	
	public void Selected (int index) {
		//print(index);
		GameManager.gameManager.currentMap = maps[index];
		MySceneManager.sceneManager.ChangeScene(MySceneManager.MAIN);
		gameObject.SetActive(false);
	}
}
