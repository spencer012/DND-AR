using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MapStorage {

	private static TileMap[] currentMaps;
	public static TileMap[] CurrentMaps {
		get { return currentMaps = (currentMaps == null ? GetData() : currentMaps); }
		set { SaveData(currentMaps = value); }
	}


	public static TileMap[] GetData() {
		if(false && File.Exists(Application.persistentDataPath + "/MySaveData.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();
			return data.data;
		}
		else {
			TileMap[] t = new TileMap[] { new TileMap(3, 3, new Tile[] { new Tile(0, 0), new Tile(0, 1), new Tile(1, 3),
																		 new Tile(3, 0), new Tile(3, 0), new Tile(3, 0),
																		 new Tile(2, 2), new Tile(2, 3), new Tile(2, 1)}),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 0) }),
										  new TileMap(1, 1, new Tile[] { new Tile(1, 1) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 2) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 3) }),
										  new TileMap(1, 1, new Tile[] { new Tile(1, 4) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 5) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 6) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 6) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 6) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 6) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 6) }),
										  new TileMap(1, 1, new Tile[] { new Tile(0, 6) }),
										  };
			SaveData(t);
			return t;
		}
	}

	public static void SaveData(TileMap[] maps) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/SavedMaps.dat");
		SaveData data = new SaveData();
		data.data = maps;
		bf.Serialize(file, data);
		file.Close();
	}
}

[System.Serializable]
class SaveData {
	public TileMap[] data;
}