using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntities : MonoBehaviour {

    public List<Entity> Entities = new List<Entity>();
    public GameObject acorn_prefab;
    public GameObject enter_prefab;
    public GameObject exit_prefab;
    int entity_count;
    public Vector2 spawn = new Vector2(0, 0);

    // Use this for initialization
    void Awake () {
        entity_count = transform.childCount;
        for (int i = 0; i<entity_count; i++) {
            GameObject entity = transform.GetChild(i).gameObject;
            Entity record = new Entity {position = entity.transform.position, type = entity.name};
            Entities.Add(record);
            if (record.type.Contains("enter")) spawn = record.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clean() {
        entity_count = transform.childCount;
        for (int i = 0; i < entity_count; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void Populate() {
        for(int i = 0; i<Entities.Count; i++) {
            Entity entity = Entities[i];
            if (entity.type.Contains("Acorn")) MakeAcorn(entity);
            if (entity.type.Contains("enter")) MakeEnter(entity);
            if (entity.type.Contains("exit")) MakeExit(entity);
        }
    }
    public void MakeAcorn(Entity entity) {
        GameObject acorn = Instantiate(acorn_prefab, new Vector3(entity.position.x, entity.position.y, 5), Quaternion.identity);
        acorn.transform.parent = this.transform;
    }
    public void MakeEnter(Entity entity) {
        GameObject enter = Instantiate(enter_prefab, new Vector3(entity.position.x, entity.position.y, 5), Quaternion.identity);
        enter.transform.parent = this.transform;
    }
    public void MakeExit(Entity entity) {
        GameObject exits = Instantiate(exit_prefab, new Vector3(entity.position.x, entity.position.y, 5), Quaternion.identity);
        exits.transform.parent = this.transform;
    }

    public int AcornNum() {
        int num = 0;
        for (int i = 0; i< Entities.Count; i++) {
            Entity entity = Entities[i];
            if (entity.type.Contains("Acorn")) num++;
        }
        return num;
    }
}

public class Entity{
    public string type;
    public Vector2 position;
}
