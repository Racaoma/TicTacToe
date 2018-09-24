using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelFlow : MonoBehaviour
{
    //Panels
    public GameObject[] panels;

    //Music
    public AudioSource music;
    public float fadeTime = 1.75f;

    //Return Methods
    public void exit()
    {
        Application.Quit();
    }

    //Activate Certain Panel
    public void changePanel(int panelToLoad)
    {
        disableAllPanels();
        panels[panelToLoad].SetActive(true);
    }

    //Disable All Panels
    private void disableAllPanels()
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    //Multi Player
    public void multiPlayer()
    {
        GameManager.typePlayer1 = PlayerType.Human_Local;
        GameManager.typePlayer2 = PlayerType.Human_Local;
    }

    //Difficulty Select Methods
    public void setDifficultyEasy()
    {
        GameManager.typePlayer1 = PlayerType.Human_Local;
        GameManager.typePlayer2 = PlayerType.AI_Easy;
    }

    public void setDifficultyMedium()
    {
        GameManager.typePlayer1 = PlayerType.Human_Local;
        GameManager.typePlayer2 = PlayerType.AI_Medium;
    }

    public void setDifficultyHard()
    {
        GameManager.typePlayer1 = PlayerType.Human_Local;
        GameManager.typePlayer2 = PlayerType.AI_Hard;
    }

    //First Move Methods
    public void setFirstMoveHuman()
    {
        GameManager.firstMove = Player.Player1;
    }

    //First Move Methods
    public void setFirstMoveOpponnent()
    {
        GameManager.firstMove = Player.Player2;
    }

    //First Move Methods
    public void setFirstMoveRandom()
    {
        GameManager.firstMove = Player.None;
    }

    //Load Game & Fade Music
    public void loadGame()
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

        //Change Scene
        MyNetworkManager.singleton.ServerChangeScene("Game");
    }
}
