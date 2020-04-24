using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public GameObject grassPrefab, treePrefab, mountPrefab, waterPrefab;

	public float heightDifference = 0.3f;

	private TileMap map;

	private GameObject[,] tileObjects = null;
	private List<GameObject> pieces = new List<GameObject>();

	public void AddPiece(GameObject g) {
		pieces.Add(g);
	}

	public void ClearBoard() {
		for(int x = 0; x < map.width; x++) {
			for(int y = 0; y < map.length; y++) {
				Destroy(tileObjects[x, y]);
			}
		}
		tileObjects = null;
		foreach(GameObject g in pieces) {
			Destroy(g);
		}
		pieces.Clear();
	}

	public void GenerateBoard(TileMap map) {
		if (this.map == map) {
			return;
		}
		if (tileObjects != null) {
			ClearBoard();
		}
		tileObjects = new GameObject[map.width, map.length];
		//occupied = new bool[map.width, map.length];
		this.map = map;
		for(int x = 0; x < map.width; x++) {
			for(int y = 0; y < map.length; y++) {
				map[x, y].x = x;
				map[x, y].y = y;

				tileObjects[x,y] = MakeTile(map[x, y]);
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

		return g;
	}

	/*
	public string PositionToIndex(float x, float y) {
		x += ((map.width - 1) / 2f);
		y += ((map.length - 1) / 2f);
		return x + "," + y;
	}

	private bool[,] occupied;

	public bool IsOccupied(float x, float y) {
		x += ((map.width - 1) / 2f);
		y += ((map.length - 1) / 2f);
		return occupied[(int)x, (int)y];
	}
	public void Occupy(float x, float y, bool yes) {
		x += ((map.width - 1) / 2f);
		y += ((map.length - 1) / 2f);
		occupied[(int)x, (int)y] = yes;
	}*/
}
