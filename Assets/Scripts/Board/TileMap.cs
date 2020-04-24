using UnityEngine;

[System.Serializable]
public class TileMap {
	public int width = -1;
	public int length = -1;

	[SerializeField]
	public Tile[] tiles;

	public TileMap(int width, int length, Tile[] tiles) {
		if(width * length != tiles.Length)
			throw new System.Exception("Array size does not equal initialized size");
		this.width = width;
		this.length = length;
		this.tiles = tiles;
	}

	public void SetTile(int x, int y, Tile tile) {
		// 2D representation stored in row-major order.
		tiles[y * width + x] = tile;
	}

	public ref Tile GetTile(int x, int y) {
		return ref tiles[y * width + x];
	}
	public ref Tile this[int x, int y] {
		get { return ref GetTile(x, y); }
	}

	public Vector2Int GetCoordinate(int index) {
		int x = index % width;
		int y = index / width;
		return new Vector2Int(x, y);
	}
}