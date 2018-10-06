using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public delegate void LoadDelegate();

public class PanelFlow : MonoBehaviour
{
    //Panels
    public GameObject[] panels;

    //Music
    public AudioSource music;
    public float fadeTime = 1.75f;

    //Singleton
    private static PanelFlow instance;
    public static PanelFlow Instance
    {
        get
        {
            return instance;
        }
    }

    //On Object Awake
    private void Awake()
    {
        //Check Singleton
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    //On Object Destroy (Safeguard)
    public void OnDestroy()
    {
        instance = null;
    }

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
    public void loadGame(LoadDelegate delegateFunction)
    {
        StartCoroutine(StartLoad(delegateFunction));
    }

    //Load Game & Fade Music
    public IEnumerator StartLoad(LoadDelegate delegateFunction)
    {
        //Get Network Manager
        MyNetworkManager networkManager = FindObjectOfType<MyNetworkManager>();
        networkManager.loadingLevel = true;

        while (music.volume > .1F)
        {
            music.volume = Mathf.Lerp(music.volume, 0F, fadeTime * Time.deltaTime);
            yield return 0;
        }
        music.volume = 0;

        //Change Scene & Wait for it to become active (next frame)
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
        yield return new WaitForEndOfFrame();

        //Call Delegated Function
        networkManager.loadingLevel = false;
        delegateFunction();
    }
}
