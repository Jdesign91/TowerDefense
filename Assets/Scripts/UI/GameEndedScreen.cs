using UnityEngine;
using UnityEngine.UI;

// Game ended dialog. Goes on two seperate prefabs that get loaded depending on win or lose.
public class GameEndedScreen : MonoBehaviour
{
    public Button playAgain;
    public Button mainMenu;

    private void Start()
    {
        playAgain.onClick.AddListener(onPlayGame);
        mainMenu.onClick.AddListener(onMainMenu);
    }

    private void onPlayGame()
    {
        Main.instance.resetGameboard();
        GameObject.Destroy(gameObject);
    }

    private void onMainMenu()
    {
        Main.instance.unloadGame();
        Main.instance.loadMainMenu();
        GameObject.Destroy(gameObject);
    }
}


