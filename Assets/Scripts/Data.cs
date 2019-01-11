using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

    public CharacterData character_data = new CharacterData();
    public string data_path;

	// Use this for initialization
	void Awake () {
        data_path = Path.Combine(Application.persistentDataPath, "CharacterData.txt");
        //print(data_path);

        if (!File.Exists(data_path)) SaveCharacter();

        character_data = LoadCharacter();
        if (character_data == null) character_data = new CharacterData();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SaveCharacter() {
        string jsonString = JsonUtility.ToJson(character_data);

        using (StreamWriter stream_writer = File.CreateText(data_path)) {
            stream_writer.Write(jsonString);
        }
    }

     public CharacterData LoadCharacter() {
        using (StreamReader stream_reader = File.OpenText(data_path)) {
            string json_string = stream_reader.ReadToEnd();
            return JsonUtility.FromJson<CharacterData>(json_string);
        }
    }

}

[System.Serializable]
public class CharacterData {
    public List<LevelData> World1LevelsData = new List<LevelData>();
    public List<LevelData> World2LevelsData = new List<LevelData>();
    public List<LevelData> World3LevelsData = new List<LevelData>();
    public List<Equip> Equips = new List<Equip>();
    public int World1Score = 0;
    public int World2Score = 0;
    public int World3Score = 0;
}

[System.Serializable]
public class LevelData {
    public float time = 99;
    public int moves = 99;
    public int level_num;
}

[System.Serializable]
public class Equip {
    public int Price = 0;
    public bool bought = false;
    //public string image_path;
    //public Sprite image = new Sprite();
    public string image_name = "";
    public Vector2 scale = new Vector2(1, 1);
    public Vector3 offset = new Vector3(0, 0, 0);
    public Quaternion rotation =  Quaternion.identity;
}

