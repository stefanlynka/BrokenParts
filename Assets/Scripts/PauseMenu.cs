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

    public int current_world = 1;
    public int world_count = 9;

    // Use this for initialization
    private void Awake() {
        Database = GameObject.Find("/DataBank");
        GetWorlds();
        AddUIs();
    }

    void Start () {
        SetPointText();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Paused) Resume();
            else Pause();
        }
	}

    private void GetWorlds() {
        for (int i = 0; i<transform.childCount; i++) {
            GameObject UI = transform.GetChild(i).gameObject;
            if ((UI.name.Length>=8)&&(UI.name.Substring(0, 8) == "UI World")) {
                Worlds.Add(UI);
                for(int j = 0; j < UI.transform.childCount; j++) {
                    GameObject button = UI.transform.GetChild(j).gameObject;
                    if (button.name.Substring(0, 5) == "Level") {
                        Levels.Add(button);
                    }
                }                
            }
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
    }

    public void GoToLevelSelect() {
        DeactivateUIs();
        Time.timeScale = 0;
        Paused = true;

        ActivateWorld(current_world);
    }

    public void GoToLevelEnd(float end_time, float time_highscore, float time_target, int ability_count, int ability_highscore, int ability_target) {
        DeactivateUIs();
        LevelEndUI.SetActive(true);
        Time.timeScale = 0;
        Paused = true;

        LevelEndUI.GetComponent<LevelEnd>().GetFinalStats(end_time, time_highscore, time_target, ability_count, ability_highscore, ability_target);
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

    private void AddUIs() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            UIs.Add(child);
            if ((child.name.Length >= 5) && (child.name.Substring(0, 5) == "Pause")) PauseMenuUI = child;
            if ((child.name.Length >= 9) && (child.name.Substring(0, 9) == "Level End")) LevelEndUI = child;
        }
    }

    private void DeactivateUIs() {
        for (int i = 0; i < UIs.Count; i++) {
            GameObject ui = UIs[i];
            bool IsInfo = ((ui.name.Length >= 5) && (ui.name.Substring(0, 4) == "Info"));
            bool IsLabel = ((ui.name.Length >= 5) && (ui.name.Substring(0, 5) == "Label"));
            if (!IsInfo && !IsLabel) ui.SetActive(false);
        }
    }

    private void SetPointText() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if ((child.name.Length >= 11) && (child.name.Substring(0,11) == "Label Score")){
                for(int j = 0; j < child.transform.childCount; j++) {
                    if ((child.transform.GetChild(j).name.Length>=10) && (child.transform.GetChild(j).name.Substring(0,10) == "Point Text")) {
                        child.transform.GetChild(j).GetComponent<Text>().text = Points.TotalPoints.ToString();
                    }
                }
            }
        }
    }

}