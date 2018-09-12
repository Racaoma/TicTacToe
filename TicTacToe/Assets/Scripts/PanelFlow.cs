using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PanelFlow : MonoBehaviour
{
    //Panels
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject creditsPanel;

    //Music
    public AudioSource music;
    public float fadeTime = 1.75f;

    //Return Methods
    public void exit()
    {
        Application.Quit();
    }

    public void backPanel2()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    public void backPanel3()
    {
        panel2.SetActive(true);
        panel3.SetActive(false);
    }

    //Credits
    public void credits()
    {
        panel1.SetActive(false);
        creditsPanel.SetActive(true);
    }

    //Credits
    public void returnFromCredits()
    {
        creditsPanel.SetActive(false);
        panel1.SetActive(true);
    }

    //Single Player
    public void singlePlayer()
    {
        panel1.SetActive(false);
        panel2.SetActive(true);
    }

    //Multi Player
    public void multiPlayer()
    {
        GameManager.typePlayer1 = PlayerType.Human;
        GameManager.typePlayer2 = PlayerType.Human;

        //Load Game!
        loadGame();
    }

    //Difficulty Select Methods
    public void setDifficultyEasy()
    {
        GameManager.typePlayer1 = PlayerType.Human;
        GameManager.typePlayer2 = PlayerType.AI_Easy;
        loadLastPanel();
    }

    public void setDifficultyMedium()
    {
        GameManager.typePlayer1 = PlayerType.Human;
        GameManager.typePlayer2 = PlayerType.AI_Medium;
        loadLastPanel();
    }

    public void setDifficultyHard()
    {
        GameManager.typePlayer1 = PlayerType.Human;
        GameManager.typePlayer2 = PlayerType.AI_Hard;
        loadLastPanel();
    }

    //First Move Methods
    public void setFirstMoveHuman()
    {
        GameManager.firstMove = FirstMove.Human;

        //Load Game!
        loadGame();
    }

    //First Move Methods
    public void setFirstMoveAI()
    {
        GameManager.firstMove = FirstMove.AI;

        //Load Game!
        loadGame();
    }

    //First Move Methods
    public void setFirstMoveRandom()
    {
        GameManager.firstMove = FirstMove.Random;

        //Load Game!
        loadGame();
    }

    //Inner Workings
    private void loadLastPanel()
    {
        panel2.SetActive(false);
        panel3.SetActive(true);
    }

    private void loadGame()
    {
        StartCoroutine(FadeMusic());
    }

    public IEnumerator FadeMusic()
    {
        while (music.volume > .1F)
        {
            music.volume = Mathf.Lerp(music.volume, 0F, fadeTime * Time.deltaTime);
            yield return 0;
        }
        music.volume = 0;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
