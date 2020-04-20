[System.Serializable]
public struct Tile {
	public const int GRASS = 0, TREE = 1, MOUNTAIN = 2, WATER = 3;

	public int type;
	public int height;
	public int x, y;

	public Tile(int type, int height, int x, int y) {
		this.type = type;
		this.height = height;
		this.x = x;
		this.y = y;
	}

	public Tile(int type, int height) {
		this.type = type;
		this.height = height;
		x = -1;
		y = -1;
	}
}
