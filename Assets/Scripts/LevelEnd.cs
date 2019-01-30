using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour {

    GameObject TimeText;
    GameObject TimeHighScoreText;
    GameObject AbilityText;
    GameObject AbilityHighscoreText;

    public List<GameObject> UIElements = new List<GameObject>();

    private void Awake() {
        FindUIElements();
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FindUIElements() {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            if (child.name.Contains("Time Taken")) TimeText = child;
            if (child.name.Contains("Time Highscore")) TimeHighScoreText = child;
            if (child.name.Contains("Abilities Used")) AbilityText = child;
            if (child.name.Contains("Abilities Highscore")) AbilityHighscoreText = child;
        }
    }

    public void GetFinalStats(float time, float time_highscore, string time_highscore_rank, string time_rank, int abilities_used, int ability_highscore, string move_highscore_rank, string move_rank) {


        for (int i = 0; i < TimeText.transform.childCount; i++) {
            GameObject time_acorn = TimeText.transform.GetChild(i).gameObject;
            time_acorn.SetActive(false);
            print("time rank : " + time_rank);
            if (time_acorn.name.Contains("Gold") && time_rank == "gold") time_acorn.SetActive(true);
            if (time_acorn.name.Contains("Silver") && time_rank == "silver") time_acorn.SetActive(true);
            if (time_acorn.name.Contains("Bronze") && time_rank == "bronze") time_acorn.SetActive(true);
        }

        for (int i = 0; i < TimeHighScoreText.transform.childCount; i++) {
            GameObject time_highscore_acorn = TimeHighScoreText.transform.GetChild(i).gameObject;
            time_highscore_acorn.SetActive(false);
            if (time_highscore_acorn.name.Contains("Gold") && time_highscore_rank == "gold") time_highscore_acorn.SetActive(true);
            if (time_highscore_acorn.name.Contains("Silver") && time_highscore_rank == "silver") time_highscore_acorn.SetActive(true);
            if (time_highscore_acorn.name.Contains("Bronze") && time_highscore_rank == "bronze") time_highscore_acorn.SetActive(true);
        }

        for (int i = 0; i < AbilityText.transform.childCount; i++) {
            GameObject move_acorn = AbilityText.transform.GetChild(i).gameObject;
            move_acorn.SetActive(false);
            if (move_acorn.name.Contains("Gold") && move_rank == "gold") move_acorn.SetActive(true);
            if (move_acorn.name.Contains("Silver") && move_rank == "silver") move_acorn.SetActive(true);
            if (move_acorn.name.Contains("Bronze") && move_rank == "bronze") move_acorn.SetActive(true);
        }

        for (int i = 0; i < AbilityHighscoreText.transform.childCount; i++) {
            GameObject move_highscore_acorn = AbilityHighscoreText.transform.GetChild(i).gameObject;
            move_highscore_acorn.SetActive(false);
            if (move_highscore_acorn.name.Contains("Gold") && move_highscore_rank == "gold") move_highscore_acorn.SetActive(true);
            if (move_highscore_acorn.name.Contains("Silver") && move_highscore_rank == "silver") move_highscore_acorn.SetActive(true);
            if (move_highscore_acorn.name.Contains("Bronze") && move_highscore_rank == "bronze") move_highscore_acorn.SetActive(true);
        }

        /*
        if (time <= time_target) TimeText.transform.GetChild(0).gameObject.SetActive(true);
        else TimeText.transform.GetChild(0).gameObject.SetActive(false);

        if (time_highscore <= time_target) TimeHighScoreText.transform.GetChild(0).gameObject.SetActive(true);
        else TimeHighScoreText.transform.GetChild(0).gameObject.SetActive(false);

        if (abilities_used <= ability_target) AbilityText.transform.GetChild(0).gameObject.SetActive(true);
        else AbilityText.transform.GetChild(0).gameObject.SetActive(false);

        if (ability_highscore <= ability_target) AbilityHighscoreText.transform.GetChild(0).gameObject.SetActive(true);
        else AbilityHighscoreText.transform.GetChild(0).gameObject.SetActive(false);
        */

        TimeText.GetComponent<Text>().text = "Time\n" + time.ToString("F2");
        TimeHighScoreText.GetComponent<Text>().text = "Highscore\n" + time_highscore.ToString("F2");

        AbilityText.GetComponent<Text>().text = "Abilities used\n" + abilities_used;
        AbilityHighscoreText.GetComponent<Text>().text = "Highscore\n" + ability_highscore;
    }

}
