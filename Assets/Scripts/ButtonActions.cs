using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour {

    public int level_num = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MakeNextWorldButton() {
        GetComponent<Button>().onClick.AddListener(NextWorld);
    }

    public void MakePrevButton() {
        GetComponent<Button>().onClick.AddListener(Prev);
    }

    public void MakeBackButton() {
        GetComponent<Button>().onClick.AddListener(Back);
    }

    public void MakeLevelButton() {
        GetComponent<Button>().onClick.AddListener(LoadLevel);
    }


    public void MakeResumeButton() {
        GetComponent<Button>().onClick.AddListener(Resume);
    }
    public void MakeSelectButton() {
        GetComponent<Button>().onClick.AddListener(Select);
    }
    public void MakeMenuButton() {
        GetComponent<Button>().onClick.AddListener(Menu);
    }

    public void MakeNextLevelButton() {
        GetComponent<Button>().onClick.AddListener(NextLevel);
    }
    public void MakeRestartButton() {
        GetComponent<Button>().onClick.AddListener(Restart);
    }
    

    public void NextWorld() {
        GameObject.Find("/Canvas").GetComponent<PauseMenu>().GoToNextWorldLevelSelect();
    }
    public void Prev() {
        GameObject.Find("/Canvas").GetComponent<PauseMenu>().GoToPrevWorldLevelSelect();
    }
    public void Back() {
        GameObject.Find("/Canvas").GetComponent<PauseMenu>().GoBack();
    }
    public void LoadLevel() {
        print("loading level : " + level_num);
        GameObject.Find("/Player").GetComponent<Player>().LoadLevelX(level_num);
        GameObject.Find("/Canvas").GetComponent<PauseMenu>().Resume();
    }

    void Resume() {
        GameObject.Find("/Canvas").GetComponent<PauseMenu>().Resume();
    }
    void Select() {
        GameObject.Find("/Canvas").GetComponent<PauseMenu>().GoToLevelSelect();
    }
    void Menu() {
        GameObject.Find("/Canvas").GetComponent<PauseMenu>().Quit();
    }

    void NextLevel() {
        GameObject.Find("/Player").GetComponent<Player>().LoadNextLevel();
    }
    void Restart() {
        GameObject.Find("/Player").GetComponent<Player>().LoadCurrentLevel();
    }
}
