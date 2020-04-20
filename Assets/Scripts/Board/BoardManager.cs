using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public GameObject grassPrefab, treePrefab, mountPrefab, waterPrefab;

	public float heightDifference = 0.3f;

	private TileMap map;

	void Start() {

	}
	
	void Update() {

	}

	public void GenerateBoard(TileMap map) {
		this.map = map;
		for(int x = 0; x < map.width; x++) {
			for(int y = 0; y < map.length; y++) {
				map[x, y].x = x;
				map[x, y].y = y;

				GameObject tile = MakeTile(map[x, y]);
			}
		}
	}

	GameObject MakeTile(Tile tile) {
		GameObject gp;
		switch(tile.type) {
			case Tile.GRASS:
				gp = grassPrefab;
				break;
			case Tile.TREE:
				gp = treePrefab;
				break;
			case Tile.MOUNTAIN:
				gp = mountPrefab;
				break;
			case Tile.WATER:
				gp = waterPrefab;
				break;
			default:
				throw new System.Exception("Tile type " + tile.type + " does not exist");
		}

		GameObject g = Instantiate(gp, transform.GetChild(0));
		g.transform.localPosition = new Vector3(tile.x - ((map.width - 1) / 2f), (tile.height + 1) * (heightDifference / 2), tile.y - ((map.length - 1) / 2f));
		float s = ((tile.height + 1) * heightDifference / 2);
		g.transform.GetChild(0).localScale = new Vector3(1, s, 1);
		g.transform.GetChild(0).localPosition = new Vector3(0, -s / 2, 0);

		return null;
	}

	void TextureCube(GameObject g) {
		Mesh mesh = g.GetComponent<MeshFilter>().mesh;
		Vector2[] UVs = new Vector2[mesh.vertices.Length];
		// Front
		UVs[0] = new Vector2(0.0f, 0.0f);
		UVs[1] = new Vector2(0.333f, 0.0f);
		UVs[2] = new Vector2(0.0f, 0.333f);
		UVs[3] = new Vector2(0.333f, 0.333f);
		// Top
		UVs[4] = new Vector2(0.334f, 0.333f);
		UVs[5] = new Vector2(0.666f, 0.333f);
		UVs[8] = new Vector2(0.334f, 0.0f);
		UVs[9] = new Vector2(0.666f, 0.0f);
		// Back
		UVs[6] = new Vector2(1.0f, 0.0f);
		UVs[7] = new Vector2(0.667f, 0.0f);
		UVs[10] = new Vector2(1.0f, 0.333f);
		UVs[11] = new Vector2(0.667f, 0.333f);
		// Bottom
		UVs[12] = new Vector2(0.0f, 0.334f);
		UVs[13] = new Vector2(0.0f, 0.666f);
		UVs[14] = new Vector2(0.333f, 0.666f);
		UVs[15] = new Vector2(0.333f, 0.334f);
		// Left
		UVs[16] = new Vector2(0.334f, 0.334f);
		UVs[17] = new Vector2(0.334f, 0.666f);
		UVs[18] = new Vector2(0.666f, 0.666f);
		UVs[19] = new Vector2(0.666f, 0.334f);
		// Right        
		UVs[20] = new Vector2(0.667f, 0.334f);
		UVs[21] = new Vector2(0.667f, 0.666f);
		UVs[22] = new Vector2(1.0f, 0.666f);
		UVs[23] = new Vector2(1.0f, 0.334f);
		mesh.uv = UVs;
	}
}
