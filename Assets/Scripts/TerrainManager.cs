using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    List<GameObject> Blocks = new List<GameObject>();

    int child_count;

    // Use this for initialization
    void Start () {
        child_count = transform.childCount;
        for(int i = 0; i < child_count; i++) {
            GameObject block = transform.GetChild(i).gameObject;
            Blocks.Add(block);
        }
		 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool SafeDestination(Vector2 destination) {
        for(int i = 0; i< Blocks.Count; i++) {
            GameObject block = Blocks[i];
            if (Collide(destination, block)) return false;
        }
        return true;
    }

    bool Collide(Vector2 destination, GameObject block) {
        float width = block.GetComponent<Renderer>().bounds.size.x;
        float height = block.GetComponent<Renderer>().bounds.size.y;
        float x_pos = block.transform.position.x;
        float y_pos = block.transform.position.y;
        if ((destination.x > x_pos-width / 2) && (destination.x < x_pos+width / 2) && (destination.y > y_pos-height / 2) && (destination.y < y_pos + height / 2)){
            return true;
        }
        return false;
    }
}
