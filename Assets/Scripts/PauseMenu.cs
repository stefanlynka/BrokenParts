using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool Paused = false;

    public List<GameObject> UIs = new List<GameObject>();
    public List<GameObject> Worlds = new List<GameObject>();
    public List<GameObject> Levels = new List<GameObject>();
    GameObject Database;

    Text PointText;

    GameObject PauseMenuUI;
    GameObject LevelEndUI;

    int current_world = 1;
    int world_count = 0;
    string ui_state = "playing";

    // Use this for initialization
    private void Awake() {
        Database = GameObject.Find("/DataBank");
        GetWorlds();
        AddUIs();
    }

    void Start () {
        SetupPauseButtons();
        SetupWorldButtons();
        if(LevelEndUI) SetupEndButtons();

        SetPointText();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Paused) Resume();
            else Pause();
        }
	}

    // Add Worlds and Levels to Lists
    private void GetWorlds() {
        for (int i = 0; i<transform.childCount; i++) {
            GameObject UI = transform.GetChild(i).gameObject;
            if (UI.name.Contains("UI World")) {
                Worlds.Add(UI);
                world_count++;
                for(int j = 0; j < UI.transform.childCount; j++) {
                    GameObject button = UI.transform.GetChild(j).gameObject;
                    if (button.name.Contains("Level")){
                        Levels.Add(button);
                    }
                }                
            }
        }
    }

    private void AddUIs() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            UIs.Add(child);
            if (child.name.Contains("Pause")) PauseMenuUI = child;
            if (child.name.Contains("Level End")) LevelEndUI = child;
        }
    }

    private void SetupWorldButtons() {
        for (int i = 0; i < world_count; i++) {
            GameObject world = Worlds[i];

            for (int j = 0; j < world.transform.childCount; j++) {
                GameObject ui = world.transform.GetChild(j).gameObject;
                if (ui.name.Contains("Prev")) {
                    ui.GetComponent<ButtonActions>().MakePrevButton();
                    if(i==0) ui.SetActive(false);
                }
                if (ui.name.Contains("Next")) {
                    ui.GetComponent<ButtonActions>().MakeNextWorldButton();
                    if (i==world_count-1) ui.SetActive(false);
                }
                if (ui.name.Contains("Level")) {
                    ui.GetComponent<ButtonActions>().MakeLevelButton();
                    ui.GetComponent<ButtonActions>().level_num = i*9 + (ui.name[6]-48);
                }
                if (ui.name.Contains("Back")) {
                    ui.GetComponent<ButtonActions>().MakeBackButton();
                }
                if (ui.name.Contains("WorldNum")) {
                    ui.GetComponent<Text>().text = "World " + (i+1);
                }
            }
        }
    }

    private void SetupPauseButtons() {
        for (int i = 0; i < PauseMenuUI.transform.childCount; i++) {
            GameObject button = PauseMenuUI.transform.GetChild(i).gameObject;
            if (button.name.Contains("Resume")) button.GetComponent<ButtonActions>().MakeResumeButton();
            if (button.name.Contains("Select")) button.GetComponent<ButtonActions>().MakeSelectButton();
            if (button.name.Contains("Menu")) button.GetComponent<ButtonActions>().MakeMenuButton();
        }
    }
    private void SetupEndButtons() {
        for (int i = 0; i < LevelEndUI.transform.childCount; i++) {
            GameObject button = LevelEndUI.transform.GetChild(i).gameObject;
            if (button.name.Contains("Next Level")) button.GetComponent<ButtonActions>().MakeNextLevelButton();
            if (button.name.Contains("Select")) button.GetComponent<ButtonActions>().MakeSelectButton();
            if (button.name.Contains("Replay")) button.GetComponent<ButtonActions>().MakeRestartButton();
        }
    }

    public void Resume() {
        DeactivateUIs();
        Time.timeScale = 1;
        Paused = false;
    }

    public void Pause() {
        DeactivateUIs();
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        Paused = true;
        ui_state = "pause";
        print("pause called, ui_state = pause");
    }

    public void GoBack() {
        print("go back activated");
        if (ui_state == "end") {
            DeactivateUIs();
            LevelEndUI.SetActive(true);
        }
        else {
            Pause();
        }
    }

    public void GoToLevelSelect() {
        DeactivateUIs();
        Time.timeScale = 0;
        Paused = true;

        ActivateWorld(current_world);
    }

    public void GoToLevelEnd(float end_time, float time_highscore, string time_highscore_rank, string time_rank, int ability_count, int ability_highscore, string ability_highscore_rank, string move_rank, bool dev_time, bool high_dev_time) {
        DeactivateUIs();
        LevelEndUI.SetActive(true);
        Time.timeScale = 0;
        Paused = true;
        ui_state = "end";
        print("ui_state = end");

        LevelEndUI.GetComponent<LevelEnd>().GetFinalStats(end_time, time_highscore, time_highscore_rank, time_rank, ability_count, ability_highscore, ability_highscore_rank, move_rank, dev_time, high_dev_time);
    }

    public void GoToNextWorldLevelSelect() {
        if (current_world<world_count) current_world++;
        GoToLevelSelect();
    }

    public void GoToPrevWorldLevelSelect() {
        if (current_world > 1) current_world--;
        GoToLevelSelect();
    }

    public void Quit() {
        Database.GetComponent<Data>().SaveCharacter();
        SceneManager.LoadScene("Menu");
    }

    public void ActivateWorld(int world) {
        Worlds[world-1].SetActive(true);
        }

    private void DeactivateUIs() {
        for (int i = 0; i < UIs.Count; i++) {
            GameObject ui = UIs[i];
            bool IsInfo = (ui.name.Contains("Info"));
            bool IsLabel = (ui.name.Contains("Label"));
            if (!IsInfo && !IsLabel) ui.SetActive(false);
        }
    }

    private void SetPointText() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name.Contains("Label Score")){
                for(int j = 0; j < child.transform.childCount; j++) {
                    if ((child.transform.GetChild(j).name.Length>=10) && (child.transform.GetChild(j).name.Contains("Point Text"))) {
                        child.transform.GetChild(j).GetComponent<Text>().text = Points.TotalPoints.ToString();
                    }
                }
            }
        }
    }

}