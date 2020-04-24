using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSize : MonoBehaviour {
	void Start() {
		GetComponent<Terrain>().terrainData.size = transform.lossyScale;
	}
}
