using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommand : MonoBehaviour {

    public Text Level;
    public Text Collected;
    public Text AbilitiesUsed;
    public Text Timer;

    public GameObject Player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Level.text = "Level " + Player.GetComponent<Player>().CurrentLevel.GetComponent<Level>().level_num.ToString();
        Collected.text = "Acorns Collected: " + Player.GetComponent<Player>().collected.ToString();
        AbilitiesUsed.text = "Abilities Used: " + Player.GetComponent<Player>().ability_counter.ToString();
        Timer.text = "Timer: " + Player.GetComponent<Player>().timer.ToString("F2");
    }
}