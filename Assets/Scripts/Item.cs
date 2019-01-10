using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour {

    public string item_name = "";
    public int cost = 1;
    public bool bought = false;
    public bool trying = false;
    public Sprite picture;
    public Vector2 item_scale = new Vector3(1, 1);
    public Vector3 item_offset = new Vector3(0,0,0);

    public Equip this_item;

    private void Awake() {
        this_item = new Equip {Price = cost, bought = bought, image_name = picture.name, scale = item_scale, offset = item_offset };

    }

    // Use this for initialization
    void Start () {
        SetCosts();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TryItem() {
        if (!bought) {
            GameObject buyer = transform.parent.transform.parent.GetComponent<Points>().player;
            if (!trying) {
                trying = true;
                buyer.GetComponent<Player>().EquipItem(this_item);
            }
            else if (trying) {
                trying = false;
                buyer.GetComponent<Player>().UnequipItem(this_item);
            }
        }
    }

    public void BuyItem() {
        GameObject buyer = transform.parent.transform.parent.GetComponent<Points>().player;

        List<Equip> already_bought = buyer.GetComponent<Player>().equipped_items;
        for (int i = 0; i < buyer.GetComponent<Player>().equipped_items.Count; i++) {
            if (already_bought[i].image_name == this_item.image_name) {
                bought = true;
            }
        }

        if ((cost <= Points.TotalPoints) && (bought == false)) {
            bought = true;

            this_item.rotation = buyer.transform.rotation;

            buyer.GetComponent<Player>().EquipItem(this_item);
            SetText("Unequip");
            if (trying) trying = false;
        }
        else if (bought) {
            bought = false;
            buyer.GetComponent<Player>().UnequipItem(this_item);
            SetText("Equip");
        }
    }
    private void SetText(string new_text) {
        GameObject parent = transform.parent.gameObject;
        for (int i = 0; i < parent.transform.childCount; i++) {
            GameObject child = parent.transform.GetChild(i).gameObject;
            if (child.name.Contains("Buy")) {
                child.transform.GetChild(0).GetComponent<Text>().text = new_text;
            }
        }
    }

    private void SetCosts() {
        GameObject parent = transform.parent.gameObject;
        for (int i = 0; i < parent.transform.childCount; i++) {
            GameObject child = parent.transform.GetChild(i).gameObject;
            if (child.name.Contains("Price")) {
                child.transform.GetChild(0).GetComponent<Text>().text = cost.ToString();
            }
        }
    }
}
