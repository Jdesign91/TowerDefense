using UnityEngine;
using UnityEngine.UI;

// Main Menu UI class
public class MainMenu : MonoBehaviour
{ 
    public Button playGame;

    private void Start()
    {
        playGame.onClick.AddListener(onPlayGame);
    }

    private void onPlayGame()
    {
        Main.instance.playGame();
        GameObject.Destroy(gameObject);
    }
}
    

