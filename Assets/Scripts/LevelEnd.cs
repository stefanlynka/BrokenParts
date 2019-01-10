using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour {

    public GameObject TimeText;
    public GameObject TimeHighScoreText;
    public GameObject AbilityText;
    public GameObject AbilityHighscoreText;

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

    public void GetFinalStats(float time, float time_highscore, float time_target, int abilities_used, int ability_highscore, int ability_target) {

        if (time <= time_target) TimeText.transform.GetChild(0).gameObject.SetActive(true);
        else TimeText.transform.GetChild(0).gameObject.SetActive(false);

        if (time_highscore <= time_target) TimeHighScoreText.transform.GetChild(0).gameObject.SetActive(true);
        else TimeHighScoreText.transform.GetChild(0).gameObject.SetActive(false);

        if (abilities_used <= ability_target) AbilityText.transform.GetChild(0).gameObject.SetActive(true);
        else AbilityText.transform.GetChild(0).gameObject.SetActive(false);

        if (ability_highscore <= ability_target) AbilityHighscoreText.transform.GetChild(0).gameObject.SetActive(true);
        else AbilityHighscoreText.transform.GetChild(0).gameObject.SetActive(false);

        TimeText.GetComponent<Text>().text = "Time\n" + time.ToString("F2");
        TimeHighScoreText.GetComponent<Text>().text = "Highscore\n" + time_highscore.ToString("F2");

        AbilityText.GetComponent<Text>().text = "Abilities used\n" + abilities_used;
        AbilityHighscoreText.GetComponent<Text>().text = "Highscore\n" + ability_highscore;
    }

}
