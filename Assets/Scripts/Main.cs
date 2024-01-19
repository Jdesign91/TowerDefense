using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basically app entry point.
public class Main : MonoBehaviour
{
    // These are the current locations of the prefabs needed to run the game. In the future these can be data driven to load themed prefabs or perhaps some sort of scenarios
    private const string MAIN_MENU_PREFAB_LOCATION = "Prefabs/UI/Main Menu";
    private const string DEFAULT_MAP_LOCATION = "Prefabs/Map/Basic Map";
    private const string DEFAULT_UI_LOCATION = "Prefabs/UI/In Game Menu";

    // This class is intended to keep control over everything we have going on
    public static Main instance;

    // Anchors for attaching major game pieces
    public GameObject mainMenuAnchor;
    public GameObject gameBoardAnchor;
    public GameObject dialogAnchor;
    public GameObject mapParent;
    public GameObject menuParent;

    // Loads configs
    InfoParser parser = new InfoParser();

    // Loaded on the fly as needed.
    GameBoard activeBoard;
    MainMenu mainMenuScreen;
    InGameMenu inGameMenu;

    private void Awake()
    {
        instance = this;

        // Normally this is all stuff that'd get parsed in 
        // some sort of larger map config. For now it's broken up for ease of use.
        // Also should probably include some better data validation. Wouldn't want to start with something unusable.
        parser.setupCreeps();
        parser.setupTowers();
        parser.setupAmmo();
        parser.setupWaves();

        // Start with main menu screen.
        mainMenuScreen = Loader.loadObjectOfType<MainMenu>(MAIN_MENU_PREFAB_LOCATION);
        mainMenuScreen = GameObject.Instantiate<MainMenu>(mainMenuScreen, mainMenuAnchor.transform);
    }

    public void playGame()
    {
        // Load right into game.
        activeBoard = Loader.loadObjectOfType<GameBoard>(DEFAULT_MAP_LOCATION);
        inGameMenu = Loader.loadObjectOfType<InGameMenu>(DEFAULT_UI_LOCATION);

        activeBoard = GameObject.Instantiate(activeBoard, mapParent.transform);
        inGameMenu = GameObject.Instantiate(inGameMenu, menuParent.transform);

        inGameMenu.gameBoardRef = activeBoard;
    }

    public void resetGameboard()
    {
        // Totally resets board and starts the next game.
        activeBoard.reset();
    }

    public void unloadGame()
    {
        // Actually destorys all gameboard stuff including UI
        GameObject.Destroy(activeBoard.gameObject);
        GameObject.Destroy(inGameMenu.gameObject);
    }

    public void loadMainMenu()
    { 
        // Load up main menu prefab
        mainMenuScreen = Loader.loadObjectOfType<MainMenu>(MAIN_MENU_PREFAB_LOCATION);
        mainMenuScreen = GameObject.Instantiate<MainMenu>(mainMenuScreen, mainMenuAnchor.transform);
    }

    public void loadDialog(string path)
    {
        GameObject dialogToLoad = Loader.loadObjectOfType<GameObject>(path);
        dialogToLoad = GameObject.Instantiate(dialogToLoad, dialogAnchor.transform);
    }
}
