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
        AbilitiesUsed.text = "Abilities Used: " + Player.GetComponent<Player>().ability_counter.ToString();
        Timer.text = "Timer: " + Player.GetComponent<Player>().timer.ToString("F2");

    }
}