using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    //References
    public AudioControl audioControl;
    public GameObject boardObject;

    //Return to Main Menu
    public void loadTitleScreen(bool fade)
    {
        if(fade) StartCoroutine(audioControl.FadeMusic());
        else SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }

    //Setup Game
    public void startGame()
    {
        this.GetComponent<GameLogic>().enabled = true;
        boardObject.SetActive(true);
        this.GetComponent<GameView>().enabled = true;
        audioControl.playMusic();
    }
}
