using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public static int current_level = 0;

    public GameObject level_entities;
    public GameObject terrain_manager;
    public GameObject LevelButton;

    public Vector2 origin = new Vector2(0,0);
    public float map_width = 10;
    public float map_height = 6;
    public int level_num = 0;
    public int required_acorns = 0;
    public int target_moves;
    public float target_time;

    public bool extend_allowed = true;
    public bool retract_allowed = true;
    public bool flip_x_allowed = true;
    public bool flip_y_allowed = true;

    public int ability_highscore;
    public float time_highscore = 999F;

    // Use this for initialization
    void Awake () {
        origin = new Vector2(transform.position.x, transform.position.y);
	}

    private void Start() {
        required_acorns = level_entities.GetComponent<LevelEntities>().AcornNum();
    }

    // Update is called once per frame
    void Update () {
		
	}

    // 
    public void UpdateHighScores(float time, int abilities) {

        if (time < time_highscore) time_highscore = time;
        if (abilities < ability_highscore) ability_highscore = abilities;

        for( int i = 0; i < LevelButton.transform.childCount; i++) {
            GameObject level_text = LevelButton.transform.GetChild(i).gameObject;

            if ((level_text.GetComponent<Text>()) && (i == 1)) {
                level_text.GetComponent<Text>().text = "Time: " + time_highscore.ToString("F2") + "\nMoves: " + ability_highscore;
                if(time_highscore == 999) level_text.GetComponent<Text>().text = "Time: " + "\nMoves: ";
            }

            if (level_text.name.Length >= 14) {
                if ((LevelButton.transform.childCount > 2) && (level_text.name.Substring(11, 4) == "Time") && (time_highscore <= target_time)) {
                    level_text.SetActive(true);
                    Points.TotalPoints++;
                }

                if ((LevelButton.transform.childCount > 3) && (level_text.name.Substring(11, 4) == "Move") && (ability_highscore <= target_moves)) {
                    level_text.SetActive(true);
                    Points.TotalPoints++;
                }
            }
        }

    }

}
