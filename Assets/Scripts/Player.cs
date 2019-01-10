using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    GameObject LvlManager;
    Camera level_cam;
    GameObject PausedMenu;
    GameObject TutorialBoard;
    GameObject Indicators;
    public GameObject CurrentLevel;

    public List<Equip> equipped_items = new List<Equip>();

    public Vector2 current_origin;
    public float current_level_width;
    public float current_level_height;


    public float collected = 0;
    public int ability_counter = 0;
    public float timer = 0;
    bool level_started = false;


    private void Awake() {
        SetUpGameObjects();
    }

    // Use this for initialization
    void Start() {
        CurrentLevel = LvlManager.GetComponent<LevelManager>().GetLevel(1);
        LoadLevel(CurrentLevel);

        level_cam.GetComponent<Camera>().orthographicSize = CameraController.scale;
        UpdateEquipment();
    }
	
	// Update is called once per frame
	void Update () {
        if (level_started) timer += Time.deltaTime;

        Inputs();
    }

    // Initialize GameObjects
    public void SetUpGameObjects() {
        LvlManager = GameObject.Find("/Level Manager");
        level_cam = GameObject.Find("/Main Camera").GetComponent<Camera>();
        PausedMenu = GameObject.Find("/Canvas");
        Indicators = GameObject.Find("/Indicators");
    }

    // Load a level, turn off UIs, and put Player at start of level
    public void LoadLevel(GameObject level) {
        if (level.GetComponent<Level>()) {
            GetLevelData(level);
            level_cam.GetComponent<CameraController>().MoveCamera(new Vector3(current_origin.x + CameraController.offset_x, current_origin.y + CameraController.offset_y, CameraController.offset_z));

            // Removes and Replaces all collectibles
            CurrentLevel.GetComponent<Level>().level_entities.GetComponent<LevelEntities>().Clean();
            CurrentLevel.GetComponent<Level>().level_entities.GetComponent<LevelEntities>().Populate();

            Restart();

            PausedMenu.GetComponent<PauseMenu>().Resume();

            SetAbilities(level);

            UpdateIndicators();
        } 
    }

    // Get level information, transfer some to the Movement script
    private void GetLevelData(GameObject level) {
        CurrentLevel = level;
        current_level_height = level.GetComponent<Level>().map_height;
        current_level_width = level.GetComponent<Level>().map_width;
        GetComponent<Movement>().origin = level.GetComponent<Level>().origin;
        current_origin = level.GetComponent<Level>().origin;
    }

    // Enable/Disable abilities based on current level
    public void SetAbilities(GameObject level) {
        GetComponent<Movement>().SetAbilityPermissions(
            level.GetComponent<Level>().extend_allowed, 
            level.GetComponent<Level>().retract_allowed, 
            level.GetComponent<Level>().flip_x_allowed, 
            level.GetComponent<Level>().flip_y_allowed);

    }

    // Moves you to the start of the level, restarts counters
    public void Restart() {
        Vector2 spawn = CurrentLevel.GetComponent<Level>().level_entities.GetComponent<LevelEntities>().spawn;
        GetComponent<Movement>().ResetPosition(spawn);
        level_started = false;
        timer = 0;
        ability_counter = 0;
        collected = 0;
    }

    // Check Inputs, use abilities
    private void Inputs() {
        if ((Input.anyKey) && (!Input.GetKey(KeyCode.Mouse0))&&(!Input.GetKey(KeyCode.Escape))&&(!PauseMenu.Paused)) level_started = true;

        if (SceneManager.GetActiveScene().name != "Menu") {

            if (Input.GetKeyUp(KeyCode.R)) {
                LoadCurrentLevel();
            }

            // If the player presses a number button, Load that level
            for (int i = 1; i <= 9; i++) {
                if (Input.GetKeyDown("" + i)) LoadLevelX(i);
            }
        }
    }

    public void LoadCurrentLevel() {
        LoadLevel(CurrentLevel);
    }

    void LoadLevelX(int num) {
        GameObject new_level = LvlManager.GetComponent<LevelManager>().GetLevel(num);
        if (new_level) LoadLevel(new_level);
    }

    public void LoadNextLevel() {
        LoadLevelX(CurrentLevel.GetComponent<Level>().level_num+1);
    }

    // Returns true if the destination isn't in the terrain of the current level
    public bool SafeDestination(Vector2 destination) {
        return CurrentLevel.GetComponent<Level>().terrain_manager.GetComponent<TerrainManager>().SafeDestination(destination);
    }

    // Creates a child GameObject with the item's sprite at the right offset, scale, and rotation
    public void EquipItem(Equip new_item) {

        Quaternion old_rot = transform.rotation;
        transform.rotation = new_item.rotation;
        equipped_items.Add(new_item);

        GameObject accessory = new GameObject();
        accessory.name = "Accessory";
        accessory.AddComponent<SpriteRenderer>();

        Sprite new_image = Resources.Load<Sprite>(new_item.image_name);
        accessory.GetComponent<SpriteRenderer>().sprite = new_image;
        accessory.transform.parent = transform;

        Vector3 localoffset = new Vector3(new_item.offset.x * transform.localScale.x, new_item.offset.y * transform.localScale.y, new_item.offset.z);
        accessory.transform.position = transform.position + localoffset;
        accessory.transform.localScale = new_item.scale;
        transform.rotation = old_rot;
    }

    // Destroys children of the Player with the corresponding name
    public void UnequipItem(Equip old_item) {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if ((child.name == "Accessory") && (child.GetComponent<SpriteRenderer>().sprite.name == old_item.image_name)) {
                DestroyObject(child);
                equipped_items.Remove(old_item);
            }
        }
        for (int i = 0; i < equipped_items.Count; i++) {
            Equip item = equipped_items[i];
            if (item.image_name == old_item.image_name) equipped_items.Remove(item);
        }
    }

    // Equips Player with all Saved Items
    void UpdateEquipment() {
        List<Equip> equips = LvlManager.GetComponent<LevelManager>().DataBank.GetComponent<Data>().character_data.Equips;
        for (int i = 0; i < equips.Count; i++) {
            Equip item = equips[i];
            EquipItem(item);
        }
    }

    // Set all indicators as Active/Inactive based on current level restrictions
    void UpdateIndicators() {
        if (Indicators) {
            for (int i = 0; i < Indicators.transform.childCount; i++) {
                GameObject indicator = Indicators.transform.GetChild(i).gameObject;
                if (indicator.name.Contains("Inner Circle")) {
                    indicator.SetActive(GetComponent<Movement>().retract_allowed);
                }
                if (indicator.name.Contains("Inner Line")) {
                    indicator.SetActive(GetComponent<Movement>().retract_allowed);
                }
                if (indicator.name.Contains("Outer Circle")) {
                    indicator.SetActive(GetComponent<Movement>().extend_allowed);
                }
                if (indicator.name.Contains("Outer Line")) {
                    indicator.SetActive(GetComponent<Movement>().extend_allowed);
                }
                if (indicator.name.Contains("X Circle")) {
                    indicator.SetActive(GetComponent<Movement>().flip_x_allowed);
                }
                if (indicator.name.Contains("Y Circle")) {
                    indicator.SetActive(GetComponent<Movement>().flip_y_allowed);
                }
            }
        }
    }

    // When you leave a tutorial collider, turn it off
    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Tutorial") {
            TutorialBoard.SetActive(false);
        }
    }

    // While in a tutorial collider, keep it on
    private void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.tag == "Tutorial") {
            TutorialBoard.SetActive(true);
        }
    }

    // When Player enters a collider, perform corresponding action
    private void OnTriggerEnter2D(Collider2D collider) {
        // When you reach the exit with enough collectibles, update highscores, prepare the level end UI, and save highscores
        if ((collider.gameObject.tag == "Exit") && (collected >= CurrentLevel.GetComponent<Level>().required_acorns)) {
            CurrentLevel.GetComponent<Level>().UpdateHighScores(timer, ability_counter);
            float highscore_timer = CurrentLevel.GetComponent<Level>().time_highscore;
            int highscore_abilities = CurrentLevel.GetComponent<Level>().ability_highscore;
            float time_target = CurrentLevel.GetComponent<Level>().target_time;
            int ability_target = CurrentLevel.GetComponent<Level>().target_moves;
            PausedMenu.GetComponent<PauseMenu>().GoToLevelEnd(timer, highscore_timer, time_target, ability_counter, highscore_abilities, ability_target);
            LvlManager.GetComponent<LevelManager>().SaveData();
        }
        // When you touch a CameraPanner, change corresponding UI elements and SlowPan the camera
        if (collider.gameObject.GetComponent<CameraPanner>()) {
            level_cam.GetComponent<CameraController>().SlowMoveCamera(collider.gameObject.GetComponent<CameraPanner>().target);
            collider.GetComponent<CameraPanner>().TextChange();
        }
        // When you touch a world loader, load the corresponding world
        if (collider.gameObject.tag == "Load World") {
            LvlManager.GetComponent<LevelManager>().DataBank.GetComponent<Data>().character_data.Equips = equipped_items;
            LvlManager.GetComponent<LevelManager>().SaveData();
            SceneManager.LoadScene("World 1");
        }
        // When you touch a tutorial collider, set the tutorial UI to the tutorial collider's message
        if (collider.gameObject.tag == "Tutorial") {
            for (int i = 0; i < PausedMenu.transform.childCount; i++) {
                GameObject child = PausedMenu.transform.GetChild(i).gameObject;
                if (child.name.Contains("Tutorial Panel")) {
                    TutorialBoard = child;
                    TutorialBoard.transform.GetChild(0).GetComponent<Text>().text = collider.GetComponent<TutorialMessage>().message.Replace("\\n", "\n");
                    TutorialBoard.SetActive(true);
                }
            }
        }
    }
}
