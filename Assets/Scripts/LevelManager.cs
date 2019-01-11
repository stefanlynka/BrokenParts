using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

    public GameObject Menu;
    public GameObject DataBank;

    public List<GameObject> Levels = new List<GameObject>();

    public CharacterData level_data;

    int child_count;


    void Awake() {
        Menu = GameObject.Find("/Canvas");
        DataBank = GameObject.Find("/DataBank");
        SetUpLevels();
    }

    // Use this for initialization
    void Start() {
        ConnectLevelsToButtons();
        UpdateData();
        UpdateScores();
    }

    // Update is called once per frame
    void Update() {
    }


    public GameObject GetLevel(int num) {
        if (Levels.Count >= num) {
            GameObject level = Levels[num - 1];
            return level;
        }
        return null;
    }

    public void UpdateScores() {
        Points.TotalPoints = 0;
        for (int i = 0; i < Levels.Count; i++) {
            GameObject level = Levels[i];
            float time = level.GetComponent<Level>().time_highscore;
            int abilities = level.GetComponent<Level>().ability_highscore;
            level.GetComponent<Level>().UpdateHighScores(time, abilities);
        }
        if (SceneManager.GetActiveScene().name == "Menu") {
            Points.TotalPoints = DataBank.GetComponent<Data>().character_data.World1Score + DataBank.GetComponent<Data>().character_data.World2Score + DataBank.GetComponent<Data>().character_data.World3Score;
        }
    }

    void SetUpLevels() {
        child_count = transform.childCount;
        for (int i = 0; i < child_count; i++) {
            GameObject world = transform.GetChild(i).gameObject;
            for (int j = 0; j < world.transform.childCount; j++) {
                GameObject level = world.transform.GetChild(j).gameObject;
                Levels.Add(level);
                level.GetComponent<Level>().level_num = 9 * i + j + 1;
            }
        }
    }

    void ConnectLevelsToButtons() {
        List<GameObject> ButtonLevels = Menu.GetComponent<PauseMenu>().Levels;

        for (int i = 0; i < Levels.Count; i++) {
            GameObject this_level = Levels[i];
            this_level.GetComponent<Level>().LevelButton = ButtonLevels[i];
        }
    }

    public void SaveData() {
        UpdateScores();

        if (SceneManager.GetActiveScene().name == "World 1") {
            List<LevelData> list = new List<LevelData>(); //new list to hold all the level data
            for (int i = 0; i < Levels.Count; i++) {
                GameObject level = Levels[i];
                LevelData lvl_data = new LevelData { time = level.GetComponent<Level>().time_highscore, moves = level.GetComponent<Level>().ability_highscore, level_num = level.GetComponent<Level>().level_num };
                list.Add(lvl_data);
            }
            DataBank.GetComponent<Data>().character_data.World1LevelsData = list;
            DataBank.GetComponent<Data>().character_data.World1Score = Points.TotalPoints;

        }
        DataBank.GetComponent<Data>().SaveCharacter();
    }

    public void LoadData() {
        DataBank.GetComponent<Data>().character_data = DataBank.GetComponent<Data>().LoadCharacter();
        if (SceneManager.GetActiveScene().name == "Menu") {
            Points.TotalPoints = DataBank.GetComponent<Data>().character_data.World1Score + DataBank.GetComponent<Data>().character_data.World2Score + DataBank.GetComponent<Data>().character_data.World3Score;
        }
    }

    public void UpdateData() {
        if (SceneManager.GetActiveScene().name == "World 1") {

            for (int i = 0; i < Levels.Count; i++) {
                List<LevelData> data_list = DataBank.GetComponent<Data>().character_data.World1LevelsData;
                if (data_list.Count > i) {
                    GameObject level = Levels[i];
                    level.GetComponent<Level>().time_highscore = data_list[i].time;
                    level.GetComponent<Level>().ability_highscore = data_list[i].moves;
                }
            }
        }
    }
}
