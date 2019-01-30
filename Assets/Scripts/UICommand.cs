using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommand : MonoBehaviour {

    Text Level;
    Text Collected;
    Text AbilitiesUsed;
    Text Timer;

    GameObject Player;

    private void Start() {
        FindText();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateText();
    }

    // Find UI Text Components
    void FindText() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name.Contains("Level Info")) Level = child.transform.GetChild(0).GetComponent<Text>();
            if (child.name.Contains("Acorns Collected Info")) Collected = child.transform.GetChild(0).GetComponent<Text>();
            if (child.name.Contains("Abilities Used Info")) AbilitiesUsed = child.transform.GetChild(0).GetComponent<Text>();
            if (child.name.Contains("Time Taken Info")) Timer = child.transform.GetChild(0).GetComponent<Text>();
        }

        Player = GameObject.Find("/Player");
    }

    // Update UI Text Components
    void UpdateText() {
        Level.text = "Level " + Player.GetComponent<Player>().CurrentLevel.GetComponent<Level>().level_num.ToString();
        Collected.text = "Acorns Collected: " + Player.GetComponent<Player>().collected.ToString();
        float ability_ratio = Player.GetComponent<Player>().ability_counter / 0.5f;
        if (Player.GetComponent<Player>().CurrentLevel.GetComponent<Level>().target_moves_silver != 0) ability_ratio = Player.GetComponent<Player>().ability_counter / Player.GetComponent<Player>().CurrentLevel.GetComponent<Level>().target_moves_silver;

        AbilitiesUsed.text = "Abilities Used: " + Player.GetComponent<Player>().ability_counter.ToString();
        AbilitiesUsed.color = new Color(ability_ratio, 1 - ability_ratio, 0);

        float time = Player.GetComponent<Player>().timer;
        Timer.text = "Timer: " + time.ToString("F2");
        float time_ratio = time / Player.GetComponent<Player>().CurrentLevel.GetComponent<Level>().target_time_silver;
        Timer.color = new Color(time_ratio, 1-time_ratio, 0f);

    }
}