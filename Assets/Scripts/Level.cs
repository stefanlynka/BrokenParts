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
    public int target_moves_gold;
    public int target_moves_silver;
    public int target_moves_bronze;

    public float target_time_gold;
    public float target_time_silver;
    public float target_time_bronze;

    public bool extend_allowed = true;
    public bool retract_allowed = true;
    public bool flip_x_allowed = true;
    public bool flip_y_allowed = true;

    public int ability_highscore;
    public float time_highscore = 999F;

    public string move_rank = "";
    public string time_rank = "";
    public string move_highscore_rank = "";
    public string time_highscore_rank = "";

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

        if (time_highscore <= target_time_gold) time_highscore_rank = "gold";
        else if (time_highscore <= target_time_silver) time_highscore_rank = "silver";
        else if (time_highscore <= target_time_bronze) time_highscore_rank = "bronze";

        if (ability_highscore <= target_moves_gold) move_highscore_rank = "gold";
        else if (ability_highscore <= target_moves_silver) move_highscore_rank = "silver";
        else if (ability_highscore <= target_moves_bronze) move_highscore_rank = "bronze";

        for ( int i = 0; i < LevelButton.transform.childCount; i++) {
            GameObject level_text = LevelButton.transform.GetChild(i).gameObject;
            level_text.SetActive(false);

            if ((level_text.GetComponent<Text>()) && (i == 1)) {
                level_text.GetComponent<Text>().text = "Time: " + time_highscore.ToString("F2") + "\nMoves: " + ability_highscore;
                if(time_highscore == 999) level_text.GetComponent<Text>().text = "Time: " + "\nMoves: ";
            }

            if (level_text.name.Contains("Gold Acorn Time") && time_highscore_rank =="gold") {
                level_text.SetActive(true);
                Points.TotalPoints += 3;
            }
            else if (level_text.name.Contains("Silver Acorn Time") && time_highscore_rank == "silver") {
                level_text.SetActive(true);
                Points.TotalPoints += 2;
            }
            else if (level_text.name.Contains("Bronze Acorn Time") && time_highscore_rank == "bronze") {
                level_text.SetActive(true);
                Points.TotalPoints += 1;
            }
            if (level_text.name.Contains("Gold Acorn Moves") && move_highscore_rank == "gold") {
                level_text.SetActive(true);
                Points.TotalPoints += 3;
            }
            else if (level_text.name.Contains("Silver Acorn Moves") && move_highscore_rank == "silver") {
                level_text.SetActive(true);
                Points.TotalPoints += 2;
            }
            else if (level_text.name.Contains("Bronze Acorn Moves") && move_highscore_rank == "bronze") {
                level_text.SetActive(true);
                Points.TotalPoints += 1;
            }
        }

        if (time <= target_time_gold) time_rank = "gold";
        else if (time <= target_time_silver) time_rank = "silver";
        else if (time <= target_time_bronze) time_rank = "bronze";
        else time_rank = "";
        print("time_rank at set is: " + time_rank);

        if (abilities <= target_moves_gold) move_rank = "gold";
        else if (time <= target_moves_silver) move_rank = "silver";
        else if (time <= target_moves_bronze) move_rank = "bronze";
        else move_rank = "";
    }

}
